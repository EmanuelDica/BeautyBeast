using BCrypt.Net;

public static class PasswordHelper
{
    // Hash the password before storing in the database
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Verify the entered password against the stored hash
    public static bool VerifyPassword(string enteredPassword, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
    }
}