using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Server.Dtos;
using Wd3w.AspNetCore.EasyTesting;
using Wd3w.AspNetCore.EasyTesting.Authentication;
using Wd3w.AspNetCore.EasyTesting.EntityFrameworkCore;
using Wd3w.AspNetCore.EasyTesting.Hestify;
using Xunit;

namespace Tests.Integration
{
    public class IndexPageTests : EasyTestingTestBase
    {
        [Fact]
        public async Task Hello()
        {
            // Given 
            SUT.ReplaceSqliteInMemoryDbContext<UserContext>();

            // When
            var response = await SUT
                .Resource("/Users/register")
                .WithJsonBody(new UserDto
                {
                    UserName = "HELLO",
                    Email = "HELLO@gmail.com",
                    Password = "password"
                })
                .PostAsync();

            // Then
            response.ShouldBeOk();
        }

        [Fact]
        public async Task Hello2()
        {
            // Given 
            var client = SUT.ReplaceSqliteInMemoryDbContext<UserContext>()
                .SetupFixture<UserContext>(async context =>
                {
                    context.Users.Add(new User
                    {
                        Email = "test@test.com",
                        Username = "test"
                    });
                    await context.SaveChangesAsync();
                })
                .AllowAuthentication(JwtBearerDefaults.AuthenticationScheme, new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "test")
                }, "Bearer"))
                .NoUserAuthentication()
                .DenyAuthentication()
                .CreateClient();

            // When
            var hello = await client.GetAsync("/Users");
            // Then
            hello.ShouldBe(HttpStatusCode.OK);
        }
    }
}