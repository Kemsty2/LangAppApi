using LangAppApi.Domain.Auth;
using LangAppApi.Domain.Settings;
using LangAppApi.Test.Integration.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Xunit;

namespace LangAppApi.Test.Integration.Base
{
    public class BaseClassFixture : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        protected readonly HttpClient Client;
        private readonly ApplicationUser _user;

        protected BaseClassFixture(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            Client = factory.SetupClient();
            _factory.CreateClient();

            var config = new ConfigurationBuilder().AddJsonFile("./appsettings.Test.json").Build();

            _user = config.GetSection("User").Get<ApplicationUser>();
        }

        protected void SetupClaimsViaHeaders()
        {
            using var scope = _factory.Services.CreateScope();

            var jwtSettings = scope.ServiceProvider.GetRequiredService<IOptions<JwtSettings>>();

            Client.SetClaimsViaHeader(_user, jwtSettings.Value);
        }
    }
}