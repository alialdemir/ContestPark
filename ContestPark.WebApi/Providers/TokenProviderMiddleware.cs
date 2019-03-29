namespace ContestPark.WebApi.Providers
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenProviderMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            LoggingService.LogInformation("Handling request: " + context.Request.Path);
            var authenticateInfo = context.AuthenticateAsync("Bearer").Result;
            var bearerTokenIdentity = authenticateInfo?.Principal;
            if (bearerTokenIdentity != null)
            {
                var tenantIdClaim = bearerTokenIdentity.Claims.First(x => x.Properties.Values.Contains(JwtRegisteredClaimNames.Sub));
                context.User = bearerTokenIdentity;
                return _next(context);
            }
            else if (context.Request.Path.Equals("/token") && context.Request.HasFormContentType && context.Request.Method.Equals("POST")) return _next(context);
            else if (context.Request.Path.StartsWithSegments("/Account/Login") && context.Response.StatusCode == (int)HttpStatusCode.OK)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Task.FromResult<object>(null);
            }
            return _next(context);
        }
    }
}