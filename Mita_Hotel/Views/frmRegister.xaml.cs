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
using Mita_Hotel.Setup;
using Mita_Hotel.BL;
using Mita_Hotel.Models;

namespace Mita_Hotel.Views
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

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            UserLogin user = new UserLogin();
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
            else if (txtRepassword.Password != txtPassword.Password || txtPassword.Password == "" || txtRepassword.Password == "")
            {
                lbPassword.Foreground = new SolidColorBrush(Colors.Red);
                txtPassword.BorderBrush = new SolidColorBrush(Colors.Red);
                lbRepassword.Foreground = new SolidColorBrush(Colors.Red);
                txtRepassword.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                bool checkUser = RegAcc.IfUsernameAlreadyExist(user.Username);
                MD5 md5Hash = MD5.Create();
                if (checkUser == true)
                {
                    lbUsername.Foreground = new SolidColorBrush(Colors.Red);
                    txtUsername.BorderBrush = new SolidColorBrush(Colors.Red);
                    lbWrong.Visibility = Visibility.Visible;
                }
                else
                {
                    string MD5Password = GetMd5Hash(md5Hash, txtPassword.Password);
                    user.MD5Password = MD5Password;
                    bool reg = RegAcc.IfReg(user.Username, user.MD5Password, user.Fullname, user.RoleID, user.Email, user.Phone);
                    MessageBox.Show("Đăng ký thành công!");
                    this.Close();
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
        }
    }
}
