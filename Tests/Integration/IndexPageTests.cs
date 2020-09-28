using System.Threading.Tasks;
using Domain;
using Server.Dtos;
using Wd3w.AspNetCore.EasyTesting;
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
    }
}