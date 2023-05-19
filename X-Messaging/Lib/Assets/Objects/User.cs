using Lib.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Assets.Objects;
public class User_
{
    protected int id;
    protected string name;
    protected string password;
    protected string login;

    public User_()
    {
        id = 0;
        password = name = string.Empty;
    }
    public User_(string name, string password = "")
    {
        this.name = name;
        this.password = password;
    }
    public User_(int id, string name, string password = "") : this(name, password)
    {
        this.id = id;
    }

    public int? Id
    {
        get => id;
        set
        {
            if(value is not null)
            {
                this.id = value.Value;
            }
        }
    }
    public string? Name
    {
        get => name;
        set =>this.name = value ?? string.Empty;
    }
    public string? Login
    {
        get => login;
        set => login = value ?? string.Empty;
    }
    public string? Password
    {
        get => password;
        set
        {
            if (value is null)
            {
                password = string.Empty;
                return;
            }

            this.password = Hasher.GetHash(value);
        }
    }

    public override string ToString() => $"ID: {Id}\tName: {Name}\t";
    public override bool Equals(object? obj) => obj is User_ u && u.Id.Equals(Id);
    public override int GetHashCode() => Id.GetHashCode() + Name?.GetHashCode() ?? 2453 % Password?.GetHashCode() ?? 23;
}
