using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lib.Assets.Checkers;

public sealed class FormDataChecker : IChecker
{
    private readonly static Regex regName = new Regex("^([A-Z]([a-z]{0,15}))|([А-Я]([а-я]{0,15}))$");
    private readonly static Regex regLogin = new Regex("^([A-Za-zА-Яа-я0-9]){6,30}$");
    private readonly static Regex regPassword = new Regex("^[A-Za-zА-Яа-я0-9_!]{8,64}$");
    
    public bool CheckName(string? name) => this.Check(name, regName) && !name.Trim().Contains(" ") && !name.Trim().Contains("   ") && !this.Check(name, new("[0-9]+"));
    public bool CheckLogin(string? login) => this.Check(login, regLogin);
    public bool CheckPassword(string? pass) => this.Check(pass, regPassword);

    public bool Check(string? name, Regex? reg) => name is not null && reg is not null && reg.IsMatch(name);

}
