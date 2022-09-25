using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    public readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Post(string email, string name )
    {
        var user = new User
        {
            Email = email,
            Name = name,
        };

        bool result;

        try
        {
            result = await _userService.Add(user);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }

        if (result)
        {
            return Ok();
        }

        return BadRequest("Unexpected Error");
    }

    [HttpGet]
    [Route("GetByID")]
    public async Task<IActionResult> Get(int id)
    {
        User result = await _userService.GetById(id);

        if (result != null)
        {
            return Ok(result);
        }

        return BadRequest();
    }

    [HttpGet]
    [Route("GetByEmail")]
    public async Task<IActionResult> Get(string email)
    {
        User result;

        try
        {
            result = await _userService.GetByEmail(email);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }

        if (result != null)
        {
            return Ok(result);
        }       

        return BadRequest();
    }

}
