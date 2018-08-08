using Lemon3.Controls.DevExp;
using Lemon3.Data;
using Mita_Coffee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Mita_Coffee.BL
{
    class BLVoucher
    {
        public static bool AddVoucher(Voucher voucher)
        {
            string sSQL = "INSERT INTO D91T2140 (VoucherID, VoucherDate, UserID, TableID, CountPerson, Amount, Status, AmountCustomerPaid)";
            sSQL += "VALUES(" + L3SQLClient.SQLString(voucher.VoucherID) + ", N" + L3SQLClient.SQLDateTimeSave(voucher.VoucherDate) + ", N" + L3SQLClient.SQLString(voucher.UserID) + ", " + L3SQLClient.SQLString(voucher.TableID) + ", 0, 0, " + voucher.Status + ", 0)";
            return L3SQLServer.ExecuteSQL(sSQL);
        }
        public static int GetCurrentRowIndex(L3GridControl Grid, string IDColumn)
        {
            try
            {
                string IDValue = Grid.GetFocusedRowCellValue(IDColumn).ToString();
                int n = Grid.FindRowByValue(IDColumn, IDValue);
                return n;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi!");
                return 0;
            }
        }

    }
}
