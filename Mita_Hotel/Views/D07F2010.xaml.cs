using Lemon3.Controls.DevExp;
using Lemon3.Data;
using Lemon3.Functions;
using Mita_Hotel.BL;
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
    /// Interaction logic for D07F2010.xaml
    /// </summary>
    public partial class D07F2010 : L3Page
    {
        public D07F2010()
        {
            InitializeComponent();
        }
        public override void SetContentForL3Page()
        {
        }

        private void L3Page_Loaded(object sender, RoutedEventArgs e)
        {
            GridStore.InputNumber288("n0", false, false, COL_Amount);
            GridStore.SetDefaultFilterChangeGrid();
            LoadTDBGrid();
        }

        private void LoadTDBGrid()
        {
            DataTable dt = L3SQLServer.ReturnDataTable("select VoucherID, VoucherDate, ObjectID, Amount, IsPayment from D07T2010");
            GridStore.ItemsSource = dt;
        }



        private void tsbAdd_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            D07F2011 frmStore = new D07F2011();
            int i = GridStore.View.FocusedRowData.RowHandle.Value;
            frmStore.VoucherID = "";
            frmStore.FormState = Lemon3.EnumFormState.FormAdd;
            frmStore.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmStore.ShowDialog();
            LoadTDBGrid();
            GridStore.FocusRowHandle(i);
        }

        private void tsbEdit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            D07F2011 frm = new D07F2011();
            int i = GridStore.View.FocusedRowData.RowHandle.Value;
            frm.VoucherID = L3ConvertType.L3String(GridStore.GetFocusedRowCellValue(COL_VoucherID));
            frm.FormState = Lemon3.EnumFormState.FormEdit;
            frm.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            frm.ShowDialog();
            LoadTDBGrid();
            GridStore.FocusRowHandle(i);
        }

        private void tsbDelete_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            string VoucherID = GridStore.GetFocusedRowCellValue(COL_VoucherID).ToString();
            int i = GridStore.View.FocusedRowData.RowHandle.Value;
            StringBuilder sSQL = new StringBuilder();
            sSQL.AppendLine(BLStore.SQLDeleteD07T2011(VoucherID));
            sSQL.AppendLine(BLStore.SQLDeleteD07T2010(VoucherID));
            try
            {
                L3SQLServer.ExecuteSQL(sSQL.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi!");
            }
            LoadTDBGrid();
            GridStore.FocusRowHandle(i);
        }

        private void tsbView_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            D07F2011 frm = new D07F2011();
            int i = GridStore.View.FocusedRowData.RowHandle.Value;
            frm.VoucherID = L3ConvertType.L3String(GridStore.GetFocusedRowCellValue(COL_VoucherID));
            frm.FormState = Lemon3.EnumFormState.FormView;
            frm.GridVoucherInventory.View.IsEnabled = false;
            frm.lkeSupplier.IsReadOnly = true;
            frm.deVoucherDate.IsReadOnly = true;
            frm.chePaid.IsReadOnly = true;
            frm.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            frm.ShowDialog();
            LoadTDBGrid();
            GridStore.FocusRowHandle(i);
        }

        private void GridStore_CustomColumnDisplayText(object sender, DevExpress.Xpf.Grid.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "IsPayment")
            {
                if (e.Value.ToString() == "1") e.DisplayText = "OK";
                else e.DisplayText = "Chưa";
            }
        }
    }
}
