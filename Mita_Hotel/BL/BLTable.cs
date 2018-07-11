using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Lemon3.Data;
using Mita_Hotel.Models;

namespace Mita_Hotel.BL
{
    class BLTable
    {
        public static bool AddTable(Table table)
        {
            string sSQL = "INSERT INTO D05T2010 (TableID, TableName, Position, TotalMoney, Status, People, IsPaid)";
            sSQL += "VALUES(" + L3SQLClient.SQLString(table.TableID) + ", N" + L3SQLClient.SQLString(table.TableName) + ", N"+ L3SQLClient.SQLString(table.Position) + ", " + L3SQLClient.SQLMoney(0) + ", 0, 0, 0)";
            return L3SQLServer.ExecuteSQL(sSQL);
        }
        public static DataTable GetTable(string TableID)
        {
            return L3SQLServer.ReturnDataTable("select * FROM  D05T2010 where TableID =" + L3SQLClient.SQLString(TableID));
        }
        public static bool UpdateTable(Table table)
        {
            return L3SQLServer.ExecuteSQL("UPDATE D05T2010 SET TableName = N" + L3SQLClient.SQLString(table.TableName) + ", Position = N" + L3SQLClient.SQLString(table.Position) + " WHERE TableID = " + L3SQLClient.SQLString(table.TableID) + "");
        }
    }
}
