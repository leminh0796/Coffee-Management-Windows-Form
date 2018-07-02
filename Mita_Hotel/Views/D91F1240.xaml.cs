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
            gridListGeneral.SetDefaultFilterChangeGrid();
        }

        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("select * from D91T1240 WHERE ListTypeID = '"+ ListTypeID +"'");
            DataTable dt = L3SQLServer.ReturnDataTable(cmd.CommandText);
            gridListGeneral.ItemsSource = dt;
        }

        private void tsbAdd_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void tsbEdit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void tsbDelete_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                ListID = gridListGeneral.GetFocusedRowCellValue("ListID").ToString();
                SqlCommand cmd = new SqlCommand("DELETE D91T1240 where ListTypeID = '" + ListTypeID + "' and ListID = '"+ ListID +"'");
                L3SQLServer.ExecuteSQL(cmd.CommandText);
                LoadGrid();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Lỗi không thể xóa được bảng trống");
            }
        }

        private void tsbExportToExcel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            gridListGeneral.ExportToXLS("DataGeneral.xls");
        }

        private void tsbListAll_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void tsbSysInfo_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }
    }
}
