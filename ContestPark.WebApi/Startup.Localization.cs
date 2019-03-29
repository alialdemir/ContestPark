namespace ContestPark.WebApi
{
    public partial class Startup
    {
        private const string enUSCulture = "en-US";
        private const string trTRCulture = "tr-TR";

        private CultureInfo[] SupportedCultures
        {
            get
            {
                return new[] { new CultureInfo(enUSCulture), new CultureInfo(trTRCulture) };
            }
        }

        private void LocalizationConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "ContestPark.Common.AppResources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(enUSCulture);
                options.SupportedCultures = SupportedCultures;
            });
        }

        private void LocalizationConfigure(IApplicationBuilder app)
        {
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(enUSCulture),
                SupportedCultures = SupportedCultures,
                SupportedUICultures = SupportedCultures
            });
        }
    }
}