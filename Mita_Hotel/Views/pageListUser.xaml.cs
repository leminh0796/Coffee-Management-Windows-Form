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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Mita_Hotel.Views
{
    /// <summary>
    /// Interaction logic for pageListUser.xaml
    /// </summary>
    public partial class pageListUser : L3Page
    {
        public pageListUser()
        {
            InitializeComponent();
        }

        private void L3Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGrid();
            gridListUser.SetDefaultFilterChangeGrid();
            L3Control.SetShortcutPopupMenu(MainMenuControl1);
        }
        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("select ID, Fullname, Username, Role, Email, Phone, LastLogin, FirstDate from D00T0040 left join tblRole on D00T0040.RoleID = tblRole.RoleID");
            DataTable dt = L3SQLServer.ReturnDataTable(cmd.CommandText);
            gridListUser.ItemsSource = dt;
        }
        public override void SetContentForL3Page()
        {
        }

        public static bool IsEdit = false;
        public static string Username = "";
        private void tsbEdit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            frmRegister frmRegister = new frmRegister();
            IsEdit = true;
            try
            {
                Username = gridListUser.GetFocusedRowCellValue("Username").ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Chọn 1 dòng để sửa không phải dòng này!");
            }
            frmRegister.Title = "Sửa thông tin người dùng";
            frmRegister.btnReg.Content = "Lưu";
            frmRegister.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            frmRegister.ShowDialog();
            LoadGrid();
            IsEdit = false;
        }

        private void tsbDelete_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                string Username = gridListUser.GetFocusedRowCellValue("Username").ToString();
                SqlCommand cmd = new SqlCommand("DELETE D00T0040 where Username = '" + Username + "'");
                L3SQLServer.ExecuteSQL(cmd.CommandText);
            }
            catch (Exception)
            {
                MessageBox.Show("Chọn đúng dùm cái!");
            }
            LoadGrid();
            gridListUser.FocusRowHandle(gridListUser.ReturnVisibleRowCount - 1);
        }

        private void tsbPrint_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void tsbAdd_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            frmRegister frmRegister = new frmRegister();
            frmRegister.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            frmRegister.ShowDialog();
            LoadGrid();
            gridListUser.FocusRowHandle(gridListUser.ReturnVisibleRowCount - 1);
        }

        private void gridListUser_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VisibleRowCount")
            {
                CheckMenuOthers();
            }
        }
        private void CheckMenuOthers()
        {
            tsbEdit.IsEnabled = gridListUser.VisibleRowCount > 0;
            tsbDelete.IsEnabled = gridListUser.VisibleRowCount > 0;
            tsbExportToExcel.IsEnabled = gridListUser.VisibleRowCount > 0;
        }
    }
}
