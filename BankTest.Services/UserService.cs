
using Domain;
using Domain.Repository.Interfaces;
using Services.Interfaces;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<bool> Add(User user)
    {
        if (user == null)
            throw new InvalidOperationException("User cannot be null");

        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Name))
            throw new InvalidOperationException("Email and Name are required");


        return await _userRepository.Add(user);
    }

    public async Task<User> GetById(int id)
    {
        return await _userRepository.Get(id);
    }

    public async Task<User> GetByEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            throw new InvalidOperationException("Email required");
        
        return await _userRepository.GetByEmail(email);
    }
}

