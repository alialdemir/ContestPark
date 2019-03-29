﻿namespace ContestPark.WebApi.Providers
{
    /// <summary>
    /// Provides options for <see cref="TokenProviderMiddleware"/>.
    /// </summary>
    public class TokenProviderOptions
    {
        private readonly string secretKey = "mysupersecret_secretkey!123";

        public TokenProviderOptions()
        {
            SigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);
        }

        public SymmetricSecurityKey SigningKey { get; set; }

        /// <summary>
        ///  The Issuer (iss) claim for generated tokens.
        /// </summary>
        public string Issuer { get; set; } = "ExampleIssuer";

        /// <summary>
        /// The Audience (aud) claim for the generated tokens.
        /// </summary>
        public string Audience { get; set; } = "ExampleAudience";

        /// <summary>
        /// The expiration time for the generated tokens.
        /// </summary>
        /// <remarks>The default is five minutes (300 seconds).</remarks>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(365);

        /// <summary>
        /// The signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        /// <summary>
        /// Generates a random value (nonce) for each generated token.
        /// </summary>
        /// <remarks>The default nonce is a random GUID.</remarks>
        public Func<Task<string>> NonceGenerator { get; set; }
            = new Func<Task<string>>(() => Task.FromResult(Guid.NewGuid().ToString()));
    }
}