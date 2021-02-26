using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using LangAppApi.Domain.Exceptions;

namespace LangAppApi.Infrastructure.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exceptionObj)
            {
                await HandleExceptionAsync(context, exceptionObj, _logger);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<CustomExceptionMiddleware> logger)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (ex)
            {
                case ApiException e:
                    _logger.LogError(e, e.Message);
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

                case AuthException e:
                    _logger.LogError(e, e.Message);
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case BadRequestException e:
                    _logger.LogError(e, e.Message);
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case NotFoundException e:
                    _logger.LogError(e, e.Message);
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case DeleteFailureException e:
                    _logger.LogError(e, e.Message);
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

                default:
                    // unhandled error
                    _logger.LogError(ex, "Une erreur est survenue");
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonConvert.SerializeObject(new { ErrorMessage = ex.Message });
            return context.Response.WriteAsync(result);
        }
    }
}