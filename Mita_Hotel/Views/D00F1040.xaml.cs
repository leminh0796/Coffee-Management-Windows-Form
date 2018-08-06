using Lemon3.Controls.DevExp;
using Lemon3.Data;
using Mita_Coffee.BL;
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

namespace Mita_Coffee.Views
{
    /// <summary>
    /// Interaction logic for pageListItem.xaml
    /// </summary>
    public partial class D00F1040 : L3Page
    {
        public D00F1040()
        {
            InitializeComponent();
        }
        public static bool IsAddItem = false;
        public static string InventoryID = "";
        public override void SetContentForL3Page()
        {
        }

        private void L3Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadItemGrid();

            GridListItem.InputNumber288("n0", false, false, COL_Price);
            GridListItem.InputNumber288("n0", false, false, COL_InStock);
            GridListItem.InputPercent(false, false, 28, 8, COL_VAT);
            GridListItem.SetDefaultFilterChangeGrid();

            L3Control.SetShortcutPopupMenu(MainMenuControl1);
        }
        public void LoadItemGrid()
        {
            DataTable dt = BLInventory.LoadItem();
            GridListItem.ItemsSource = dt;
        }


        private void tsbAdd_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            D00F1041 frm = new D00F1041();
            IsAddItem = true;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.ShowDialog();
            LoadItemGrid();
            GridListItem.FocusRowHandle(GridListItem.ReturnVisibleRowCount - 1);
        }

        private void tsbEdit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            D00F1041 frm = new D00F1041();
            frm.Title = "Chỉnh sửa thông tin hàng hóa";
            int i = 0;
            try
            {
                InventoryID = GridListItem.GetFocusedRowCellValue("InventoryID").ToString();
                i = GridListItem.View.FocusedRowData.RowHandle.Value;
            }
            catch (Exception)
            {
                MessageBox.Show("Chọn 1 dòng để sửa không phải dòng này!");
            }
            IsAddItem = false;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.ShowDialog();
            LoadItemGrid();
            GridListItem.FocusRowHandle(i);
        }

        private void tsbDelete_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                InventoryID = GridListItem.GetFocusedRowCellValue("InventoryID").ToString();
                BLInventory.DeleteInventory(InventoryID);
                LoadItemGrid();
                GridListItem.FocusRowHandle(GridListItem.ReturnVisibleRowCount - 1);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Lỗi không thể xóa được bảng trống");
            }
        }
        private void CheckMenuOthers()
        {
            tsbEdit.IsEnabled = GridListItem.VisibleRowCount > 0;
            tsbDelete.IsEnabled = GridListItem.VisibleRowCount > 0;
            tsbExportToExcel.IsEnabled = GridListItem.VisibleRowCount > 0;
        }
        private void tsbExportToExcel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GridListItem.ExportToXLS("Data.xls");
        }

        private void tsbListAll_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GridListItem.ListAll();
        }

        private void tsbSysInfo_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            L3Window.ShowSysInforForm(GridListItem);
        }

        private void GridListItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VisibleRowCount")
            {
                CheckMenuOthers();
            }
        }
    }
}
