using System;
using System.Collections.Generic;
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
using System.Security.Cryptography;
using Lemon3.Controls.DevExp;
using Mita_Hotel.Setup;
using Lemon3;
using System.Data.SqlClient;
using System.Timers;
using System.Threading;
using Mita_Hotel.BL;
using Mita_Hotel.Models;

namespace Mita_Hotel.Views
{
    /// <summary>
    /// Interaction logic for frmLogin.xaml
    /// </summary>
    public partial class frmLogin : L3Window
    {
        public frmLogin()
        {
            InitializeComponent();
            LoginSuccess = false;
        }
        public bool LoginSuccess { get; private set; }
        UserLogin user = new UserLogin();
        public int LoginCount = 0;
        public static string UserLogged = "";
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            MD5 md5Hash = MD5.Create();
            if (Properties.Settings.Default.checkerAuto == false)
            {
                string MD5Password = GetMd5Hash(md5Hash, txtPassword.Password);
                user.MD5Password = MD5Password;
                user.Username = txtUsername.Text;
            }
            else
            {
                user.MD5Password = Properties.Settings.Default.rePassword;
                user.Username = Properties.Settings.Default.reUsername;
            }
            bool valid = BLLogin.IfLogin(user.Username, user.MD5Password);
            bool Uvalid = BLLogin.IfUsernameValid(user.Username);
            if (LoginCount < 4)
            {
                if (valid)
                {
                    LoginSuccess = true;
                    L3.UserID = user.Username;
                    this.Close();
                }
                else if (Uvalid)
                {
                    lbWrong.Visibility = Visibility.Hidden;
                    LoginSuccess = false;
                    lbPassword.Foreground = new SolidColorBrush(Colors.Red);
                    txtPassword.BorderBrush = new SolidColorBrush(Colors.Red);
                    lbWrong1.Visibility = Visibility.Visible;
                    LoginCount++;
                }
                else
                {
                    lbWrong1.Visibility = Visibility.Hidden;
                    LoginSuccess = false;
                    lbUsername.Foreground = new SolidColorBrush(Colors.Red);
                    lbWrong.Visibility = Visibility.Visible;
                    txtUsername.BorderBrush = new SolidColorBrush(Colors.Red);
                    LoginCount++;
                }
            }
            else
            {
                LoginCount = 0;
                MessageBox.Show("Bạn đã nhập sai quá số lần quy định.\n Màn hình đăng nhập sẽ bị khóa trong 5 phút!");
                btnLogin.IsEnabled = false;
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 300000;
                timer.Elapsed += new ElapsedEventHandler(EnableBtn);
                timer.Enabled = true;
            }
            if (ceRememberLogin.IsChecked == true && ceAutoLogin.IsChecked == false)
            {
                Properties.Settings.Default.checkerUser = true;
                Properties.Settings.Default.checkerAuto = false;
                Properties.Settings.Default.reUsername = user.Username;
                Properties.Settings.Default.Save();
            }
            else if (ceRememberLogin.IsChecked == false || ceAutoLogin.IsChecked == false)
            {
                Properties.Settings.Default.checkerUser = false;
                Properties.Settings.Default.checkerAuto = false;
                Properties.Settings.Default.reUsername = "";
                Properties.Settings.Default.rePassword = "";
                Properties.Settings.Default.Save();
            }
            else if (ceAutoLogin.IsChecked == true)
            {
                Properties.Settings.Default.checkerAuto = true;
                Properties.Settings.Default.reUsername = user.Username;
                Properties.Settings.Default.rePassword = user.MD5Password;
                Properties.Settings.Default.Save();
            }
        }
        private void EnableBtn(object source, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(
                new ThreadStart(() => btnLogin.IsEnabled = true));
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bool isConnected = false;
            using (SqlConnection connection = new SqlConnection(L3.ConnectionString))
            {
                try
                {
                    connection.Open();
                    isConnected = true;
                }
                catch (SqlException)
                {
                    isConnected = false;
                }
            }
            if (isConnected == false)
            {
                btnLogin.Content = "Not connected!";
                btnLogin.Foreground = new SolidColorBrush(Colors.Red);
                btnLogin.IsEnabled = false;
                lbWrong2.Visibility = Visibility.Visible;
            }
            if (Properties.Settings.Default.checkerUser == true)
            {
                ceRememberLogin.IsChecked = true;
                txtUsername.Text = Properties.Settings.Default.reUsername;
            }
            if (Properties.Settings.Default.checkerAuto == true)
            {
                LoginSuccess = true;
                L3.UserID = Properties.Settings.Default.reUsername;
                this.Close();
            }
        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void miHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Vui lòng liên hệ bộ phận kỹ thuật: technical@mitahotel.com");
        }

        private void txtUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            txtUsername.BorderBrush = new SolidColorBrush();
            txtUsername.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#7F5C5C5C"));
            lbUsername.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF3C3C3C"));
            lbWrong.Visibility = Visibility.Hidden;
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPassword.BorderBrush = new SolidColorBrush();
            txtPassword.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#7F5C5C5C"));
            lbPassword.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF3C3C3C"));
            lbWrong1.Visibility = Visibility.Hidden;
        }

        private void miClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

    }
}
