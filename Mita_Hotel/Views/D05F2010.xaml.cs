using Lemon3;
using Lemon3.Controls.DevExp;
using Lemon3.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mita_Hotel.Views
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

        private void L3PageLoaded(object sender, RoutedEventArgs e)
        {

            GridTable.InputNumber288("n0", false, false, COL_TotalMoney);
            GridTable.SetDefaultFilterChangeGrid();
            if (dt.Rows.Count == 0)
            {
                LoadTableGrid();
            }
            L3Control.SetShortcutPopupMenu(MainMenuControl);
        }

        private SqlDataAdapter da = new SqlDataAdapter();
        private DataTable dt = new DataTable();

        public void LoadTableGrid()
        {
            //SqlCommand cmd = new SqlCommand("select TableID, TableName, Position, TotalMoney, StatusName, People, IsPaid from D05T2010 left join D05T2011 on D05T2010.Status = D05T2011.Status");
            //DataTable dt = L3SQLServer.ReturnDataTable(cmd.CommandText);
            //GridTable.ItemsSource = dt;
            //lkesStatus.ItemsSource = L3SQLServer.ReturnDataTable("select Status, StatusName from D05T2011");
            SqlConnection conn = new SqlConnection(L3.ConnectionString);
            conn.Open();
            da = new SqlDataAdapter("select * from D05T2010", conn);
            SqlCommandBuilder builder = new SqlCommandBuilder(da);
            da.Fill(dt);
            conn.Close();
            BindingSource bSource = new BindingSource();
            bSource.DataSource = dt;
            GridTable.ItemsSource = bSource;
            lkesStatus.ItemsSource = L3SQLServer.ReturnDataTable("select Status, StatusName from D05T2011");
            dt.RowChanged += (o, arg) =>
            {
                da.Update(dt);
            };
        }

        public void LoadSimple()
        {
            DataTable dt = L3SQLServer.ReturnDataTable("select * from D05T2010");
            GridTable.ItemsSource = dt;
            lkesStatus.ItemsSource = L3SQLServer.ReturnDataTable("select Status, StatusName from D05T2011");
        }

        private void mnsAdd_Click(object sender, RoutedEventArgs e)
        {
            D05F2140 frmTable = new D05F2140();
            int i = GridTable.View.FocusedRowData.RowHandle.Value;
            frmTable.IsBooking = true;
            frmTable.TableID = GridTable.GetFocusedRowCellValue("TableID").ToString();
            frmTable.lbTableName.Content = "Tên bàn: " + GridTable.GetFocusedRowCellValue("TableName").ToString();
            frmTable.lkeStatus.EditValue = 2;
            frmTable.lkeStatus.ItemsSource = L3SQLServer.ReturnDataTable("select Status, StatusName from D05T2011");
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
                SqlCommand cmd = new SqlCommand("DELETE D05T2010 where TableID = '" + TableID + "'");
                L3SQLServer.ExecuteSQL(cmd.CommandText);
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

        private void mnsAdd_Loaded(object sender, RoutedEventArgs e)
        {
            string temp = GridTable.GetFocusedRowCellValue("Status").ToString();
            if (temp == "0")
            {
                mnsAdd.IsEnabled = true;
            }
        }
    }
}
