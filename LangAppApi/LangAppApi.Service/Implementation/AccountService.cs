using LangAppApi.Domain.Auth;
using LangAppApi.Domain.Common;
using LangAppApi.Domain.Enum;
using LangAppApi.Domain.Exceptions;
using LangAppApi.Domain.Settings;
using LangAppApi.Service.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LangAppApi.Service.Implementation
{
    /// <summary>
    ///
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IStringLocalizer<AccountService> _localizer;

        /// <summary>
        ///
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="jwtSettings"></param>
        /// <param name="signInManager"></param>
        /// <param name="localizer"></param>
        public AccountService(UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings,
            SignInManager<ApplicationUser> signInManager, IStringLocalizer<AccountService> localizer)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _localizer = localizer;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException($"{_localizer["No Accounts Registered with"]} {request.Email}");
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);
            if (!result.Succeeded)
            {
                throw new BadRequestException($"{_localizer["Invalid Credentials for"]} '{request.Email}'.");
            }

            var jwtSecurityToken = await GenerateJwToken(user);
            var response = new AuthenticationResponse
            {
                Id = user.Id,
                JwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName
            };

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;

            var refreshToken = GenerateRefreshToken();
            response.RefreshToken = refreshToken.Token;

            return new Response<AuthenticationResponse>(response, $"{_localizer["Authenticated"]} {user.UserName}");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<string>> RegisterAsync(RegisterRequest request)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new ConflictException($"{_localizer["Username"]} '{request.UserName}' {_localizer["is already taken"]}.");
            }
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null) throw new ConflictException($"{_localizer["Email"]} {request.Email} {_localizer["is already registered"]}.");

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) throw new BadRequestException($"{result.Errors.ToList()[0].Description}");

            await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());

            return new Response<string>(user.Id, _localizer["User {0} Registered !. You can login on the app", request.Email]);
        }

        #region Private Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<JwtSecurityToken> GenerateJwToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(role => new Claim("roles", role)).ToList();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private static string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private static RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
        }

        #endregion Private Methods
    }
}