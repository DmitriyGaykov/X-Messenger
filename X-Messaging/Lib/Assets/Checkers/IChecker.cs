using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lib.Assets.Checkers;

public interface IChecker
{
    bool Check(string? name, Regex? reg);
}
