namespace Domain.Repository.Interfaces;

public interface IUserRepository
{
    Task<Domain.User> Get(int id);
    Task<bool> Add(Domain.User user);
    Task<Domain.User> GetByEmail(string email);
}

