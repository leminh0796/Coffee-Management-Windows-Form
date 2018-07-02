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

namespace Mita_Hotel.Views
{
    /// <summary>
    /// Interaction logic for D92F1240.xaml
    /// </summary>
    public partial class D92F1240 : L3Window
    {
        public D92F1240()
        {
            InitializeComponent();
        }

        ListGeneral li = new ListGeneral();
        public static string ListTypeID = "";

        public bool AddNew()
        {
            return L3SQLServer.ExecuteNoneQuery("sp_AddGeneral",
                   CommandType.StoredProcedure,
                   new string[] { "ListTypeID", "ListID", "ListName", "Description" }
                   , new object[] { li.ListTypeID, li.ListID, li.ListName, li.Description });
        }

        public bool EditNew()
        {
            return L3SQLServer.ExecuteNoneQuery("sp_EditGeneral",
                   CommandType.StoredProcedure,
                   new string[] { "ListTypeID", "ListID", "ListName", "Description" }
                   , new object[] { li.ListTypeID, li.ListID, li.ListName, li.Description });
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            li.ListID = txtListID.Text;
            li.ListName = txtListName.Text;
            li.Description = txtDescription.Text;
            switch(D91F1240.IsAdd)
            {
                case true:
                    if (txtListID.Text != "")
                    {
                        bool DoAdd = AddNew();
                        if (DoAdd)
                        {
                            MessageBox.Show("Thêm thành công!");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Thêm thất bại!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không được để trống!");
                    }
                    break;

                case false:
                    bool DoEdit = EditNew();
                    if (DoEdit)
                    {
                        MessageBox.Show("Sửa thành công!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Sửa thất bại!");
                    }
                    break;
            }
        }

        private void L3Window_Loaded(object sender, RoutedEventArgs e)
        {
            li.ListTypeID = ListTypeID;
            if (D91F1240.IsAdd == false)
            {
                DataTable dt = L3SQLServer.ReturnDataTable("select * from D91T1240 WHERE ListTypeID = '"+ li.ListTypeID +"' and ListID = '"+ D91F1240.ListID +"'");
                txtListID.Text = dt.Rows[0]["ListID"].ToString();
                txtListID.IsReadOnly = true;
                txtListName.Text = dt.Rows[0]["ListName"].ToString();
                txtDescription.Text = dt.Rows[0]["Description"].ToString();
            }
        }
    }

}
