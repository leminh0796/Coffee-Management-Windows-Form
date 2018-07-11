using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mita_Hotel.Models
{
    class Voucher
    {
        public string VoucherID { get; set; }
        public DateTime VoucherDate { get; set; }
        public string UserID { get; set; }
        public string TableID { get; set; }
        public int CountPerson { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public decimal AmountCustomerPaid { get; set; }
    }
}
