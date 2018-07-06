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
            BindingSource bSource = new BindingSource();
            bSource.DataSource = dt;
            GridTable.ItemsSource = bSource;
            dt.RowChanged += (o, arg) =>
            {
                da.Update(dt);
            };
            lkesStatus.ItemsSource = L3SQLServer.ReturnDataTable("select Status, StatusName from D05T2011");
        }

        private void mnsAdd_Click(object sender, RoutedEventArgs e)
        {
            D05F2140 frmTable = new D05F2140();
            frmTable.lbTableName.Content = "Tên bàn: " + GridTable.GetFocusedRowCellValue("TableName").ToString();
            frmTable.sePeople.Text = GridTable.GetFocusedRowCellValue("People").ToString();
            frmTable.lkeStatus.EditValue = GridTable.GetFocusedRowCellValue("Status");
            frmTable.lkeStatus.ItemsSource = L3SQLServer.ReturnDataTable("select Status, StatusName from D05T2011");
            frmTable.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmTable.ShowDialog();
        }
    }
}
