namespace TaskManagerAPI.Services;

using static BCrypt.Net.BCrypt;

public class PasswordService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return Verify(password, hashedPassword);
    }
}

