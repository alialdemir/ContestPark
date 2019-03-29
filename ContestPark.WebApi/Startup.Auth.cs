using ContestPark.WebApi.Providers;

namespace ContestPark.WebApi
{
    public partial class Startup
    {
        private void AuthConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                // User settings
                options.User.RequireUniqueEmail = true;
            })
             .AddEntityFrameworkStores<ContestParkContext>()
             .AddErrorDescriber<CustomIdentityErrorDescriber>()
             .AddDefaultTokenProviders();

            TokenProviderOptions tokenProvider = new TokenProviderOptions();
            services.AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
            {
                options.Audience = tokenProvider.Audience;
                options.ClaimsIssuer = tokenProvider.Issuer;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = tokenProvider.SigningKey,
                    ValidateIssuer = true,
                    ValidAudience = tokenProvider.Audience,
                    ValidIssuer = tokenProvider.Issuer,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        private void AuthConfigure(IApplicationBuilder app)
        {
            app.UseMiddleware<TokenProviderMiddleware>();
            app.UseAuthentication();
        }
    }
}