using Lemon3;
using Lemon3.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mita_Coffee.BL
{
    class BLPermission
    {
        static public string SQLDeleteD00T2040(string Username)
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.AppendLine("DELETE D00T2040 WHERE UserID = " + L3SQLClient.SQLString(Username));
            return sSQL.ToString();
        }

        static public string SQLInsertD00T2040(string Username, string FormID, int Permission)
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.AppendLine("INSERT D00T2040 (UserID, FormID, Permission)");
            sSQL.AppendLine("VALUES(");
            sSQL.AppendLine(L3SQLClient.SQLString(Username) + L3.COMMA); //Username
            sSQL.AppendLine(L3SQLClient.SQLString(FormID) + L3.COMMA); //FormID
            sSQL.AppendLine(L3SQLClient.SQLMoney(Permission, "n0")); //Permission
            sSQL.AppendLine(")");
            return sSQL.ToString();
        }

        public static int[] GetPermissionFromDB(string Username)
        {
            int[] CheckListNumber = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            DataTable dt = L3SQLServer.ReturnDataTable("select FormID, UserID, Permission from D00T2040 WHERE UserID = " + L3SQLClient.SQLString(Username));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    switch(row["FormID"].ToString())
                    {
                        case "D00F0040":
                            CheckListNumber[0] = Convert.ToInt32(row["Permission"]);
                            break;
                        case "D00F1040":
                            CheckListNumber[1] = Convert.ToInt32(row["Permission"]);
                            break;
                        case "D05F2010":
                            CheckListNumber[2] = Convert.ToInt32(row["Permission"]);
                            break;
                        case "D91F2140":
                            CheckListNumber[3] = Convert.ToInt32(row["Permission"]);
                            break;
                        case "D07F2010":
                            CheckListNumber[4] = Convert.ToInt32(row["Permission"]);
                            break;
                        case "D91F1240Location":
                            CheckListNumber[5] = Convert.ToInt32(row["Permission"]);
                            break;
                        case "D91F1240UnitID":
                            CheckListNumber[6] = Convert.ToInt32(row["Permission"]);
                            break;
                        case "D91F1240Object":
                            CheckListNumber[7] = Convert.ToInt32(row["Permission"]);
                            break;
                    }
                }
            }
            return CheckListNumber;
        }
    }
}
