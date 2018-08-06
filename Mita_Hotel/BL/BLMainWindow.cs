using Lemon3.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mita_Coffee.BL
{
    class BLMainWindow
    {
        public static DataTable GetUserLogged (string UserID)
        {
            DataTable dt = L3SQLServer.ReturnDataTable("select Fullname, LastLogin, RoleID from D00T0040 WHERE Username = '" + UserID + "'");
            return dt;
        }
    }
}
