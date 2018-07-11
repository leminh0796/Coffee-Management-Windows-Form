using Lemon3;
using Lemon3.Controls.DevExp;
using Lemon3.Data;
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
using System.Windows.Shapes;
using Mita_Hotel.Models;
using Mita_Hotel.BL;

namespace Mita_Hotel.Views
{
    /// <summary>
    /// Interaction logic for D05F2140.xaml
    /// </summary>
    public partial class D05F2140 : L3Window
    {
        public D05F2140()
        {
            InitializeComponent();
            txtItem.TextChanged += txtItem_TextChanged;
        }
        public string TableID { get; set; }
        public bool IsBooking = false;

        void txtItem_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataTable DtSuggest = L3SQLServer.ReturnDataTable("select InventoryID, InventoryName, BarCode from D91T1040");
            string typedString = txtItem.Text;
            DataTable AutoTable = DtSuggest.Clone();
            AutoTable.Clear();
            foreach (DataRow item in DtSuggest.Rows)
            {
                if (!string.IsNullOrEmpty(txtItem.Text))
                {
                    if (item["InventoryID"].ToString().Contains(typedString) || item["InventoryName"].ToString().Contains(typedString) || item["BarCode"].ToString().Contains(typedString))
                    {
                        AutoTable.ImportRow(item);
                    }
                }
            }

            if (AutoTable.Rows.Count > 0)
            {
                lbeSuggestion.ItemsSource = AutoTable;
                lbeSuggestion.DisplayMember = "InventoryName";
                lbeSuggestion.Visibility = Visibility.Visible;
            }
            else if (txtItem.Text.Equals(""))
            {
                lbeSuggestion.Visibility = Visibility.Collapsed;
                lbeSuggestion.ItemsSource = null;
            }
            else
            {
                lbeSuggestion.Visibility = Visibility.Collapsed;
                lbeSuggestion.ItemsSource = null;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void L3Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsBooking == true)
            {
                Voucher voucher = new Voucher();
                voucher.VoucherID = L3.UserID + DateTime.Now.ToString("yyyyMMddHHmmss");
                voucher.VoucherDate = DateTime.Now;
                voucher.UserID = L3.UserID;
                voucher.Status = 2;
                voucher.TableID = TableID;
                if (BLVoucher.AddVoucher(voucher))
                {
                    //////////////////////////////////////////TO DO////CẮT CU///////////////
                }
            }
        }

        private void lbeSuggestion_KeyDown(object sender, KeyEventArgs e)
        {
            if (ReferenceEquals(sender, lbeSuggestion))
            {
                if (e.Key == Key.Enter)
                {
                    txtItem.Text = "";
                    lbeSuggestion.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void txtItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                lbeSuggestion.Focus();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            L3SQLServer.ExecuteSQL("UPDATE D05T2010 SET TotalMoney = 0, Status = 0, People = 0, IsPaid = 0 WHERE TableID = " + L3SQLClient.SQLString(TableID) + "");
            Close();
        }
    }
}
