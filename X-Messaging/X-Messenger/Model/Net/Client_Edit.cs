using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using X_Messenger.Model.Database;
using X_Messenger.Model.Database.DBObjects;
using X_Messenger.Model.Objects;

namespace X_Messenger.Model.Net;

internal partial class Client
{
    public async Task<int> EditAsync(User? user) => await Task.Run(() => Edit(user));
    public int Edit(User? user)
    {
        if(user is null)
        {
            return -3;
        }

        using DB db = new();
        return db.UpdateObject<DBUser>(new(user));
    }
}
