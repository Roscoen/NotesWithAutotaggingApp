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
        User CreateUser(string username, string password, string email);
        bool VerifyPassword(string password, User user);
    }
}
