using Lemon3.Controls.DevExp;
using Lemon3.Data;
using System;
using System.Collections.Generic;
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
using System.Data;
using Lemon3;

namespace Mita_Coffee.Views
{
    /// <summary>
    /// Interaction logic for D91F2141.xaml
    /// </summary>
    public partial class D91F2141 : L3Window
    {
        public D91F2141()
        {
            InitializeComponent();
        }

        private void L3Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPBLDetailGrid();
            GridPBLDetail.InputNumber288("n0", false, false, COL_Amount);
            GridPBLDetail.InputNumber288("n0", false, false, COL_Price);
            GridPBLDetail.InputNumber288("n0", false, false, COL_VAT);
        }
        public static string VoucherID = "";
        private void LoadPBLDetailGrid()
        {
            SqlConnection conn = new SqlConnection(L3.ConnectionString);
            SqlCommand cmd = new SqlCommand("D91P2140", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@VoucherID", VoucherID));
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            GridPBLDetail.ItemsSource = dt;
        }
    }
}
