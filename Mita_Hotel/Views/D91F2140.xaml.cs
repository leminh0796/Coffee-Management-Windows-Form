using Lemon3.Controls.DevExp;
using Lemon3.Data;
using System;
using System.Collections.Generic;
using System.Data;
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
            LoadTDBGrid();
            L3DataSource.LoadDataSource(lkeTable, "select TableID, TableName from D05T2010");
            GridPBL.InputNumber288("n0", false, false, COL_Amount);
            GridPBL.InputNumber288("n0", false, false, COL_AmountPayment);
            LoadDateEdit();
        }

        private void LoadDateEdit()
        {
            DateEditSetting(deFrom);
            DateEditSetting(deTo);
            deFrom.EditValue = DateTime.Today.AddMonths(-1);
            deTo.EditValue = DateTime.Today;
        }

        private void DateEditSetting(DevExpress.Xpf.Editors.DateEdit de)
        {
            de.MaskType = DevExpress.Xpf.Editors.MaskType.DateTime;
            de.Mask = "dd/MM/yyyy";
            de.MaskUseAsDisplayFormat = true;
        }

        private void LoadTDBGrid()
        {
            L3DataSource.LoadDataSource(GridPBL, "select * from D91T2140");
        }
        
        private void OpenFormDetail()
        {
            D91F2141 frmDetail = new D91F2141();
            D91F2141.VoucherID = GridPBL.GetFocusedRowCellValue("VoucherID").ToString();
            frmDetail.lbVoucherID.Content = "Mã phiếu bán lẻ: " + D91F2141.VoucherID;
            frmDetail.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmDetail.ShowDialog();
        }

        private void miDetail_Click(object sender, RoutedEventArgs e)
        {
            OpenFormDetail();
        }

        private void GridPBL_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenFormDetail();
        }

        private string SQLStoreD91P2142(string TableID, int Mode)
        {
            return "EXEC D91P2142 '"+ deFrom.EditValue +"' , '"+ deTo.EditValue +"', '"+ TableID +"', "+ Mode +"";
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dtGrid = new DataTable();
                if (lkeTable.EditValue == null)
                {
                    dtGrid = L3SQLServer.ReturnDataTable(SQLStoreD91P2142("", 1));
                }
                else
                {
                    dtGrid = L3SQLServer.ReturnDataTable(SQLStoreD91P2142(lkeTable.EditValue.ToString(), 0));
                }
                L3DataSource.LoadDataSource(GridPBL, dtGrid);
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi!");
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            deFrom.EditValue = DateTime.Today.AddMonths(-1);
            deTo.EditValue = DateTime.Today;
            lkeTable.EditValue = null;
            LoadTDBGrid();
        }
    }
}
