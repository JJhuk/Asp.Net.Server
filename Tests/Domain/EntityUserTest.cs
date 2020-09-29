using System.Threading.Tasks;
using Domain;
using Domain.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Domain
{
    public class EntityUserTest
    {
        private readonly UserContext _db;

        public EntityUserTest()
        {
            var option = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase("Test")
                .Options;

            _db = new UserContext(option);
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
        }

        private async Task CreateUser()
        {
            await _db.Users.AddAsync(new User());
            await _db.SaveChangesAsync();
        }

        [Fact]
        public async Task UserEntity_Should_HasAuditFields_When_AuditIsCreated()
        {
            await CreateUser();

            var userCount = await _db.Users.AsQueryable().CountAsync();

            userCount.Should().Be(1);
        }

        [Fact]
        public async Task UserEntity_Should_SetCreatedAt_When_UserCreated()
        {
            //given
            await CreateUser();

            //when
            var user = await _db.Users.SingleAsync();

            //then
            user.CreatedAt.Should().NotBeNull();
        }
    }
}