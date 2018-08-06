using Lemon3;
using Lemon3.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mita_Coffee.BL
{
    class BLRegistration
    {
        public static bool IfReg(string Username, string MD5Password, string Fullname, string RoleID, string Email, int Phone)
        {
            bool regAccepted = false;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = L3.ConnectionString;
            SqlCommand cmd = new SqlCommand("sp_Reg", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = Username;
            cmd.Parameters.Add("@MD5Password", SqlDbType.Char).Value = MD5Password;
            cmd.Parameters.Add("@Fullname", SqlDbType.NVarChar).Value = Fullname;
            cmd.Parameters.Add("@RoleID", SqlDbType.Char).Value = RoleID;
            cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;
            cmd.Parameters.Add("@Phone", SqlDbType.Int).Value = Phone;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0) regAccepted = true;
            return regAccepted;
        }
        public static bool IfUsernameAlreadyExist(string Username)
        {
            bool uvalid = false;
            SqlConnection conn2 = new SqlConnection();
            conn2.ConnectionString = L3.ConnectionString;
            SqlCommand cmd2 = new SqlCommand("select Username from D00T0040 Where Username = '" + Username + "'", conn2);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            if (dt2.Rows.Count > 0) uvalid = true;
            return uvalid;
        }
    }
}
