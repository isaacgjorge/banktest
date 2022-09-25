using Domain;

namespace Services.Interfaces;

public interface IAccountService
{
    Task<Account> GetByAccountId(int accountId);
    Task<List<Account>> GetAllByUserEmail(string userEmail);
    Task<bool> CreateAccount(Account account);
    Task<bool> DeleteAccountByAccountId(int accountId);
    Task<bool> Withdraw(int accountId, decimal amount);
    Task<bool> Deposit(int accountId, decimal amount);

}

