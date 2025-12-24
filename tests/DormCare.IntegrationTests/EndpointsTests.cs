using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using DormCare;
using DormCare.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace DormCare.IntegrationTests
{
    public class EndpointsTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public EndpointsTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AdminRooms_Unauthenticated_RedirectsToLogin()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            var res = await client.GetAsync("/Rooms");
            // Not authenticated -> should redirect to login (302)
            Assert.Equal(HttpStatusCode.Redirect, res.StatusCode);
            Assert.Contains("/Account/Login", res.Headers.Location.ToString());
        }

        [Fact]
        public async Task Register_Post_CreatesUser()
        {
            var client = _factory.CreateClient();

            var form = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("email","testuser@example.com"),
                new KeyValuePair<string,string>("password","pass123")
            });

            var res = await client.PostAsync("/Account/Register", form);

            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var user = db.Users.FirstOrDefault(u => u.Email == "testuser@example.com");
            Assert.NotNull(user);
        }
    }
}
