using System.Security.Cryptography;
using System.Text;
namespace BeautyBeastServer.Helpers;

// Static helper class for generating password salts and hashing passwords.
public static class PasswordHelper
{
    public static string GenerateSalt()
    {
        // Create a byte array to hold the random salt (16 bytes = 128 bits).
        byte[] saltBytes = new byte[16];

        // Use a cryptographic random number generator to fill the byte array with random bytes.
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes); // Fill the saltBytes array with cryptographically secure random bytes.
        }

        // Convert the random bytes to a Base64 string so that it can be easily stored as text.
        return Convert.ToBase64String(saltBytes);
    }

    /// <summary>
    /// Hashes a password using the provided salt.
    /// The password and salt are combined and passed through a hashing algorithm (HMAC-SHA256).
    /// HMAC-SHA256 produces a secure 256-bit hash value.
    /// </summary>
    /// <param name="password">The plain-text password entered by the user.</param>
    /// <param name="salt">The random salt generated for this password.</param>
    /// <returns>A Base64-encoded string representation of the hashed password.</returns>
    public static string HashPassword(string password, string salt)
    {
        // Convert the salt (which is a string) to a byte array.
        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

        // Create a new instance of HMACSHA256 with the salt bytes as the key.
        // HMACSHA256 is a hashing algorithm that uses the provided key (salt) during hashing.
        using (var hmac = new HMACSHA256(saltBytes))
        {
            // Convert the plain-text password to a byte array.
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Compute the hash of the password combined with the salt.
            byte[] hashBytes = hmac.ComputeHash(passwordBytes);

            // Convert the hashed bytes to a Base64 string so it can be stored as text.
            return Convert.ToBase64String(hashBytes);
        }
    }
}