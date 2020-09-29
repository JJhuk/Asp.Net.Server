using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using FluentAssertions;
using Server.Services;
using Tests.Domain;
using Tests.Helpers;
using Xunit;

namespace Tests
{
    public class UserServiceTest
    {
        private readonly TestUserContext _db;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _db = new TestUserContext();
            _userService = new UserService(_db.Db);
        }

        [Fact]
        public void Authenticate_Should_ReturnFalse_When_PasswordIncorrect()
        {
            //given
            CreateDefaultUser();

            //When
            var result = _userService.Authenticate("Jamie", "PASSWORD");

            //Then
            result.Should().BeFalse();
        }

        [Fact]
        public void Authenticate_Should_ReturnTrue_When_PasswordCorrect()
        {
            //given
            CreateDefaultUser();

            //When
            var result = _userService.Authenticate("Jamie", "password");

            //Then
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetAll_Should_ReturnAllUsers_When_ThereAre10UsersInDatabase()
        {
            //given
            var random = new Random();
            const int userCount = 10;
            for (var i = 0; i < userCount; i++)
                await _db.Db.Users.AddAsync(new User
                {
                    Username = RandomGenerator.RandomString(random.Next(5, 7)),
                    Email = RandomGenerator.RandomString(random.Next(5, 7)) + "@gmail.com",
                    PasswordHash = new byte[32],
                    PasswordSalt = new byte[16]
                });

            await _db.Db.SaveChangesAsync();

            //When
            var users = _userService.GetAll();

            //Then
            users.Select(user => user.Username).ToHashSet().Should().HaveCount(userCount);
        }

        [Fact]
        public void Create_Should_InputContext_When_InputUser()
        {
            //given
            var a = new User {Username = "MyNameA", Email = "eamil@gmail.com"};
            var b = new User {Username = "MyNameB", Email = "email@naver.com"};

            //When
            _userService.Create(a, "password");
            try
            {
                _userService.Create(b, "password");
            }
            catch (Exception e)
            {
                //Then
                e.Should().BeNull();
            }
        }

        [Fact]
        public void Create_Should_ThrowException_When_InputUsernameThatAlreadyTaken()
        {
            //Given
            var a = new User {Username = "MyNameA", Email = "eamil@gmail.com"};
            var b = new User {Username = "MyNameA", Email = "email@naver.com"};

            //When
            _userService.Create(a, "password");
            try
            {
                _userService.Create(b, "password");
            }
            catch (Exception e)
            {
                //Then
                e.Should().NotBeNull();
            }
        }

        [Fact]
        public void Update_Should_UpdateUserData_When_Input()
        {
            //given
            var user = CreateDefaultUser();
            var beforeName = user.Username;
            var beforeEmail = user.Email;

            const string afterName = "MyNameB";
            const string afterEmail = "email@naver.com";

            //When
            user.Username = afterName;
            user.Email = afterEmail;
            _userService.Update(user);

            //Then
            _userService.GetById(user.Id).Username.Should().NotBe(beforeName);
            _userService.GetById(user.Id).Username.Should().Be(afterName);

            _userService.GetById(user.Id).Email.Should().NotBe(beforeEmail);
            _userService.GetById(user.Id).Email.Should().Be(afterEmail);
        }

        [Fact]
        public void VerifyUserName_Should_ReturnBool_When_VerifyUserName()
        {
            //given
            const string wrongUsername = "hhhh";
            var user = CreateDefaultUser();
            var username = user.Username;


            _userService.Create(user, "password");

            //when
            var checkFindSuccess = _userService.VerifyUserName(username);
            var checkFindFail = _userService.VerifyUserName(wrongUsername);

            //then
            checkFindSuccess.Should().BeTrue();
            checkFindFail.Should().BeFalse();
        }

        [Fact]
        public void Delete_Should_NotRealDelete_When_CallDelete()
        {
            //given
            var user = CreateDefaultUser();

            //when
            _userService.Delete(user.Id);

            //then
            user.Should().NotBeNull();
            user.IsDeleted.Should().BeTrue();
            _userService.GetAll().ToHashSet().Count.Should().Be(0);
        }

        [Fact]
        public void UserEntity_Should_UpdateUpdatedAt_When_UserUpdated()
        {
            //given
            var defaultUser = CreateDefaultUser();
            var curUpdateTime = defaultUser.UpdatedAt;
            const string changeName = "I'm not Jamie!";

            //when
            defaultUser.Username = changeName;
            _userService.Update(defaultUser, "password");

            //then
            curUpdateTime.Should().NotBe(defaultUser.UpdatedAt);
        }

        private User CreateDefaultUser()
        {
            var user = new User {Username = "Jamie", Email = "Jamie@gmail.com"};
            _userService.Create(user, "password");

            return user;
        }
    }
}