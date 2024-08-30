using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PubApi;
using Microsoft.Extensions.DependencyInjection;
using PublishData;
using PubApi.DTOs;
using System.Net.Http.Json;
using System.Net;

namespace PubTests
{
    [TestClass]
    public class ApiTests
    {
        [TestMethod]
        public async Task ApiIsRunning()
        {
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();
            var response = await client.GetStringAsync("/weatherforecast");
            Assert.AreEqual("[{\"date", response.Substring(0, 7));
        }

        [TestMethod]
        public async Task CanRetrieveAnAuthorDTO()
        {
            await using var application = new CustomWebApplicationFactory<Program>();
            CreateAndSeedDatabase(application);
            using var client = application.CreateClient();
            var authorDTO = await client.GetFromJsonAsync<AuthorDto>("/api/author/1");
            Assert.IsInstanceOfType(authorDTO, typeof(AuthorDto));
        }

        [TestMethod]
        public async Task CanInsertAnAuthor()
        {
            var authorDTO = new AuthorDto(0, "John", "Doe");
            await using var application = new CustomWebApplicationFactory<Program>();
            CreateAndSeedDatabase(application);
            using var client = application.CreateClient();
            var response = await client.PostAsJsonAsync("/api/author/", authorDTO);
            var author = await response.Content.ReadFromJsonAsync<AuthorDto>();
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Assert.AreEqual(0, author.AuthorId);
        }

        private static void CreateAndSeedDatabase(WebApplicationFactory<Program> appFactory)
        {
            using (var scope = appFactory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<PubContext>();
                db.Database.EnsureCreated();

            }
        }

    }
}
