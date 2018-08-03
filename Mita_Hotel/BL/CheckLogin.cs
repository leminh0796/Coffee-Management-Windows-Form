using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Lemon3;
using Lemon3.Data;

namespace Mita_Hotel.Login
{

}

namespace Mita_Hotel.Setup
{
    class CheckLogin
    {

        public static bool IfLogin(string username, string MD5password)
        {
            bool valid = false;
            SqlConnection conn = new SqlConnection(L3.ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_CheckLogin", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
            cmd.Parameters.Add("@MD5Password", SqlDbType.Char).Value = MD5password;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0) valid = true;
            return valid;
        }
        public static bool IfUsernameValid(string username)
        {
            SqlCommand cmd2 = new SqlCommand("select Username from D00T0040 Where Username = '" + username + "'");
            return L3SQLServer.ExecuteSQL(cmd2.CommandText); ;
        }
        
    }
}
