using System.ComponentModel.DataAnnotations;
using ImageProcessingService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ImageProcessingService.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtService _jwtService;

    public AuthController(UserManager<AppUser> userManager, JwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterRequest request)
    {
        var email = request.Email.Trim();
        var userName = request.UserName.Trim();

        var emailExists = await _userManager.FindByEmailAsync(email);

        if (emailExists != null)
        {
            return Conflict(new { Message = "Email already exists" });
        }

        var userNameExist = await _userManager.FindByNameAsync(userName);

        if (userNameExist != null)
        {
            return Conflict(new { Message = "UserName already exists" });
        }

        var user = new AppUser
        {
            Email = email,
            UserName = userName
        };

        await _userManager.CreateAsync(user, request.Password);

        return Ok("User is Created Successfully");
    }


    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user is null)
        {
            return BadRequest("Email Or Password is incorrect");
        }
        
        var checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!checkPassword)
        {
            return BadRequest("Email Or Password is incorrect");
        }

        var token = _jwtService.Create(user);

        return Ok(token);
    }

    [HttpGet("test")]
    [Authorize]
    public ActionResult TestEndpoint()
    {
        return Ok("You are authorized");
    }
    
    
    
}

public class RegisterRequest()
{
    [Required]
    [StringLength(32), MinLength(3)]
    public string UserName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

}