using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.ServiceInterface;

namespace Talabat.Services
{
    public class TokenService : ITokennService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> _userManager)
        {
            if (string.IsNullOrEmpty(user.Email))
                throw new ArgumentNullException(nameof(user.Email), "User email cannot be null.");

              var authClaims = new List<Claim>
              {
               new Claim(ClaimTypes.Email, user.Email)  // Make sure email is added here
              };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var signingCredentials = new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> _userManager)
        //{
        //    // Validate input
        //    if (user == null)
        //        throw new ArgumentNullException(nameof(user), "User cannot be null.");
        //    if (string.IsNullOrEmpty(user.Email))
        //        throw new ArgumentNullException(nameof(user.Email), "User email cannot be null.");
        //    if (string.IsNullOrEmpty(user.UserName))
        //        throw new ArgumentNullException(nameof(user.UserName), "User name cannot be null.");

        //    // Create claims
        //    var authClaims = new List<Claim>
        //{
        //    new Claim(ClaimTypes.GivenName, user.UserName),
        //    new Claim(ClaimTypes.Email, user.Email)
        //};

        //    var userRoles = await _userManager.GetRolesAsync(user);
        //    foreach (var role in userRoles)
        //    {
        //        authClaims.Add(new Claim(ClaimTypes.Role, role));
        //    }

        //    // Validate configuration
        //    var jwtKey = _configuration["JWT:Key"];
        //    var jwtIssuer = _configuration["JWT:ValidIssuer"];
        //    var jwtAudience = _configuration["JWT:Audience"];
        //    var jwtDuration = _configuration["JWT:DurationInDays"];

        //    if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience) || string.IsNullOrEmpty(jwtDuration))
        //    {
        //        throw new InvalidOperationException("JWT configuration is missing or invalid.");
        //    }

        //    // Generate token
        //    var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        //    var signingCredentials = new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature);
        //    var token = new JwtSecurityToken(
        //        issuer: jwtIssuer,
        //        audience: jwtAudience,
        //        expires: DateTime.Now.AddDays(double.TryParse(jwtDuration, out var days) ? days : 1), // Fallback to 1 day
        //        claims: authClaims,
        //        signingCredentials: signingCredentials
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }

}
