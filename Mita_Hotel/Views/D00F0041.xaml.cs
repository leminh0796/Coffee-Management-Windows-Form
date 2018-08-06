using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Lemon3;
using Lemon3.Data;
using Mita_Coffee.Setup;
using Mita_Coffee.BL;
using Mita_Coffee.Models;

namespace Mita_Coffee.Views
{
    /// <summary>
    /// Interaction logic for frmRegister.xaml
    /// </summary>
    public partial class frmRegister : Window
    {
        public frmRegister()
        {
            InitializeComponent();
        }
        UserLogin user = new UserLogin();
        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            user.Username = txtUsername.Text;
            user.Fullname = txtFullname.Text;
            user.RoleID = lkeRole.EditValue.ToString();
            user.Email = txtEmail.Text;
            if (sePhone.Text != "")
                user.Phone = Int32.Parse(sePhone.Text);
            else user.Phone = 0;

            if (user.Username == "" || user.Username.Length > 20)
            {
                lbUsername.Foreground = new SolidColorBrush(Colors.Red);
                txtUsername.BorderBrush = new SolidColorBrush(Colors.Red);
                lbWrong1.Visibility = Visibility.Visible;
            }
            else if ( ( txtRepassword.Password != txtPassword.Password || txtPassword.Password == "" || txtRepassword.Password == "") && !D00F0040.IsEdit)
            {
                lbPassword.Foreground = new SolidColorBrush(Colors.Red);
                txtPassword.BorderBrush = new SolidColorBrush(Colors.Red);
                lbRepassword.Foreground = new SolidColorBrush(Colors.Red);
                txtRepassword.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                bool checkUser = BLRegistration.IfUsernameAlreadyExist(user.Username);
                MD5 md5Hash = MD5.Create();
                if (!D00F0040.IsEdit)
                {
                    if (checkUser)
                    {
                        lbUsername.Foreground = new SolidColorBrush(Colors.Red);
                        txtUsername.BorderBrush = new SolidColorBrush(Colors.Red);
                        lbWrong.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        string MD5Password = GetMd5Hash(md5Hash, txtPassword.Password);
                        user.MD5Password = MD5Password;
                        bool reg = BLRegistration.IfReg(user.Username, user.MD5Password, user.Fullname, user.RoleID, user.Email, user.Phone);
                        MessageBox.Show("Đăng ký thành công!");
                        this.Close();
                    }
                }
                else
                {
                    if (txtPassword.Password == txtRepassword.Password && txtPassword.Password != "")
                    {
                        string MD5Password = GetMd5Hash(md5Hash, txtPassword.Password);
                        user.MD5Password = MD5Password;
                    }
                    if (txtRepassword.Password != txtPassword.Password)
                    {
                        lbPassword.Foreground = new SolidColorBrush(Colors.Red);
                        txtPassword.BorderBrush = new SolidColorBrush(Colors.Red);
                        lbRepassword.Foreground = new SolidColorBrush(Colors.Red);
                        txtRepassword.BorderBrush = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        bool DoEdit = EditUser();
                        if (DoEdit)
                        {
                            MessageBox.Show("Sửa thành công!");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Sửa thất bại!");
                        }
                    }
                    
                }
            }
            
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private void txtUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            txtUsername.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#7F5C5C5C"));
            lbUsername.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF000000"));
            lbWrong.Visibility = Visibility.Hidden;
            lbWrong1.Visibility = Visibility.Hidden;
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPassword.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#7F5C5C5C"));
            lbPassword.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF000000"));
            txtRepassword.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#7F5C5C5C"));
            lbRepassword.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF000000"));
        }

        private void lkeRole_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = L3SQLServer.ReturnDataTable("select * FROM tblRole");
            lkeRole.ItemsSource = dt;
            lkeRole.ValueMember = "RoleID";
            lkeRole.DisplayMember = "Role";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sePhone.InputNumber288("n0", false, false);
            if (D00F0040.IsEdit)
            {
                DataTable dt = L3SQLServer.ReturnDataTable("select FullName, Username, Email, Phone, RoleID, MD5Password from D00T0040 WHERE Username = '" + D00F0040.Username + "'");
                txtEmail.Text = dt.Rows[0]["Email"].ToString();
                txtFullname.Text = dt.Rows[0]["FullName"].ToString();
                txtUsername.IsReadOnly = true;
                txtUsername.Text = dt.Rows[0]["Username"].ToString();
                try
                {
                    sePhone.Value = System.Convert.ToDecimal(dt.Rows[0]["Phone"]);
                }
                catch (Exception)
                {
                    sePhone.Value = 0;
                }
                lkeRole.EditValue = dt.Rows[0]["RoleID"];
                user.MD5Password = dt.Rows[0]["MD5Password"].ToString();
            }
        }
        public bool EditUser()
        {
            return L3SQLServer.ExecuteNoneQuery("sp_EditUser",
               CommandType.StoredProcedure,
               new string[] { "Username", "FullName", "MD5Password", "RoleID", "Email", "Phone" }
               , new object[] { user.Username , user.Fullname, user.MD5Password, user.RoleID, user.Email, user.Phone });
        }
    }
}
