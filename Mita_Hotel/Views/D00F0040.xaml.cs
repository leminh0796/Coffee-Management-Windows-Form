using Lemon3.Controls.DevExp;
using Lemon3.Data;
using Mita_Coffee.BL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Mita_Coffee.Views
{
    /// <summary>
    /// Interaction logic for pageListUser.xaml
    /// </summary>
    public partial class D00F0040 : L3Page
    {
        public D00F0040() //The Employees List Form.
        {
            InitializeComponent();
        }

        public override void SetContentForL3Page()
        {
        }

        private void L3Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGrid();
            GridListUser.SetDefaultFilterChangeGrid();
            L3Control.SetShortcutPopupMenu(MainMenuControl1);
        }
        public void LoadGrid() //Get the employees list.
        {
            DataTable dt = BLEmployees.LoadEmployees();
            GridListUser.ItemsSource = dt;
        }
        
        public static bool IsEdit = false;
        public static string Username = "";

        private void tsbEdit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            frmRegister frmRegister = new frmRegister();
            IsEdit = true;
            int i = 0;
            try
            {
                Username = GridListUser.GetFocusedRowCellValue("Username").ToString();
                i = GridListUser.View.FocusedRowData.RowHandle.Value;
            }
            catch (Exception)
            {
                MessageBox.Show("Chọn 1 dòng để sửa không phải dòng này!");
            }
            frmRegister.Title = "Sửa thông tin người dùng";
            frmRegister.btnReg.Content = "Lưu";
            frmRegister.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmRegister.ShowDialog();
            LoadGrid();
            GridListUser.FocusRowHandle(i);
            IsEdit = false;
        }

        private void tsbDelete_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                string Username = GridListUser.GetFocusedRowCellValue("Username").ToString();
                BLEmployees.DeleteEmployee(Username);
            }
            catch (Exception)
            {
                MessageBox.Show("Chọn đúng dùm cái!");
            }
            LoadGrid();
            GridListUser.FocusRowHandle(GridListUser.ReturnVisibleRowCount - 1);
        }

        private void tsbAdd_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            frmRegister frmRegister = new frmRegister();
            frmRegister.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmRegister.ShowDialog();
            LoadGrid();
            GridListUser.FocusRowHandle(GridListUser.ReturnVisibleRowCount - 1);
        }

        private void GridListUser_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VisibleRowCount")
            {
                CheckMenuOthers();
            }
        }
        private void CheckMenuOthers()
        {
            tsbEdit.IsEnabled = GridListUser.VisibleRowCount > 0;
            tsbDelete.IsEnabled = GridListUser.VisibleRowCount > 0;
            tsbExportToExcel.IsEnabled = GridListUser.VisibleRowCount > 0;
        }

        private void mnsUserRight_Click(object sender, RoutedEventArgs e)
        {
            D00F2040 frmPermission = new D00F2040();
            string Username = GridListUser.GetFocusedRowCellValue(COL_Username).ToString();
            string Fullname = GridListUser.GetFocusedRowCellValue(COL_Fullname).ToString();
            int i = GridListUser.View.FocusedRowData.RowHandle.Value;
            frmPermission.txtUsername.Text = Username;
            frmPermission.txtFullname.Text = Fullname;
            frmPermission.lbLastLogin.Content = "Đăng nhập lần cuối: " + GridListUser.GetFocusedRowCellValue(COL_LastLogin).ToString();
            frmPermission.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmPermission.ShowDialog();
            LoadGrid();
            GridListUser.FocusRowHandle(i);
        }

        private void tsbListAll_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            LoadGrid();
        }

        private void tsbExportToExcel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GridListUser.ExportToXLS("DataEmployees.xls");
        }
        private void mnsUserRightReadOnly_Click(object sender, RoutedEventArgs e)
        {
            D00F2040 frmPermission = new D00F2040();
            string Username = GridListUser.GetFocusedRowCellValue(COL_Username).ToString();
            string Fullname = GridListUser.GetFocusedRowCellValue(COL_Fullname).ToString();
            int i = GridListUser.View.FocusedRowData.RowHandle.Value;
            frmPermission.txtUsername.Text = Username;
            frmPermission.txtFullname.Text = Fullname;
            frmPermission.lbLastLogin.Content = "Đăng nhập lần cuối: " + GridListUser.GetFocusedRowCellValue(COL_LastLogin).ToString();
            frmPermission.btnSave.IsEnabled = false;
            frmPermission.gbPermission.IsEnabled = false;
            frmPermission.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmPermission.ShowDialog();
            LoadGrid();
            GridListUser.FocusRowHandle(i);
        }
    }
}
