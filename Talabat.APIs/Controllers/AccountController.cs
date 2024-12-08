using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.ServiceInterface;

namespace Talabat.APIs.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokennService _tokennService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokennService tokennService) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokennService = tokennService;
        }
        //Login

        //Login
        [HttpPost(template: "Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

            if (!result.Succeeded) return Unauthorized(value: new ApiResponse(statusCode: 401));
            return Ok(new UserDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token =await _tokennService.CreateTokenAsync(user, _userManager)
            }
            );

        }



        //Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
        {
            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email.Split(separator: "@")[0]

            };
            var result = await _userManager.CreateAsync(user, password: model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(error: new ApiResponse(statusCode:400));
               
            }

            var returnedUser = new UserDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokennService.CreateTokenAsync(user, _userManager)
            };
            return Ok(returnedUser);
        }


       // [Authorize]
        //[HttpGet("GetCurrentUser")]
        //public async Task<ActionResult<UserDTO>> GetCurrentUser()
        //{
           
        //    var userEmail = User.FindFirstValue(ClaimTypes.Email);
        //    var user = await _userManager.FindByEmailAsync(userEmail);
        //    return Ok(new UserDTO()
        //    {
        //        DisplayName = user.DisplayName,
        //        Email = user.Email,
        //        Token = await _tokennService.CreateTokenAsync(user, _userManager)
        //    }
        //    );
        //}

        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var claims = User.Claims.ToList();
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            // Extract the email claim
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest(new ApiResponse(400, "Email claim is missing or invalid in the token."));
            }

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "User not found."));
            }

            return Ok(new UserDTO
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokennService.CreateTokenAsync(user, _userManager)
            });
        }


        [HttpGet("GetCurrentUserAddress")]
        public async Task<ActionResult<Address>> GetCurrentUserAddress()
        {

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
            return Ok(new UserDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokennService.CreateTokenAsync(user, _userManager)
            }
            );
        }


    }
}
