using NWA.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWA.Application.Interfaces
{
    public interface IJwtGeneratorService
    {
        string GenerateToken(UserDto user);
    }
}
