using Lemon3.Controls.DevExp;
using Lemon3.Data;
using Mita_Coffee.BL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Mita_Coffee.Views
{
    /// <summary>
    /// Interaction logic for D05F2010.xaml
    /// </summary>
    public partial class D05F2010 : L3Page
    {
        public D05F2010()
        {
            InitializeComponent();
        }

        public override void SetContentForL3Page()
        {
        }
        private bool bLoaded = false;

        private void L3PageLoaded(object sender, RoutedEventArgs e)
        {

            GridTable.InputNumber288("n0", false, false, COL_TotalMoney);
            GridTable.SetDefaultFilterChangeGrid();
            L3Control.SetShortcutPopupMenu(MainMenuControl);
            try
            {
                if (dt.Rows.Count == 0)
                {
                    LoadSimple();
                    SetTimer();
                }
                bLoaded = true;
            }
            catch (SqlException)
            {
                System.Windows.MessageBox.Show("Lỗi!");
                bLoaded = false;
            }
        }
        private void SetTimer()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Start();
        }
        protected void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (bLoaded == true)
            {
                int i = BLVoucher.GetCurrentRowIndex(GridTable, "TableID");
                LoadSimple();
                GridTable.FocusRowHandle(i);
            }
        }

        private SqlDataAdapter da = new SqlDataAdapter();
        private DataTable dt = new DataTable();
        

        public void LoadSimple()
        {
            GridTable.ItemsSource = BLTable.LoadTable();
            lkesStatus.ItemsSource = BLTable.LoadStatusName();
        }

        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }
        
        private D05F2140 LoadD05F2140()
        {
            D05F2140 frmTable = new D05F2140();
            string TableID = GridTable.GetFocusedRowCellValue("TableID").ToString();
            frmTable.Status = Convert.ToInt32(GridTable.GetFocusedRowCellValue("Status"));
            frmTable.TableID = TableID;
            frmTable.AmountPayment = 0;
            frmTable.lbTableName.Content = "Tên bàn: " + GridTable.GetFocusedRowCellValue("TableName").ToString();
            if (frmTable.Status == 0)
            {
                frmTable.VoucherID = "";
            }
            else
            {
                DataTable dt = BLTable.LoadD05F2140(TableID, frmTable.Status);
                if (dt.Rows.Count > 0) frmTable.VoucherID = dt.Rows[dt.Rows.Count - 1]["VoucherID"].ToString();
                else frmTable.VoucherID = "";
            }
            return frmTable;
        }

        private void mnsAdd_Click(object sender, RoutedEventArgs e)
        {
            D05F2140 frmTable = LoadD05F2140();
            int i = GridTable.View.FocusedRowData.RowHandle.Value;
            frmTable.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmTable.ShowDialog();
            LoadSimple();
            GridTable.FocusRowHandle(i);
        }

        private void mnsRead_Click(object sender, RoutedEventArgs e)
        {
            D05F2140 frmTable = LoadD05F2140();
            int i = GridTable.View.FocusedRowData.RowHandle.Value;
            frmTable.btnSave.Visibility = Visibility.Hidden;
            frmTable.btnPay.Visibility = Visibility.Hidden;
            frmTable.btnAdd10Quatity.Visibility = Visibility.Hidden;
            frmTable.btnAddQuatity.Visibility = Visibility.Hidden;
            frmTable.btnDeleteQuatity.Visibility = Visibility.Hidden;
            frmTable.txtItem.IsEnabled = false;
            frmTable.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmTable.ShowDialog();
            LoadSimple();
            GridTable.FocusRowHandle(i);
        }

        private void tsbAdd_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            D05F2011 frm = new D05F2011();
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.ShowDialog();
            LoadSimple();
            GridTable.FocusRowHandle(GridTable.ReturnVisibleRowCount - 1);
        }

        private void tsbEdit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            D05F2011 frm = new D05F2011();
            int i = 0;
            try
            {
                frm.TableID = GridTable.GetFocusedRowCellValue("TableID").ToString();
                i = GridTable.View.FocusedRowData.RowHandle.Value;
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Chọn 1 dòng để sửa không phải dòng này!");
            }
            frm.FormState = Lemon3.EnumFormState.FormEdit;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.ShowDialog();
            LoadSimple();
            GridTable.FocusRowHandle(i);
        }

        private void tsbDelete_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                string TableID = GridTable.GetFocusedRowCellValue("TableID").ToString();
                BLTable.DeleteTable(TableID);
                LoadSimple();
                GridTable.FocusRowHandle(GridTable.ReturnVisibleRowCount - 1);
            }
            catch (NullReferenceException)
            {
                System.Windows.MessageBox.Show("Lỗi không thể xóa được bảng trống");
            }
        }

        private void tsbListAll_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GridTable.ListAll();
        }

        private void cmTable_Loaded(object sender, RoutedEventArgs e)
        {
            mnsNew.IsEnabled = false;
            mnsPay.IsEnabled = true;
            if (Convert.ToInt32(GridTable.GetFocusedRowCellValue("Status")) == 0)
            {
                mnsPay.IsEnabled = false;
            }
            if (Convert.ToInt32(GridTable.GetFocusedRowCellValue("Status")) == 2)
            {
                mnsNew.IsEnabled = true;
                mnsPay.IsEnabled = false;
                mnsAdd.IsEnabled = false;
            }
            if (Convert.ToInt32(GridTable.GetFocusedRowCellValue("Status")) == 1 && Convert.ToDecimal(GridTable.GetFocusedRowCellValue("TotalMoney")) == 0)
            {
                mnsNew.IsEnabled = true;
                mnsPay.IsEnabled = false;
            }
        }

        private void mnsNew_Click(object sender, RoutedEventArgs e)
        {
            BLTable.InitialTable(GridTable.GetFocusedRowCellValue("TableID").ToString());
            LoadSimple();
            mnsNew.IsEnabled = false;
            mnsPay.IsEnabled = true;
            mnsAdd.IsEnabled = true;
        }

        private void mnsPay_Click(object sender, RoutedEventArgs e)
        {
            string VoucherID = "";
            DataTable dt = BLTable.SelectPayment(GridTable.GetFocusedRowCellValue("TableID").ToString(), GridTable.GetFocusedRowCellValue("Status").ToString());
            if (dt.Rows.Count > 0) VoucherID = dt.Rows[dt.Rows.Count - 1]["VoucherID"].ToString();
            D05F2141 frmPayment = new D05F2141();
            frmPayment.TotalMoney = Convert.ToDecimal(dt.Rows[dt.Rows.Count - 1]["Amount"]);
            frmPayment.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmPayment.ShowDialog();
            if (!frmPayment.bClicked) return;
            decimal AmountPayment = frmPayment.TotalMoney;
            BLTable.UpdateMoney(L3SQLClient.SQLMoney(GridTable.GetFocusedRowCellValue("TotalMoney"), "n0"), L3SQLClient.SQLMoney(GridTable.GetFocusedRowCellValue("People"), "n0"), GridTable.GetFocusedRowCellValue("TableID"));
            BLTable.UpdateVoucherD91T2140(L3SQLClient.SQLMoney(AmountPayment, "n0"), VoucherID);
            LoadSimple();
        }
    }
}
