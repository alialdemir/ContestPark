namespace ContestPark.WebApi
{
    public partial class Startup
    {
        private void LoggerConfigure(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Logging
            loggerFactory
                .AddFile("log/MyLogs/Cp-{Date}.txt");
            if (env.IsDevelopment())
            {
                loggerFactory
                   .AddConsole(Configuration.GetSection("Logging"))
                   .AddDebug();
            }
            LoggingService.LoggerFactory = loggerFactory;
        }
    }
}