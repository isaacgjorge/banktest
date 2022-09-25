using Domain.Data;
using Domain.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repository;

public class UserRepository: IUserRepository
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public Task<User> Get(int userId) => _context.User.FirstOrDefaultAsync(_ => _.UserId == userId);

    public Task<User> GetByEmail(string email) => 
        _context.User.FirstOrDefaultAsync(_ => _.Email == email);
    

    public async Task<bool> Add(User user)
    {
        try
        {
            /*
             * !Warning!
             * This could be solved using index isUnique constraint, as tried (check ApplicationContext).
             * As this is for test purposes, I'm using inMemoryDatabase, which ignores those types of constraints.
             *
             * Reference:
             *  https://learn.microsoft.com/en-us/ef/core/testing/
             *  https://stackoverflow.com/questions/52259580/adding-a-unique-index-on-ef-core-mapping-does-not-seem-to-work
             *  https://learn.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=dotnet-core-cli
             *
             */
            if (await _context.User.AnyAsync(_ => _.Email == user.Email))
            {
                return await Task.FromResult(false);
            }

            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

        }
        catch
        {
            await _context.DisposeAsync();
            return await Task.FromResult(false);
        }

        return await Task.FromResult(true);
    }
}

