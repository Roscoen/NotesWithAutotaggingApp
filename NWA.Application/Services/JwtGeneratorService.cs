using Microsoft.Extensions.Options;
using NWA.Application.DTOs;
using NWA.Application.Helpers;
using NWA.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWA.Application.Services
{
    public class JwtGeneratorService : IJwtGeneratorService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtGeneratorService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(UserDto user)
        {
            return JwtHelper.GenerateToken(user, _jwtSettings);
        }
    }
}
