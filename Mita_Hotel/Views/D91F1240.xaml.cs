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
    /// Interaction logic for D91F1240.xaml
    /// </summary>
    public partial class D91F1240 : L3Page
    {
        //Khi lưu xuống D91T1240 thì ListTypeID lưu xuống cho cột ListTypeID
        public string ListTypeID { set; get; }
        public static string ListID;
        public static bool IsAdd = false;

        public D91F1240()
        {
            InitializeComponent();

        }

        public override void SetContentForL3Page()
        {
        }

        private void L3Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGrid();
            GridListGeneral.SetDefaultFilterChangeGrid();
            L3Control.SetShortcutPopupMenu(MainMenuControl2);
        }

        private void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("select * from D91T1240 WHERE ListTypeID = '"+ ListTypeID +"'");
            DataTable dt = L3SQLServer.ReturnDataTable(cmd.CommandText);
            GridListGeneral.ItemsSource = dt;
        }
        
        private void tsbAdd_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            D92F1240 Add = new D92F1240();
            IsAdd = true;
            switch(ListTypeID)
            {
                case "UnitID":
                    Add.Title = "Thêm đơn vị tính";
                    Add.lbHeader.Content = "Loại danh mục : ĐVT";
                    Add.txtblockListID.Text = "Mã đơn vị";
                    Add.lbListName.Content = "Tên đơn vị";
                    D92F1240.ListTypeID = "UnitID";
                    break;
                case "TT":
                    Add.Title = "Thêm tỉnh thành";
                    Add.lbHeader.Content = "Loại danh mục : Tỉnh thành";
                    Add.txtblockListID.Text = "Mã tỉnh";
                    Add.lbListName.Content = "Tên tỉnh";
                    D92F1240.ListTypeID = "TT";
                    break;
            }
            Add.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Add.ShowDialog();
            LoadGrid();
            GridListGeneral.FocusRowHandle(GridListGeneral.ReturnVisibleRowCount - 1);
        }

        private void tsbEdit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            D92F1240 Edit = new D92F1240();
            IsAdd = false;
            int i = 0;
            try
            {
                ListID = GridListGeneral.GetFocusedRowCellValue("ListID").ToString();
                i = GridListGeneral.View.FocusedRowData.RowHandle.Value;
            }
            catch (Exception)
            {
                MessageBox.Show("Chọn 1 dòng để sửa không phải dòng này!");
            }
            switch (ListTypeID)
            {
                case "UnitID":
                    Edit.Title = "Sửa đơn vị tính";
                    Edit.lbHeader.Content = "Loại danh mục : ĐVT";
                    Edit.txtblockListID.Text = "Mã đơn vị";
                    Edit.lbListName.Content = "Tên đơn vị";
                    D92F1240.ListTypeID = "UnitID";
                    break;
                case "TT":
                    Edit.Title = "Sửa tỉnh thành";
                    Edit.lbHeader.Content = "Loại danh mục : Tỉnh thành";
                    Edit.txtblockListID.Text = "Mã tỉnh";
                    Edit.lbListName.Content = "Tên tỉnh";
                    D92F1240.ListTypeID = "TT";
                    break;
            }
            Edit.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Edit.ShowDialog();
            LoadGrid();
            GridListGeneral.FocusRowHandle(i);
        }

        private void tsbDelete_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                ListID = GridListGeneral.GetFocusedRowCellValue("ListID").ToString();
                SqlCommand cmd = new SqlCommand("DELETE D91T1240 where ListTypeID = '" + ListTypeID + "' and ListID = '"+ ListID +"'");
                L3SQLServer.ExecuteSQL(cmd.CommandText);
                LoadGrid();
                GridListGeneral.FocusRowHandle(GridListGeneral.ReturnVisibleRowCount - 1);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Lỗi không thể xóa được bảng trống");
            }
        }

        private void tsbExportToExcel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GridListGeneral.ExportToXLS("DataGeneral.xls");
        }

        private void tsbListAll_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GridListGeneral.ListAll();
        }

        private void tsbSysInfo_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            L3Window.ShowSysInforForm(GridListGeneral);
        }

        private void gridListGeneral_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VisibleRowCount")
            {
                CheckMenuOthers();
            }
        }
        private void CheckMenuOthers()
        {
            tsbEdit.IsEnabled = GridListGeneral.VisibleRowCount > 0;
            tsbDelete.IsEnabled = GridListGeneral.VisibleRowCount > 0;
            tsbExportToExcel.IsEnabled = GridListGeneral.VisibleRowCount > 0;
        }
    }
}
