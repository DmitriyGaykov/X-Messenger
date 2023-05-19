using Lib.Converters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace X_Messenger.Model.Objects;

internal class User
{
    protected int id;
    protected int adminLvl;
    protected string name;
    protected string password;
    protected string login;
    protected string descr;
    protected DateTime lastVisit;
    protected BitmapImage image;
    protected bool isBanned = false;

    public User()
    {
        id = 0;
        password = name = string.Empty;
    }
    public User(string name, string password = "")
    {
        this.name = name;
        this.password = password;
    }
    public User(int id, string name, string password = "") : this(name, password)
    {
        this.id = id;
    }

    public int? Id
    {
        get => id;
        set
        {
            if (value is not null)
            {
                this.id = value.Value;
            }
        }
    }

    public int? AdminLvl
    {
        get => adminLvl;
        set => adminLvl = value ?? 0;
    }

    public string? Description
    {
        get => descr;
        set => descr = value ?? "I'm closed";
    }

    public string? Name
    {
        get => name;
        set => this.name = value;
    }

    public bool IsBanned
    {
        get => isBanned;
        set => isBanned = value;
    }

    public BitmapImage Source
    {
        get => image;
        set => image = value;
    }

    public DateTime? LastVisit
    {
        get => lastVisit;
        set => lastVisit = value ?? DateTime.Now;
    }

    public string? Login
    {
        get => login;
        set => login = value;
    }
    public string? Password
    {
        get => password;
        set
        {
            if (value is null)
            {
                password = value;
                return;
            }

            this.password = Hasher.GetHash(value);
        }
    }

    public override string ToString() => $"ID: {Id}\tName: {Name}\t";
    public override bool Equals(object? obj) => obj is User u && u.Id.Equals(Id);
    public override int GetHashCode() => Id.GetHashCode() + Name?.GetHashCode() ?? 2453 % Password?.GetHashCode() ?? 23;
}
