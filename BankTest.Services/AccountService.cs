using Domain;
using Domain.Repository.Interfaces;
using Services.Interfaces;

namespace Services;

public class AccountService : IAccountService
{
    private const decimal MinimumAmountPossible = 100;
    private const decimal HighestDepositAmountPossible = 10000;

    private readonly IAccountRepository _accountRepository;
    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Account> GetByAccountId(int accountId)
    {
        return await _accountRepository.Get(accountId);
    }

    public async Task<List<Account>> GetAllByUserEmail(string userEmail)
    {
        if (string.IsNullOrWhiteSpace(userEmail))
            throw new ArgumentNullException(null, "User email is required");


        return await _accountRepository.GetAllByUserEmail(userEmail);
    }

    public async Task<bool> CreateAccount(Account account)
    {
        if (!BalanceIsGreaterOrEqualTheMinimumPossibleAmount(account.Balance))
            throw new InvalidOperationException("Initial deposit must be greater than $100");

        return await _accountRepository.Add(account);
    }
    
    public async Task<bool> DeleteAccountByAccountId(int accountId)
    {
        return await _accountRepository.DeleteById(accountId);
    }
    
    public async Task<bool> Withdraw(int accountId, decimal amount)
    {
        Account account = await GetByAccountId(accountId);
        if (account == null)
            throw new ArgumentNullException("Account doesn't exists");
        
        if (!WithdrawAmountLessThan90PercentTotalAmount(account.Balance, amount))
            throw new InvalidOperationException("Withdraw amount cannot be more than 90% of total balance");

        account.Balance -= amount;

        if (!BalanceIsGreaterOrEqualTheMinimumPossibleAmount(account.Balance))
            throw new InvalidOperationException("Account cannot have less than $100 at any time");
        
        return await _accountRepository.Update(account);

        bool WithdrawAmountLessThan90PercentTotalAmount(decimal balance, decimal withdrawAmount)
        {
            return (100 - ((balance - withdrawAmount) / balance) * 100) < 90;

        }
    }

    public async Task<bool> Deposit(int accountId, decimal amount)
    {
        if (DepositAmountCannotBeGreaterThan10000(amount))
            throw new InvalidOperationException("Cannot deposit more than $10,000 in a single transaction");

        Account account = await GetByAccountId(accountId);
        account.Balance += amount;

        return await _accountRepository.Update(account);

        bool DepositAmountCannotBeGreaterThan10000(decimal depositAmount)
        {
            return depositAmount > HighestDepositAmountPossible;
;
        }
    }

    private bool BalanceIsGreaterOrEqualTheMinimumPossibleAmount(decimal balance)
    {
        return balance >= MinimumAmountPossible;
    }
}

