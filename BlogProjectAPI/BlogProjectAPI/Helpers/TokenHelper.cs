using BlogProjectAPI.Data.Models;
using BlogProjectAPI.Data.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogProjectAPI.Helpers
{
    public static class TokenHelper
    {
        public static string GenerateToken(IConfiguration configuration,TokenUser user)
        {
            var claims = new[]
                    {
                         new Claim(JwtRegisteredClaimNames.Sub,user.Username),
                         new Claim(JwtRegisteredClaimNames.Jti,user.Password),
                         new Claim("role",user.Roles.RoleName) //:TODO
                     };
            var token = new JwtSecurityToken
                (
                    issuer: configuration["Issuer"], //appsettings.json içerisinde bulunan issuer değeri
                    audience: configuration["Audience"],//appsettings.json içerisinde bulunan audince değeri
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SigningKey"])), SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static void LogThisAccess(TokenLoginModel tokenLoginModel ,bool isApproved, string access)
        {
            //TokenAccessLogs tokenAccessLogs = new TokenAccessLogs
            //{
            //    Access = access,
            //    AccessTrueFalse = isApproved,
            //    AccessRequest = tokenLoginModel.ToString()
            //};
            //TODO: Db ye logla

        }
    }
}
