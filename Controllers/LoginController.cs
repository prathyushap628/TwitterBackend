using Twitter.Models;
using Twitter.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Twitter.DTOs;
using Twitter.Utilities;

namespace Twitter.Controllers;

[ApiController]
[Route("api/users")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly IUserRepository _user;
    private readonly IConfiguration _config;

    public LoginController(ILogger<LoginController> logger,
    IUserRepository user, IConfiguration config)
    {
        _logger = logger;
        _user = user;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResDTO>> Login(
        [FromBody] UserLoginDTO Data
    )
    {
        var existingUser = await _user.GetByEmail(Data.Email);

        if (existingUser is null)
            return NotFound("User with given email id not found");
        bool verified = BCrypt.Net.BCrypt.Verify(Data.Password, existingUser.Password);

        if (!verified)
            return BadRequest("Incorrect password");

        var token = Generate(existingUser);

        var res = new UserLoginResDTO
        {
            UserId = existingUser.UserId,
            Email = existingUser.Email,
            Token = token,
            UserName = existingUser.UserName,
        };

        return Ok(res);
    }

    private string Generate(Users users)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(TwitterConstants.UserId, users.UserId.ToString()),
            new Claim(TwitterConstants.Email, users.Email.ToString()),
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}