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
using Mita_Hotel.Setup;
using System.Data.SqlClient;
using Lemon3;
using Lemon3.Controls.DevExp;
using Mita_Hotel.Models;

namespace Mita_Hotel.Views
{
    /// <summary>
    /// Interaction logic for frmAddNewItem.xaml
    /// </summary>
    public partial class frmAddNewItem : L3Window
    {
        public frmAddNewItem()
        {
            InitializeComponent();
        }

        InventoryItem item = new InventoryItem();
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            item.InventoryName = txtInventoryName.Text;
            try
            {
                item.UnitID = lkeUnitID.EditValue.ToString();
            }
            catch (NullReferenceException)
            {
                item.UnitID = "";
                MessageBox.Show("Không được để trống đơn vị tính!");
            }
            item.Price = sePrice.Value;
            item.Notes = txtNotes.Text;
            item.VAT = seVAT.Value;
            item.BarCode = txtBarcode.Text;
            switch (pageListItem.IsAddItem)
            {
                case true:
                    if (txtInventoryName.Text != "")
                    {
                        bool add = AddNewItem2();
                        if (add)
                        {
                            MessageBox.Show("Thêm mới thành công!");

                            this.Close();
                        }
                        else MessageBox.Show("Thêm mới thất bại!");
                    }
                    else MessageBox.Show("Không được để trống tên hàng hóa!");
                    break;

                case false:
                    if (txtInventoryName.Text != "")
                    {
                        bool edit = EditItem();
                        if (edit)
                        {
                            MessageBox.Show("Sửa thành công!");

                            this.Close();
                        }
                        else MessageBox.Show("Sửa thất bại!");
                    }
                    else MessageBox.Show("Không được để trống tên hàng hóa!");
                    break;
            }
        
        }
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt1 = L3SQLServer.ReturnDataTable("select * FROM D91T1240 WHERE ListTypeID = 'UnitID'");
            lkeUnitID.ItemsSource = dt1;
            sePrice.InputNumber288("n0", false, false);
            seVAT.InputNumber288("n0", false, false);
            if (pageListItem.IsAddItem == false)
            {
                DataTable dt = L3SQLServer.ReturnDataTable("select InventoryName, UnitID, Price, Notes, VAT, BarCode from D91T1040 WHERE InventoryID = '" + pageListItem.InventoryID + "'");
                txtInventoryName.Text = dt.Rows[0]["InventoryName"].ToString();
                lkeUnitID.EditValue = dt.Rows[0]["UnitID"];
                sePrice.Value = System.Convert.ToDecimal(dt.Rows[0]["Price"]);
                txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                seVAT.Value = System.Convert.ToDecimal(dt.Rows[0]["VAT"]);
                txtBarcode.Text = dt.Rows[0]["BarCode"].ToString();
            }
        }

        public bool AddNewItem2()
        {
            return L3SQLServer.ExecuteNoneQuery("sp_AddItem",
               CommandType.StoredProcedure,
               new string[] { "InventoryName", "UnitID", "Price", "Notes", "VAT", "BarCode", "CreateUserID" }
               , new object[] { item.InventoryName, item.UnitID, item.Price, item.Notes, item.VAT, item.BarCode, L3.UserID });
        }
        public bool EditItem()
        {
            return L3SQLServer.ExecuteNoneQuery("sp_EditItem",
               CommandType.StoredProcedure,
               new string[] { "InventoryID", "InventoryName", "UnitID", "Price", "Notes", "VAT", "BarCode", "LastModifyUserID" }
               , new object[] { pageListItem.InventoryID, item.InventoryName, item.UnitID, item.Price, item.Notes, item.VAT, item.BarCode, L3.UserID });
        }
    }
}
