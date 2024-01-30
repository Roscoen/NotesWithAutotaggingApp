using NWA.Application.DTOs;
using NWA.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWA.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterAsync(string username, string password, string email);
        Task<bool> CheckPasswordAsync(string username, string password);
        Task<UserDto> GetUserByUsernameAsync(string username);
    }
}
