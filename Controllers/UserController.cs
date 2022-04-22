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
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _user;
    private readonly IConfiguration _config;

    public UserController(ILogger<UserController> logger,
    IUserRepository user, IConfiguration config)
    {
        _logger = logger;
        _user = user;
        _config = config;
    }
    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == TwitterConstants.UserId).First().Value);
    }

    [HttpPost("registration")]
    public async Task<ActionResult<UserCreateResDTO>> Registration(
        [FromBody] UserCreateDTO Data
    )
    {
        var existingUser = await _user.GetByEmail(Data.Email);

        if (existingUser != null)
            return BadRequest("User with given email already exist");

        Users newUser = new Users
        {
            UserName = Data.UserName,
            Email = Data.Email?.Trim(),
            Password = BCrypt.Net.BCrypt.HashPassword(Data.Password),
        };

        var createdUser = await _user.CreateNewUser(newUser);



        var res = new UserCreateResDTO
        {
            UserId = createdUser.UserId.ToString(),
            Email = createdUser.Email,
            UserName = createdUser.UserName,


        };

        return Ok(res);
    }

    [HttpPut("update")]
    public async Task<ActionResult> UpdateUserName(
[FromBody] UserLoginUpdateDTO Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _user.GetById(userId);

        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != userId)
            return StatusCode(403, "You cannot update other's username");

        var toUpdateItem = existingItem with
        {
            UserName = Data.UserName is null ? existingItem.UserName : Data.UserName.Trim(),
            // UpdatedAt = !Data.Updated_at.HasValue ? existingItem.UpdatedAt : Data.Updated_at.Value,
        };

        await _user.Update(toUpdateItem);

        return NoContent();
    }


}