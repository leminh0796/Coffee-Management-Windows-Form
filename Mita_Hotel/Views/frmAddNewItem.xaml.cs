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
                MessageBox.Show("Không được để trống đơn vị tính!", "Lỗi");
            }
            item.Price = sePrice.Value;
            item.Notes = txtNotes.Text;
            item.VAT = seVAT.Value;
            item.BarCode = txtBarcode.Text;
            switch (PageListItem.IsAddItem)
            {
                case true:
                    if (txtInventoryName.Text != "")
                    {
                        bool add = AddNewItem2();
                        if (add)
                        {
                            MessageBox.Show("Thêm mới thành công!", "Yeah!");

                            this.Close();
                        }
                        else MessageBox.Show("Thêm mới thất bại!", "Lỗi");
                    }
                    else MessageBox.Show("Không được để trống tên hàng hóa!", "Lỗi");
                    break;

                case false:
                    if (txtInventoryName.Text != "")
                    {
                        bool edit = EditItem();
                        if (edit)
                        {
                            MessageBox.Show("Sửa thành công!", "Yeah!");

                            this.Close();
                        }
                        else MessageBox.Show("Sửa thất bại!", "Lỗi");
                    }
                    else MessageBox.Show("Không được để trống tên hàng hóa!", "Lỗi");
                    break;
            }
        
        }
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt1 = L3SQLServer.ReturnDataTable("select * FROM D91T1240 WHERE ListTypeID = 'UnitID'");
            lkeUnitID.ItemsSource = dt1;
            sePrice.InputNumber288("n0", false, false);
            seVAT.InputNumber288("n0", false, false);
            if (PageListItem.IsAddItem == false)
            {
                DataTable dt = L3SQLServer.ReturnDataTable("select InventoryName, UnitID, Price, Notes, VAT, BarCode, ImageLocation from D91T1040 WHERE InventoryID = '" + PageListItem.InventoryID + "'");
                txtInventoryName.Text = dt.Rows[0]["InventoryName"].ToString();
                lkeUnitID.EditValue = dt.Rows[0]["UnitID"];
                sePrice.Value = System.Convert.ToDecimal(dt.Rows[0]["Price"]);
                txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                seVAT.Value = System.Convert.ToDecimal(dt.Rows[0]["VAT"]);
                txtBarcode.Text = dt.Rows[0]["BarCode"].ToString();
                var uriSource = new Uri(@"/Mita_Hotel;component/Images/"+ dt.Rows[0]["ImageLocation"], UriKind.Relative);
                ieImage.Source = new BitmapImage(uriSource);
            }
        }

        public bool AddNewItem2()
        {
            return L3SQLServer.ExecuteNoneQuery("sp_AddItem",
               CommandType.StoredProcedure,
               new string[] { "InventoryName", "UnitID", "Price", "Notes", "VAT", "BarCode", "CreateUserID", "ImageLocation" }
               , new object[] { item.InventoryName, item.UnitID, item.Price, item.Notes, item.VAT, item.BarCode, L3.UserID, item.ImageLocation });
        }
        public bool EditItem()
        {
            return L3SQLServer.ExecuteNoneQuery("sp_EditItem",
               CommandType.StoredProcedure,
               new string[] { "InventoryID", "InventoryName", "UnitID", "Price", "Notes", "VAT", "BarCode", "LastModifyUserID" }
               , new object[] { PageListItem.InventoryID, item.InventoryName, item.UnitID, item.Price, item.Notes, item.VAT, item.BarCode, L3.UserID });
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog img = new Microsoft.Win32.OpenFileDialog();
            img.DefaultExt = ".jpeg;.png;.jpg";
            img.Filter = "Image files(*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            Nullable<bool> show = img.ShowDialog();
            if (show == true)
            {
                item.ImageLocation = img.SafeFileName;
                var uriSource = new Uri(@"/Mita_Hotel;component/Images/" + item.ImageLocation, UriKind.Relative);
                ieImage.Source = new BitmapImage(uriSource);
            }
        }
    }
}
