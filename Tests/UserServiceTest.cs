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
        private readonly UserService _userService;
        private readonly TestUserContext _db;

        public UserServiceTest()
        {
            _db = new TestUserContext();
            _userService = new UserService(_db.Db);
        }

        [Fact]
        public void Authenticate_Should_ReturnFalse_When_PasswordIncorrect()
        {
            //given
            var a = new User {Username = "Jamie", Email = "Jamie@gmail.com"};
            _userService.Create(a, "password");

            //When
            var result = _userService.Authenticate("Jamie", "PASSWORD");

            //Then
            result.Should().BeFalse();
        }
        
        [Fact]
        public void Authenticate_Should_ReturnTrue_When_PasswordCorrect()
        {
            //given
            var a = new User {Username = "Jamie", Email = "Jamie@gmail.com"};
            _userService.Create(a, "password");

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
            var userCount = 10;
            for (var i = 0; i < userCount; i++)
            {
                await _db.Db.Users.AddAsync(new User
                {
                    Username = RandomGenerator.RandomString(random.Next(5, 7)),
                    Email = RandomGenerator.RandomString(random.Next(5, 7)) + "@gmail.com",
                    PasswordHash = new byte[32],
                    PasswordSalt = new byte[16],
                });
            }
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
            const string beforeName = "MyNameA";
            const string beforeEmail = "email.gmail.com";
            const string afterName = "MyNameB";
            const string afterEmail = "email@naver.com";

            var a = new User {Username = beforeName, Email = beforeEmail};
            _userService.Create(a, "password");

            //When
            a.Username = afterName;
            a.Email = afterEmail;
            _userService.Update(a);

            //Then
            _userService.GetById(a.Id).Username.Should().NotBe(beforeName);
            _userService.GetById(a.Id).Username.Should().Be(afterName);
            
            _userService.GetById(a.Id).Email.Should().NotBe(beforeEmail);
            _userService.GetById(a.Id).Email.Should().Be(afterEmail);

        }

        [Fact]
        public void VerifyUserName_Should_ReturnBool_When_VerifyUserName()
        {
            //given
            const string username = "Dabby";
            const string wrongUsername = "hhhh";

            var user = new User {Username = username, Email = "Dabby@gmail.com"};
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
            var user = new User {Username = "Jamie", Email = "Jamie@gmail.com"};
            _userService.Create(user, "password");

            //when
            _userService.Delete(user.Id);

            //then
            user.Should().NotBeNull();
            user.IsDeleted.Should().BeTrue();
        }
    }
}