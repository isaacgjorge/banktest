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
public class AccountController : ControllerBase
{
    public readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }


    [HttpGet]
    [Route("GetByAccountId")]
    public async Task<IActionResult> Get(int accountId)
    {
        Account result = await _accountService.GetByAccountId(accountId);

        if (result != null)
        {
            return Ok(result);
        }

        return BadRequest();
    }

    [HttpGet]
    [Route("GetAllByUserEmail")]
    public async Task<IActionResult> Get(string userEmail)
    {
        List<Account> result;

        try
        {
            result = await _accountService.GetAllByUserEmail(userEmail);
        }
        catch (ArgumentNullException e)
        {
            return BadRequest(e.Message);
        }

        if (result != null)
        {
            return Ok(result);
        }

        return BadRequest();
    }


    [HttpPost]
    public async Task<IActionResult> Post(int userId, decimal initialBalance)
    {
        Account account = new Account()
        {
            UserId = userId,
            Balance = initialBalance
        };

        bool result;

        try
        {
            result = await _accountService.CreateAccount(account);
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


    [HttpDelete]
    public async Task<IActionResult> Delete(int accountId)
    {
        bool result  = await _accountService.DeleteAccountByAccountId(accountId);
       
        if (result != null)
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpPatch]
    [Route("withdraw")]
    public async Task<IActionResult> Withdraw(int accountId, decimal amount)
    {
        bool result;

        try
        {
            result = await _accountService.Withdraw(accountId, amount);
        }
        catch (ArgumentNullException e)
        {
            return BadRequest(e.Message);
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

    [HttpPatch]
    [Route("deposit")]
    public async Task<IActionResult> Deposit(int accountId, decimal amount)
    {
        bool result;

        try
        {
            result = await _accountService.Deposit(accountId, amount);
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
