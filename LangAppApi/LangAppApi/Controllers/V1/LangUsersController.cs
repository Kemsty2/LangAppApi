using AutoMapper;
using LangAppApi.ControllerUtilities;
using LangAppApi.Domain.Common;
using LangAppApi.Domain.Queries;
using LangAppApi.Infrastructure.ViewModel;
using LangAppApi.Service.Features.LangFeatures.Commands;
using LangAppApi.Service.Features.LangFeatures.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LangUser = LangAppApi.Domain.Entities.LangUser;

namespace LangAppApi.Controllers.V1
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/langusers")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class LangUsersController : ControllerBase
    {
        private IMediator _mediator;
        private IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        private readonly IMapper _mapper;

        public LangUsersController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: api/langusers
        /// <summary>
        /// Get and Filter All User Lang
        /// </summary>
        /// <returns></returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Response<IEnumerable<LangViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OutPutModel<LangViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromQuery] LangQuery query)
        {
            if (query == null || !query.IsPaged)
            {
                var result = await Mediator.Send(new GetAllLangQuery());
                return Ok(new Response<IEnumerable<LangViewModel>>(_mapper.Map<IEnumerable<LangUser>, IEnumerable<LangViewModel>>(result)));
            }

            if (!query.IsValid())
            {
                return BadRequest(query.ValidateQuery());
            }

            var items = new PagedList<LangUser>(await Mediator.Send(new GetPagingLangUsersQuery { Query = query }));
            return Ok(new OutPutModel<LangViewModel>(items.GetHeader(), _mapper.Map<List<LangUser>, List<LangViewModel>>(items.List)));
        }

        /// <summary>
        /// Create Lang of a User
        /// </summary>
        /// <param name="command">The Command to create User Language</param>
        /// <returns></returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Response<LangViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CreateLangCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await Mediator.Send(command);

            return Created(new Uri($"/api/v1/langusers/{result.Id}", UriKind.Relative),
                new Response<LangViewModel>(_mapper.Map<LangUser, LangViewModel>(result), $"The user lang with id : {result.Id} have been created"));
        }

        /// <summary>
        /// Get User Lang by its identifier
        /// </summary>
        /// <param name="id">Identifier of the user lang</param>
        /// <returns></returns>
        [HttpGet("{id}"), MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Response<LangViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetLangByIdQuery { Id = id });

            return Ok(new Response<LangViewModel>(_mapper.Map<LangUser, LangViewModel>(result)));
        }

        /// <summary>
        /// Delete User Lang By its Identifier
        /// </summary>
        /// <param name="id">Identifier of the user lang</param>
        /// <returns></returns>
        [HttpDelete("{id}"), MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new DeleteLangByIdCommand { Id = id });
            return Ok(new Response<Guid>(result, $"The user lang {id} have been deleted"));
        }

        /// <summary>
        /// Update an user lang
        /// </summary>
        /// <param name="id">Identifier of the user lang</param>
        /// <param name="command">The Update element</param>
        /// <returns></returns>
        [HttpPut("{id}"), MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Response<LangViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateLangCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            command.Id = id;
            var result = await Mediator.Send(command);
            return Ok(new Response<LangViewModel>(_mapper.Map<LangUser, LangViewModel>(result), $"The User Lang with id : {id} have been updated"));
        }
    }
}