using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using IMDbion_MovieHandlerService.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;

namespace IMDbion_MovieHandlerService.ExceptionHandler
{
    public class CustomExceptionHandler
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (ex)
            {
                case NotFoundException _:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case FieldNullException _:
                    response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                    break;
                case BadRequestException _:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await response.WriteAsync(JsonConvert.SerializeObject(new { Error = "An error occurred while processing your request." }));
                    throw ex;
            }
        }
    }
}
