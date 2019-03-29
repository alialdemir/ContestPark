namespace ContestPark.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
               .CaptureStartupErrors(true)
                .UseStartup<Startup>()
                .Build();
    }
}