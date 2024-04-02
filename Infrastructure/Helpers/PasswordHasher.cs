

using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Helpers;

public class PasswordHasher
{
    public static (string, string) GenerateSecurePassword(string password)
    {
        using var hmac = new HMACSHA512();
        var securityKey = hmac.Key;
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return (Convert.ToBase64String(hash), Convert.ToBase64String(securityKey));
    }

    public static bool ValidateSecurePassword(string password, string hash, string securityKey)
    {
        using var hmac = new HMACSHA512(Convert.FromBase64String(securityKey));
        var hashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        var userPassword = Convert.FromBase64String(hash);

        for (var i = 0; i < hashedPassword.Length; i++)
        {
            if (hashedPassword[i] != userPassword[i])
                return false;
        }
        return true;
    }





    /******************************************** variant with Base64 API KEY*******************************/

    public static string GenerateSecurePassword2(string password)
    {
        try
        {
            byte[] salt = new byte[16];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);



            return  Convert.ToBase64String(hashBytes);

            

        }
        catch { }
        return null!;
    }


    public static bool ValidateSecurePassword2(string password, string storedPassword)
    {
        byte[] hashBytes = Convert.FromBase64String(storedPassword);
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0 , salt, 0, 16);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(20);

        for(int i = 0; i < hashBytes.Length; i++)
        {
            if (hashBytes[i +16] != hash[i])
                return false;
        }
        return true;
    }

}
