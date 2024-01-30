using NWA.Application.Interfaces;
using NWA.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWA.Application.Services
{
    public class UserService : IUserService
    {
        public UserService()
        {
                
        }

        public User CreateUser(string username, string password, string email)
        {
            var user = new User
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Email = email
            };

            // Dodaj użytkownika do bazy danych

            return user;
        }
        public bool VerifyPassword(string password, User user)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
    }
}
