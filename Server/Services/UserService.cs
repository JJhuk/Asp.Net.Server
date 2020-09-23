using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Server.Helpers;

namespace Server.Services
{
    public interface IUserService
    {
        bool Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        Task<User> Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
        User GetByUsername(string username);
        Task<bool> UserNameExists(string userName);
    }

    public class UserService : IUserService
    {
        private readonly UserContext _context;

        public UserService(UserContext context)
        {
            _context = context;
        }

        public bool Authenticate(string username, string password)
        {
            var user = _context.Users.Single(x => x.Username == username);
            
            return VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public async Task<User> Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            
            if (await _context.Users.AnyAsync(x => x.Username == user.Username))
                throw new AppException("Username " + user.Username + " is already taken");
            
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.CreatedDateTime = DateTime.Now;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public void Update(User userParam, string password = null)
        {
            bool IsNameAlreadyTaken(int userId)
            {
                return _context.Users.Any(x => x.Id != userId && x.Username == userParam.Username);
            }

            var user = _context.Users.Single(u => u.Id == userParam.Id);
            if (IsNameAlreadyTaken(user.Id))
                throw new AppException("Username " + userParam.Username + " is already taken");

            user.Email = userParam.Email;
            user.Username = userParam.Username;

            if (!string.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return;
            user.IsDeleted = true;
        }

        public User GetByUsername(string username)
        {
            return _context.Users.Single(x => x.Username == username);
        }

        public bool VerifyUserName(string userName)
        {
            return _context.Users.Any(x => x.Username == userName);
        }

        public Task<bool> UserNameExists(string userName)
        {
            return _context.Users.AnyAsync(user => user.Username == userName);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            const string passwordHash = "passwordHash";

            if (password == null)
                throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", passwordHash);
            if (storedSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", passwordHash);

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                if (computedHash.Where((t, i) => t != storedHash[i]).Any())
                {
                    return false;
                }
            }

            return true;
        }
    }
}