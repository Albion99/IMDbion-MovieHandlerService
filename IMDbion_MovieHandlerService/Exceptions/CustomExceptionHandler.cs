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
            string errorMessage;

            switch (ex)
            {
                case NotFoundException nfe:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorMessage = nfe.Message;
                    break;

                case CantBeNullException cbe:
                    response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                    errorMessage = cbe.Message;
                    break;
                case BadRequestException bre:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorMessage = bre.Message;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorMessage = "An error occurred while processing your request.";
                    break;
            }

            await response.WriteAsync(JsonConvert.SerializeObject(new { Error = errorMessage }));
            throw ex;
        }
    }
}
