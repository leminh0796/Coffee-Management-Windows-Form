using Lemon3.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mita_Coffee.BL
{
    class BLListType
    {
        public static DataTable LoadListType(string ListTypeID)
        {
            return L3SQLServer.ReturnDataTable("select * from D91T1240 WHERE ListTypeID = '" + ListTypeID + "'");
        }
        public static void DeleteListTypeID(string ListTypeID, string ListID)
        {
            L3SQLServer.ExecuteSQL("DELETE D91T1240 where ListTypeID = '" + ListTypeID + "' and ListID = '" + ListID + "'");
        }
    }
}
