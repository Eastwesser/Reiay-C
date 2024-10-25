using Relay.Models;

public interface IUserService
{
    User Register(UserCreateDto userDto);
}

public class UserService : IUserService
{
    public User Register(UserCreateDto userDto)
    {
        // Логика регистрации пользователя
        return new User { Username = userDto.Username, Password = userDto.Password };
    }
}
