using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mita_Hotel.Models
{
    class InventoryItem
    {
        public string InventoryName { get; set; }
        public string UnitID { get; set; }
        public Decimal Price { get; set; }
        public string Notes { get; set; }
        public string BarCode { get; set; }
        public Decimal VAT { get; set; }
    }
}
