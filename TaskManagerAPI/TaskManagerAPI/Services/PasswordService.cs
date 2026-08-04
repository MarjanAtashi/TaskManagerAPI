namespace TaskManagerAPI.Services;

using static BCrypt.Net.BCrypt;

public class PasswordService
{
    public string HashingPassword(string password)
    {
        return HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return Verify(password, hashedPassword);
    }
}

