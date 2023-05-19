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
    public bool DoAuthorization(User? user)
    {
        if (user is null) return false;

        using DB db = new();

        DBUser dbUser = new(user);

        user.Id = db.EqualsObject(dbUser);

        if (user.Id > 0) CurrentUser = dbUser;

        return user.Id > 0;
    }

    public async Task<bool> DoAuthorizationAsync(User? user) => await Task.Run(() => DoAuthorization(user));
}
