using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Messenger.Model.Objects;

internal class Statistics
{
    public int CountUsers { get; set; }
    public int CountAdmins { get; set; }
    public int CountNewUsersForDay { get; set; }
    public int CountNewUsersForWeek { get; set; }
    public int CountStickers { get; set; }
}
