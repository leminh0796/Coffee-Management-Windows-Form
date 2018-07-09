using Lemon3.Controls.DevExp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mita_Hotel.Views
{
    /// <summary>
    /// Interaction logic for D91F2140.xaml
    /// </summary>
    public partial class D91F2140 : L3Page
    {
        public D91F2140()
        {
            InitializeComponent();
        }

        public override void SetContentForL3Page()
        {
        }

        private void L3Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPBLGrid();
            GridPBL.InputNumber288("n0", false, false, COL_Amount);
            GridPBL.InputNumber288("n0", false, false, COL_AmountCustomerPaid);
        }
        private void LoadPBLGrid()
        {
            L3DataSource.LoadDataSource(GridPBL, "select * from D91T2140");
        }

        private void miDetail_Click(object sender, RoutedEventArgs e)
        {
            D91F2141 frmDetail = new D91F2141();
            D91F2141.VoucherID = GridPBL.GetFocusedRowCellValue("VoucherID").ToString();
            frmDetail.lbVoucherID.Content = "Mã phiếu bán lẻ: " + D91F2141.VoucherID;
            frmDetail.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmDetail.ShowDialog();
        }
    }
}
