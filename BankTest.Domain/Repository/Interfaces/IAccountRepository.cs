namespace Domain.Repository.Interfaces;
public interface IAccountRepository
{
    Task<bool> Add(Domain.Account account);
    Task<bool> Delete(Domain.Account account);
    Task<bool> DeleteById(int accountId);
    Task<bool> Update(Domain.Account account);
    Task<Domain.Account> Get(int accountId);
    Task<List<Domain.Account>> GetAllByUserEmail(string userEmail);
}

