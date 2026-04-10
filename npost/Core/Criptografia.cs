using System.Security.Cryptography;
using System.Text;

namespace npost.Core;

public class Criptografia
{
    public static string sha256(string randomString)
    {
        var sHA256 = SHA256.Create();
        byte[] hashByte = sHA256.ComputeHash(Encoding.UTF8.GetBytes(randomString));
        var hashString = new StringBuilder();

        foreach (byte theByte in hashByte)
        {
            hashString.Append(theByte.ToString("x2"));
        }
        return hashString.ToString();
    }
}