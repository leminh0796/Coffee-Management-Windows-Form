using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Lemon3;
using Lemon3.Data;
using Mita_Coffee.Models;

namespace Mita_Coffee.BL
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
        public static DataTable LoadTable()
        {
            return L3SQLServer.ReturnDataTable("select TableID, TableName, Position, TotalMoney, Status, People, IsPaid from D05T2010");
        }
        public static DataTable LoadStatusName()
        {
            return L3SQLServer.ReturnDataTable("select Status, StatusName from D05T2011");
        }
        public static DataTable LoadD05F2140(string TableID, int Status)
        {
            return L3SQLServer.ReturnDataTable("select VoucherID from D91T2140 WHERE TableID = '" + TableID + "' and Status = '" + Status + "'");
        }
        public static void DeleteTable(string TableID)
        {
            L3SQLServer.ExecuteSQL("DELETE D05T2010 where TableID = '" + TableID + "'");
        }
        public static void InitialTable(string TableID)
        {
            L3SQLServer.ExecuteSQL("UPDATE D05T2010 SET TotalMoney = 0, Status = 0, People = 0, IsPaid = 0 WHERE TableID = '" + TableID + "'");
        }
        public static DataTable SelectPayment(string TableID, string Status)
        {
            return L3SQLServer.ReturnDataTable("select VoucherID, Amount from D91T2140 WHERE TableID = '" + TableID + "' and Status = '" + Status + "'");
        }
        public static void UpdateMoney(string TotalMoney, string People, object TableID)
        {
            L3SQLServer.ExecuteSQL("UPDATE D05T2010 " +
                                   "SET TotalMoney = '" + TotalMoney + "', Status = 2, People = '" + People + "', IsPaid = 1" +
                                   "WHERE TableID = '" + TableID + "'");
        }
        public static void UpdateVoucherD91T2140(string AmountPayment, string VoucherID)
        {
            L3SQLServer.ExecuteSQL("UPDATE D91T2140 " +
                                   "SET Status = 2, AmountPayment = '" + AmountPayment + "'" +
                                   "WHERE VoucherID = '" + VoucherID + "'");
        }
        public static DataTable LoadSuggest()
        {
            return L3SQLServer.ReturnDataTable("select InventoryID, InventoryName, BarCode from D91T1040");
        }
        public static DataTable LoadVoucherDataGrid(string VoucherID)
        {
            return L3SQLServer.ReturnDataTable("exec D91P2140 " + VoucherID);
        }
        public static DataTable LoadMaster(string VoucherID)
        {
            return L3SQLServer.ReturnDataTable("SELECT * FROM D91T2140 WHERE VoucherID = " + VoucherID);
        }
        public static DataTable SelectInventoryID(string sInventoryID)
        {
            return L3SQLServer.ReturnDataTable("select * FROM D91T1040 where IsDelete = 0 and InventoryID = '" + sInventoryID + "'");
        }
        public static string SQLInsertD91T2140(string VoucherID, string TableID, object sePeopleValue, string sePeopleFormat, object seTotalMoneyValue, string seTotalMoneyFormat, int Status, decimal AmountPayment)
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.AppendLine("INSERT D91T2140 (VoucherID,VoucherDate,UserID,TableID,CountPerson,Amount,[Status],AmountPayment) ");
            sSQL.AppendLine("VALUES(");
            sSQL.AppendLine(L3SQLClient.SQLString(VoucherID) + L3.COMMA); //VoucherID'
            sSQL.AppendLine("getDate()" + L3.COMMA); //VoucherDate
            sSQL.AppendLine(L3SQLClient.SQLString(L3.UserID) + L3.COMMA); //UserID
            sSQL.AppendLine(L3SQLClient.SQLString(TableID) + L3.COMMA); // TableID
            sSQL.AppendLine(L3SQLClient.SQLMoney(sePeopleValue, sePeopleFormat) + L3.COMMA); //CountPerson
            sSQL.AppendLine(L3SQLClient.SQLMoney(seTotalMoneyValue, seTotalMoneyFormat) + L3.COMMA); //Amount
            sSQL.AppendLine(L3SQLClient.SQLString(Status) + L3.COMMA); //[Status]
            sSQL.AppendLine(L3SQLClient.SQLString(AmountPayment)); //AmountPayment
            sSQL.AppendLine(")");
            return sSQL.ToString();
        }
        public static string SQLDeleteD91T2140(string VoucherID)
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.AppendLine("DELETE D91T2140 WHERE VoucherID = " + L3SQLClient.SQLString(VoucherID));
            return sSQL.ToString();
        }
        public static string SQLDeleteD91T2141(string VoucherID)
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.AppendLine("DELETE D91T2141 WHERE VoucherID = " + L3SQLClient.SQLString(VoucherID));
            return sSQL.ToString();
        }
        public static string SQLInsertD91T2141s(DataTable dtGrid, string VoucherID)
        {
            StringBuilder sRet = new StringBuilder("--Luu them tin chi tiet");
            StringBuilder sSQL = new StringBuilder();
            foreach (DataRow dr in dtGrid.Rows)
            {
                sSQL = new StringBuilder();
                sSQL.AppendLine(" ");
                sSQL.AppendLine("INSERT D91T2141 (VoucherID, InventoryID, VAT,Price,UnitID,Quantity) ");
                sSQL.AppendLine("VALUES(");
                sSQL.AppendLine(L3SQLClient.SQLString(VoucherID) + L3.COMMA); //VoucherID'
                sSQL.AppendLine(L3SQLClient.SQLString(dr["InventoryID"]) + L3.COMMA); //InventoryID
                sSQL.AppendLine(dr["VAT"] + L3.COMMA); //VAT
                sSQL.AppendLine(L3SQLClient.SQLMoney(dr["Price"], "n0") + L3.COMMA); //Price
                sSQL.AppendLine(L3SQLClient.SQLString(dr["UnitID"]) + L3.COMMA); //UnitID
                sSQL.AppendLine(L3SQLClient.SQLMoney(dr["Quantity"], "n0")); //Quantity
                sSQL.AppendLine(")");
                sRet.AppendLine(sSQL.ToString());
            }
            return sRet.ToString();
        }
        public static string SQLUpdateStockD91T1040s(DataTable dtGrid)
        {
            StringBuilder sRet = new StringBuilder("--Luu them tin chi tiet");
            StringBuilder sSQL = new StringBuilder();
            foreach (DataRow dr in dtGrid.Rows)
            {
                sSQL = new StringBuilder();
                sSQL.AppendLine(" ");
                sSQL.AppendLine("UPDATE D91T1040 ");
                sSQL.AppendLine("SET InStock =");
                sSQL.AppendLine(dr["InStock"].ToString());
                sSQL.AppendLine("WHERE InventoryID =");
                sSQL.AppendLine(dr["InventoryID"].ToString());
                sRet.AppendLine(sSQL.ToString());
            }
            return sRet.ToString();
        }
        public static void UpdateStatus1(string TotalMoney, string People, string TableID)
        {
            L3SQLServer.ExecuteSQL("UPDATE D05T2010 " +
                                           "SET TotalMoney = '" + TotalMoney + "', Status = 1, People = '" + People + "'" +
                                           "WHERE TableID = '" + TableID + "'");
        }
        public static void UpdateStatus2(string TotalMoney, string People, string TableID)
        {
            L3SQLServer.ExecuteSQL("UPDATE D05T2010 " +
                                           "SET TotalMoney = '" + TotalMoney + "', Status = 2, People = '" + People + "', IsPaid = 1" +
                                           "WHERE TableID = '" + TableID + "'");
        }
    }
}
