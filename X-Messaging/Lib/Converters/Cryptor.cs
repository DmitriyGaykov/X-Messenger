using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Converters;

public static class Cryptor
{
    private static readonly Aes aes;

    static Cryptor()
    {
        aes = Aes.Create();
        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Padding = PaddingMode.PKCS7;
        aes.GenerateKey();
        aes.GenerateIV();
    }

    public static string Encrypte(string text)
    {
        ICryptoTransform encryptor = aes.CreateEncryptor();
        byte[] textBytes = Encoding.UTF8.GetBytes(text);
        byte[] encryptedBytes = encryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);
        return Convert.ToBase64String(encryptedBytes);
    }

    public static string Decrypte(string dtext)
    {
        ICryptoTransform decryptor = aes.CreateDecryptor();
        byte[] encryptedBytes = Convert.FromBase64String(dtext);
        byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}
