﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;
using TripPlannerAPI.DTOs;
using TripPlannerAPI.Models;
using TripPlannerAPI.Services;

namespace TripPlannerAPI.Controllers
{
    public class LoginResponse { public string Token { get; set; } }
    public class GetUserResponse { public string username { get; set; } }

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<User> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(typeof(string), 401)]
        public async Task<ActionResult<LoginResponse>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Unauthorized("The provided username-password pair does not match any accounts.");

            return new LoginResponse { Token = await _tokenService.GenerateToken(user) };
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(LoginResponse),200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult<LoginResponse>> Register(RegisterDto registerDto)
        {
            if (null != await _userManager.FindByNameAsync(registerDto.UserName))
                return StatusCode((int)HttpStatusCode.Conflict, "The requested username is already in use.");

            var user = new User { UserName = registerDto.UserName, Email = registerDto.Email };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }

                return ValidationProblem("Validation failed.");
            }

            await _userManager.AddToRoleAsync(user, "User");

            return new LoginResponse { Token = await _tokenService.GenerateToken(user) };
        }

        [Authorize]
        [HttpGet("current_user")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(typeof(UnauthorizedResult), 401)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return NotFound();
            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user)
            };
        }

        [Authorize]
        [HttpGet("user/{username}")]
        [ProducesResponseType(typeof(GetUserResponse), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<ActionResult<GetUserResponse>> GetUser(string username)
        {
            var callingUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (callingUser == null)
                return Unauthorized("Unauthorized.");
            var user = await _userManager.FindByNameAsync(username);
            if(user==null)
                return NotFound("User not found.");
            return new GetUserResponse { username = username };
        }

        [Authorize]
        [HttpDelete("user/{username}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<ActionResult<GetUserResponse>> DeleteUser(string username)
        {
            if (User.Identity == null)
                return Unauthorized("Unauthorized.");
            var roles = await _userManager.GetRolesAsync(await _userManager.FindByNameAsync(User.Identity.Name));
            bool isAdmin = false;
            for (int i = 0; i < roles.Count(); i++)
            {
                if (roles[i] == "Admin")
                    isAdmin = true;
            }
            if (isAdmin == false)
                return Unauthorized("You are not an Admin.");

            var user = await _userManager.FindByNameAsync(username);
            if(user==null) return NotFound("The user you tried to delete doesn't exist.");

            _ = await _userManager.DeleteAsync(user);
            return Ok("User "+ username +" succesfully deleted.");
        }
    }
}
