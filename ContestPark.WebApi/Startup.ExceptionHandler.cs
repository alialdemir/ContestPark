namespace ContestPark.WebApi
{
    public partial class Startup
    {
        private void ExceptionHandlerConfigure(IApplicationBuilder app)
        {
            app.UseExceptionHandler(
        builder =>
        {
            builder.Run(
            async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>();
                if (exception != null)
                {
                    var code = HttpStatusCode.InternalServerError; // 500 if unexpected
                                                                   /* if (exception is NotificationException) code = HttpStatusCode.NotFound;
                                                                    else if (exception is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
                                                                    else */
                    if (exception.Error is NotificationException) code = HttpStatusCode.BadRequest;
                    else LoggingService.LogError($"{exception.GetType().Name} - Message:{exception.Error.Message} - StackTrace:{exception.Error.StackTrace}");

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)code;
                    await context.Response.WriteAsync(exception.Error.Message);
                }
            });
        });
        }
    }
}