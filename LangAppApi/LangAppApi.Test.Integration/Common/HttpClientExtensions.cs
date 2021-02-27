using LangAppApi.Domain.Auth;
using LangAppApi.Domain.Settings;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace LangAppApi.Test.Integration.Common
{
    public static class HttpClientExtensions
    {
        public static void SetClaimsViaHeader(this HttpClient client, ApplicationUser user, JwtSettings jwtSettings)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetJwtToken(user, jwtSettings));
        }

        private static string GetBasicToken(string username, string password)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
        }

        private static string GetJwtToken(ApplicationUser user, JwtSettings jwtSettings)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return tokenHandler.WriteToken(jwtSecurityToken);
        }
    }
}