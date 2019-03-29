namespace ContestPark.WebApi
{
    public partial class Startup
    {
        private void SignalRConfigureServices(IServiceCollection services)
        {
            // Add service and create Policy with options
            services.AddCors();
            // SignalR
            services.AddSignalR(options => options.Hubs.EnableDetailedErrors = true);
        }

        private void SignalRConfigure(IApplicationBuilder app)
        {
            // SignalR
            app.UseCors(
             builder => builder.AllowAnyOrigin()
                 .AllowAnyHeader()
                 .AllowAnyMethod()
                 .AllowCredentials())
             .UseStaticFiles()
             .UseWebSockets();
            app.UseSignalR("/signalr");
        }
    }
}