namespace ContestPark.WebApi
{
    public partial class Startup
    {
        private void SwanggerConfigureServices(IServiceCollection services)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "ContestPark API",
                    Description = "Bilgi yarışması web API",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Ali Aldemir", Email = "", Url = "https://www.facebook.com/aldemirali93" }
                });
            });
        }

        private void SwanggerConfigure(IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "apidocs";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ContestPark API V1");
            });
        }
    }
}