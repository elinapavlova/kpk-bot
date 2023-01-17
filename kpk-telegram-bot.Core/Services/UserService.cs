using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Mappers;
using kpk_telegram_bot.Common.Responses;

namespace kpk_telegram_bot.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse?> Create(UserEntity user)
    {
        var result = await _userRepository.Create(user);
        return result is null
            ? null
            : UserMapper.Map(result);
    }

    public async Task<bool> CheckExist(long userId)
    {
        return await _userRepository.IsExist(userId);
    }

    public async Task<UserResponse?> GetById(long userId)
    {
        var user = await _userRepository.GetById(userId);
        return user is null
            ? null
            : UserMapper.Map(user);
    }
}