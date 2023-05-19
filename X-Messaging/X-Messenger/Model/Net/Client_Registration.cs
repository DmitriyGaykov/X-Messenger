using Lib.Assets.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Messenger.Model.Database;
using X_Messenger.Model.Database.DBObjects;
using X_Messenger.Model.Objects;

namespace X_Messenger.Model.Net;

internal partial class Client
{
    public bool DoRegistration(User? user)
    {
        if (user is null) return false;
        using var db = new DB();

        DBUser dbUser = new(user);
        user.Id = db.InsertObject(dbUser);

        return user.Id > 0;
    }

    public async Task<bool> DoRegistrationAsync(User? user) => await Task.Run(() =>  DoRegistration(user));
}
