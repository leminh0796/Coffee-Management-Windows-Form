using Lemon3;
using Lemon3.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mita_Hotel.BL
{
    class BLStore
    {
        static public string SQLStoreD07P2011(string VoucherID, int Mode)
        {
            return "EXEC D07P2011 " + L3SQLClient.SQLString(VoucherID) + " ," + L3SQLClient.SQLNumber(Mode);
        }

        static public DataTable ReturnObjectIDTable()
        {
            return L3SQLServer.ReturnDataTable("select * FROM D91T1240 WHERE ListTypeID = 'ObjectID'");
        }

        static public DataTable ReturnLookupInventory()
        {
            return L3SQLServer.ReturnDataTable("select InventoryID, InventoryName, UnitID, VAT, Price FROM D91T1040");
        }

        static public string SQLInsertD07T2010(string VoucherID, string ObjectID, double Amount, int IsPayment)
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.AppendLine("INSERT D07T2010 (VoucherID,VoucherDate,ObjectID,Amount,IsPayment)");
            sSQL.AppendLine("VALUES(");
            sSQL.AppendLine(L3SQLClient.SQLString(VoucherID) + L3.COMMA); //VoucherID'
            sSQL.AppendLine("getDate()" + L3.COMMA); //VoucherDate
            sSQL.AppendLine(L3SQLClient.SQLString(ObjectID) + L3.COMMA); //ObjectID
            sSQL.AppendLine(L3SQLClient.SQLMoney(Amount, "n0") + L3.COMMA); //Amount
            sSQL.AppendLine(L3SQLClient.SQLString(IsPayment)); // IsPayment
            sSQL.AppendLine(")");
            return sSQL.ToString();
        }

        static public string SQLDeleteD07T2010(string VoucherID)
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.AppendLine("DELETE D07T2010 WHERE VoucherID = " + L3SQLClient.SQLString(VoucherID));
            return sSQL.ToString();
        }

        static public string SQLDeleteD07T2011(string VoucherID)
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.AppendLine("DELETE D07T2011 WHERE VoucherID = " + L3SQLClient.SQLString(VoucherID));
            return sSQL.ToString();
        }

        static public string SQLInsertD07T2011s(string VoucherID, DataTable dtGrid)
        {
            StringBuilder sRet = new StringBuilder("--Luu them tin chi tiet");
            StringBuilder sSQL = new StringBuilder();
            foreach (DataRow dr in dtGrid.Rows)
            {
                sSQL = new StringBuilder();
                sSQL.AppendLine(" ");
                sSQL.AppendLine("INSERT D07T2011 (VoucherID, InventoryID, VAT, Price, UnitID, Quantity, Discount, Amount) ");
                sSQL.AppendLine("VALUES(");
                sSQL.AppendLine(L3SQLClient.SQLString(VoucherID) + L3.COMMA); //VoucherID
                sSQL.AppendLine(L3SQLClient.SQLString(dr["InventoryID"]) + L3.COMMA); //InventoryID
                sSQL.AppendLine( dr["VAT"] + L3.COMMA);
                sSQL.AppendLine(L3SQLClient.SQLMoney(dr["Price"], "n0") + L3.COMMA); //Price
                sSQL.AppendLine(L3SQLClient.SQLString(dr["UnitID"]) + L3.COMMA); //UnitID
                sSQL.AppendLine(L3SQLClient.SQLMoney(dr["Quantity"], "n0") + L3.COMMA); //Quantity
                sSQL.AppendLine( dr["Discount"] + L3.COMMA); //Discount
                sSQL.AppendLine(L3SQLClient.SQLMoney(dr["Amount"], "n0")); //Amount
                sSQL.AppendLine(")");
                sRet.AppendLine(sSQL.ToString());
            }
            return sRet.ToString();
        }

        static public string UpdateStockD91T1040(DataTable dtGrid)
        {
            StringBuilder sRet = new StringBuilder("--Luu them tin chi tiet");
            StringBuilder sSQL = new StringBuilder();
            foreach (DataRow dr in dtGrid.Rows)
            {
                sSQL = new StringBuilder();
                sSQL.AppendLine(" ");
                sSQL.AppendLine("UPDATE D91T1040 ");
                sSQL.AppendLine("SET InStock +=");
                sSQL.AppendLine(dr["Quantity"].ToString());
                sSQL.AppendLine("WHERE InventoryID =");
                sSQL.AppendLine(dr["InventoryID"].ToString());
                sRet.AppendLine(sSQL.ToString());
            }
            return sRet.ToString();
        }

    }
}
