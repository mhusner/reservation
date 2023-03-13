using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ReservationsApi.Contracts;
using ReservationsApi.Data;
using Xunit;

namespace ReservationsApi.Tests
{
    public class CoursesTests
    {
        [Fact]
        public async Task GetRegistration_WhenExists_ReturnsExpectedData()
        {
            await using var app = CreateApplication();
            using HttpClient client = app.CreateClient();

            Registration result = await client.GetFromJsonAsync<Registration>("/registrations/s8skjaus6");

            result.Should().BeOfType<Registration>();
            result.Id.Should().Be("s8skjaus6");
            result.Name.Should().Contain("Holec");
        }

        [Fact]
        public async Task GetRegistration_WhenNotExists_ReturnsProblem404()
        {
            await using var app = CreateApplication();
            using var client = app.CreateClient();

            HttpResponseMessage response = await client.GetAsync("/registrations/00000000A");
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task CreateRegistration_WhenSuccess_ReturnsExpectecData()
        {
            await using var app = CreateApplication();
            using var client = app.CreateClient();

            RegistrationCreate registration = new RegistrationCreate();
            registration.Date = DateTime.Now.Date.AddDays(1);
            registration.Name = "John Doe";

            var result = await client.PostAsJsonAsync("/registrations", registration);
            var resultObj = await result.Content.ReadFromJsonAsync<Registration>();

            result.StatusCode.Should().Be(201);
            result.Headers.Location.Should().NotBeNull();
            resultObj.Name.Should().Be("John Doe");
            resultObj.Id.Should().NotBeNull();
        }

        private RestApplication CreateApplication()
        {
            RestApplication app = new RestApplication();
            using var scope = app.Services.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            uow.Database.EnsureDeleted();
            uow.Database.EnsureCreated();
            uow.AddTestDataForCourses();

            return app;
        }
    }
}