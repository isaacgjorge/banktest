using Domain;

namespace Services.Interfaces;

public interface IUserService
{
    public Task<bool> Add(User user);

    public Task<User> GetById(int id);

    public Task<User> GetByEmail(string email);
}

