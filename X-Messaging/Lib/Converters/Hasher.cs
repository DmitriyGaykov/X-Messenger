using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Converters;

public static class Hasher
{
    public static string GetHash(string text)
    {
        using var sha256 = SHA256.Create();
        var buffer = sha256.ComputeHash(Encoding.UTF8.GetBytes(text)).Select(el => Convert.ToChar(el));
        var hash = string.Join("", buffer);
        return hash;
    }
}
