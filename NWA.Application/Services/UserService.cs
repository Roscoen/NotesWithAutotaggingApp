using Microsoft.EntityFrameworkCore;
using NWA.Application.DTOs;
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
        private readonly IAppDbContext _context;
        public UserService(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(string username, string password, string email)
        {
            var user = new User
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Email = email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> CheckPasswordAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return false;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
                
            };
        }
    }
}
