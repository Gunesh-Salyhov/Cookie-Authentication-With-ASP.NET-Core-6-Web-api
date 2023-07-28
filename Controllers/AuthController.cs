
using System.Security.Claims;
using Data;
using Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers;
[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase

{
    public readonly DataContext _context;

    public AuthController(DataContext context)
    {
        _context = context;
    }
    [HttpPost("signin")]
    public async Task<IActionResult> SignInAsync([FromBody] LoginDto signInRequest)
    {
        var user = _context.Users.FirstOrDefault(x => x.Email == signInRequest.Email &&
                                            x.Password == signInRequest.Password);
        if (user is null)
        {
            return BadRequest("Invalid credentials.");
        }

        var claims = new List<Claim>
    {
        new Claim(type: ClaimTypes.Email, value: signInRequest.Email),
        new Claim(type: ClaimTypes.Name, value: user.Username)
    };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity)
            );

        return Ok("Success");
    }

    [HttpGet("signout")]
    public async Task SignOutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

}