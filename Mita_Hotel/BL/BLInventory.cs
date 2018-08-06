using Lemon3.Data;
using Mita_Coffee.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mita_Coffee.BL
{
    class BLInventory
    {
        public static DataTable LoadItem()
        {
            SqlCommand cmd = new SqlCommand("select InventoryID, InventoryName, ListName, Price, Notes, VAT, BarCode, InStock from D91T1040 left join D91T1240 on D91T1040.UnitID = D91T1240.ListID");
            DataTable dt = L3SQLServer.ReturnDataTable(cmd.CommandText);
            return dt;
        }
        public static void DeleteInventory(string InventoryID)
        {
            SqlCommand cmd = new SqlCommand("DELETE D91T1040 where InventoryID = '" + InventoryID + "'");
            L3SQLServer.ExecuteSQL(cmd.CommandText);
        }
        public static DataTable LoadUnitID()
        {
            DataTable dt1 = L3SQLServer.ReturnDataTable("select ListTypeID, ListID, ListName, Description FROM D91T1240 WHERE ListTypeID = 'UnitID'");
            return dt1;
        }
        public static DataTable LoadInventory(string InventoryID)
        {
            DataTable dt = L3SQLServer.ReturnDataTable("select InventoryName, UnitID, Price, Notes, VAT, BarCode, ImageLocation from D91T1040 WHERE InventoryID = '" + InventoryID + "'");
            return dt;
        }
        public static bool AddNewItem(object InventoryName, object UnitID, object Price, object Notes, object VAT, object BarCode, object CreateUserID, object ImageLocation)
        {
            return L3SQLServer.ExecuteNoneQuery("sp_AddItem",
               CommandType.StoredProcedure,
               new string[] { "InventoryName", "UnitID", "Price", "Notes", "VAT", "BarCode", "CreateUserID", "ImageLocation" }
               , new object[] { InventoryName, UnitID, Price, Notes, VAT, BarCode, CreateUserID, ImageLocation});
        }
        public static bool EditItem(object InventoryID, object InventoryName, object UnitID, object Price, object Notes, object VAT, object BarCode, object LastModifyUserID, object ImageLocation)
        {
            return L3SQLServer.ExecuteNoneQuery("sp_EditItem",
               CommandType.StoredProcedure,
               new string[] { "InventoryID", "InventoryName", "UnitID", "Price", "Notes", "VAT", "BarCode", "LastModifyUserID", "ImageLocation" }
               , new object[] { InventoryID, InventoryName, UnitID, Price, Notes, VAT, BarCode, LastModifyUserID, ImageLocation });
        }
    }
}
