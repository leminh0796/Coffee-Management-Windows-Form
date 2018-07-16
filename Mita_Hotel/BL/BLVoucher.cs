using Lemon3.Data;
using Mita_Hotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mita_Hotel.BL
{
    class BLVoucher
    {
        public static bool AddVoucher(Voucher voucher)
        {
            string sSQL = "INSERT INTO D91T2140 (VoucherID, VoucherDate, UserID, TableID, CountPerson, Amount, Status, AmountCustomerPaid)";
            sSQL += "VALUES(" + L3SQLClient.SQLString(voucher.VoucherID) + ", N" + L3SQLClient.SQLDateTimeSave(voucher.VoucherDate) + ", N" + L3SQLClient.SQLString(voucher.UserID) + ", " + L3SQLClient.SQLString(voucher.TableID) + ", 0, 0, " + voucher.Status + ", 0)";
            return L3SQLServer.ExecuteSQL(sSQL);
        }
    }
}
