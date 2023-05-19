using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Messenger.Model.Assets;

internal static class MyExtentions
{
    public static int LevenshteinDistance(this string s, string t)
    {
        int m = s.Length;
        int n = t.Length;

        if (m == 0)
        {
            return n;
        }

        if (n == 0)
        {
            return m;
        }

        int[,] d = new int[m + 1, n + 1];

        for (int i = 0; i <= m; i++)
        {
            d[i, 0] = i;
        }

        for (int j = 0; j <= n; j++)
        {
            d[0, j] = j;
        }

        for (int j = 1; j <= n; j++)
        {
            for (int i = 1; i <= m; i++)
            {
                int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
            }
        }

        return d[m, n];
    }
}
