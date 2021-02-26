using System;
using LangAppApi.Service.Features.CustomerFeatures.Commands;
using LangAppApi.Service.Features.CustomerFeatures.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace LangAppApi.Controllers.V1
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/customers")]
    [ApiVersion("1.0")]
    public class CustomerController : ControllerBase
    {
        private IMediator _mediator;
        private IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet, MapToApiVersion("1.0")]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllCustomerQuery()));
        }

        [HttpGet("{id}"), MapToApiVersion("1.0")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await Mediator.Send(new GetCustomerByIdQuery { Id = id }));
        }

        [HttpDelete("{id}"), MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new DeleteCustomerByIdCommand { Id = id }));
        }

        [HttpPut("{id}"), MapToApiVersion("1.0")]
        public async Task<IActionResult> Update(Guid id, UpdateCustomerCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }
    }
}