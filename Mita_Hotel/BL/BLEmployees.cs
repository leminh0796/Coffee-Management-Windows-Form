using Lemon3.Data;
using System.Data;
using System.Data.SqlClient;

namespace Mita_Coffee.BL
{
    class BLEmployees
    {
        public static DataTable LoadEmployees()
        {
            SqlCommand cmd = new SqlCommand("select ID, Fullname, Username, Role, Email, Phone, LastLogin, FirstDate from D00T0040 left join tblRole on D00T0040.RoleID = tblRole.RoleID");
            DataTable dt = L3SQLServer.ReturnDataTable(cmd.CommandText);
            return dt;
        }
        public static void DeleteEmployee(string Username)
        {
            SqlCommand cmd = new SqlCommand("DELETE D00T0040 where Username = '" + Username + "'");
            L3SQLServer.ExecuteSQL(cmd.CommandText);
        }
    }
}
