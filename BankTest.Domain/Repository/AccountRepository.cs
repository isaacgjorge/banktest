using Domain.Data;
using Domain.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repository;

public class AccountRepository: IAccountRepository
{
    private readonly ApplicationContext _context;
    public AccountRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<bool> Add(Domain.Account account)
    {
        try
        {
            await _context.Account.AddAsync(account);
            await _context.SaveChangesAsync();
        }
        catch
        {
            return await Task.FromResult(false);
        }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(Domain.Account account)
    {
        try
        {
            _context.Account.Remove(account);
            await _context.SaveChangesAsync();
        }
        catch
        {
            return await Task.FromResult(false);
        }

        return await Task.FromResult(true);

       
    }
    public async Task<bool> DeleteById(int accountId)
    {
        try
        {
            var account = await Get(accountId);
            _context.Account.Remove(account);
            await _context.SaveChangesAsync();
        }
        catch
        {
            return await Task.FromResult(false);
        }

        return await Task.FromResult(true);


    }
    public async Task<bool> Update(Domain.Account account)
    {
        try
        {
           _context.Account.Update(account);
           await _context.SaveChangesAsync();
        }
        catch
        {
            return await Task.FromResult(false);
        }

        return await Task.FromResult(true);
    }

    public async Task<Domain.Account> Get(int accountId)
    {
        return await _context.Account.FirstOrDefaultAsync(x => x.AccountId == accountId);
    }

    public async Task<List<Domain.Account>> GetAllByUserEmail(string userEmail)
    {
        return await _context.Account.Where(x => x.User.Email == userEmail).ToListAsync();
    }
}

