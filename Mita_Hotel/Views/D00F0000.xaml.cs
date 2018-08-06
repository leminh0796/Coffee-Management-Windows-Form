using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Security.Cryptography;
using Lemon3.Controls.DevExp;
using Mita_Coffee.Setup;
using Lemon3;
using System.Data.SqlClient;
using System.Timers;
using System.Threading;
using Mita_Coffee.Models;
using System.Data;

namespace Mita_Coffee.Views
{
    /// <summary>
    /// Interaction logic for frmLogin.xaml
    /// </summary>
    public partial class D00F0000 : L3Window
    {
        public D00F0000()   //The Login Form
        {
            InitializeComponent();
            LoginSuccess = false;
        }

        public bool LoginSuccess { get; private set; }                              //This let you pass the login form.
        UserLogin user = new UserLogin();                                           //Mita_Hotel.Models
        public int LoginCount = 0;                                                  //This will count when you try to login. (Hit the Login button -> this + 1).
        public static string UserLogged = "";                                       //For tracking the User who logged.

        private void btnLogin_Click(object sender, RoutedEventArgs e)               //When you hit the Login button....
        {
            MD5 md5Hash = MD5.Create();                                             //Initial MD5 hash code for encrypt the password.

            if (Properties.Settings.Default.checkerAuto == false)                   //Check if the "Remember auto login" is unchecked last session.
            {
                string MD5Password = GetMd5Hash(md5Hash, txtPassword.Password);     //Hash the password is typed by User to MD5Password.
                user.MD5Password = MD5Password;                                     //This MD5Password will be saved to Database.
                user.Username = txtUsername.Text;
            }
            else
            {
                user.MD5Password = Properties.Settings.Default.rePassword;          //"Remember auto login" is checked so it loads MD5Password from Properties.Settings
                user.Username = Properties.Settings.Default.reUsername;             //Same as Username.
            }

            bool valid = BLLogin.TryToLogin(user.Username, user.MD5Password);       //Compare the username & hashed password to Database.  
            bool IsUsernameValid = BLLogin.IfUsernameValid(user.Username);          //Check if the username exist.

            if (LoginCount < 4)
            {
                if (valid)                                                          //If Username & Hashed Password valid -> Access to main window.
                {
                    LoginSuccess = true;
                    L3.UserID = user.Username;
                    Close();
                }
                else if (IsUsernameValid)                                           //Else check the Username only. If the Username is correct, highlight the Wrong Password notification.
                {
                    lbWrongUsernamePassword.Visibility = Visibility.Hidden;
                    LoginSuccess = false;
                    lbPassword.Foreground = new SolidColorBrush(Colors.Red);
                    txtPassword.BorderBrush = new SolidColorBrush(Colors.Red);
                    lbWrongPassword.Visibility = Visibility.Visible;
                    LoginCount++;
                }
                else                                                                //When both Username & Password are wrong. Show the Wrong notification.
                {
                    lbWrongPassword.Visibility = Visibility.Hidden;
                    LoginSuccess = false;
                    lbUsername.Foreground = new SolidColorBrush(Colors.Red);
                    lbWrongUsernamePassword.Visibility = Visibility.Visible;
                    txtUsername.BorderBrush = new SolidColorBrush(Colors.Red);
                    LoginCount++;
                }
            }
            else                                                                   //STOP IT YOUR ACCESS IS DENIED. YOU'VE TYPED INVALID INFORMATION FOR 5 TIMES.
            {
                LoginCount = 0;
                MessageBox.Show("Bạn đã nhập sai quá số lần quy định.\n Màn hình đăng nhập sẽ bị khóa trong 5 phút!");
                btnLogin.IsEnabled = false;                             //Disable Login button.
                System.Timers.Timer timer = new System.Timers.Timer();  //Just call the timer to delay 5 minutes.
                timer.Interval = 300000;                                //Bad security. Prevent brute-force temporarily =]]z
                timer.Elapsed += new ElapsedEventHandler(EnableBtn);
                timer.Enabled = true;
            }
            /////////////////////The section below do saving if "Remember login information" or "Remember auto login" is checked.
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
            ////////////////////End of saving section.
        }

        private void EnableBtn(object source, ElapsedEventArgs e)   ///The timer elapsed. Show the Login button again.
        {
            Dispatcher.BeginInvoke(
                new ThreadStart(() => btnLogin.IsEnabled = true));
        }

        static string GetMd5Hash(MD5 md5Hash, string input)     //Function for getting md5 hashed string from original string.
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) //When window loaded..
        {
            bool isConnected = false;
            using (SqlConnection connection = new SqlConnection(L3.ConnectionString))   //Check the connection of Database.
            {
                try
                {
                    connection.Open();
                    isConnected = true;
                    connection.Close();
                }
                catch (SqlException)
                {
                    isConnected = false;
                }
            }
            if (isConnected == false)   //If the connection can't be established.
            {
                btnLogin.Content = "Không thể kết nối!";
                btnLogin.Foreground = new SolidColorBrush(Colors.Red);
                btnLogin.IsEnabled = false;
                lbWrong2.Visibility = Visibility.Visible;
            }
            if (Properties.Settings.Default.checkerUser == true)    //Load username if "Remember the username" was checked.
            {
                ceRememberLogin.IsChecked = true;
                txtUsername.Text = Properties.Settings.Default.reUsername;
            }
            if (Properties.Settings.Default.checkerAuto == true)    //Access to Main Windows with Username get from last session.
            {                                                       //When "Remember auto login" was checked.
                LoginSuccess = true;
                L3.UserID = Properties.Settings.Default.reUsername;
                Close();
            }
        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void miHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Vui lòng liên hệ bộ phận kỹ thuật: leminh0796@gmail.com");
        }

        private void txtUsername_GotFocus(object sender, RoutedEventArgs e) //Highlight the Username text box.
        {
            txtUsername.BorderBrush = new SolidColorBrush();
            txtUsername.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#7F5C5C5C"));
            lbUsername.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF3C3C3C"));
            lbWrongUsernamePassword.Visibility = Visibility.Hidden;
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e) //Highlight the Password text box.
        {
            txtPassword.BorderBrush = new SolidColorBrush();
            txtPassword.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#7F5C5C5C"));
            lbPassword.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF3C3C3C"));
            lbWrongPassword.Visibility = Visibility.Hidden;
        }

        private void miClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void miDBChange_Click(object sender, RoutedEventArgs e) //Open Database Changer window.
        {
            D00F0001 FormDB = new D00F0001();
            FormDB.txtServer.Text = L3.Server;
            FormDB.txtLogin.Text = L3.ConnectionUser;
            FormDB.txtPassword.Password = L3.Password;
            FormDB.txtDB.Text = L3.CompanyID;
            FormDB.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            FormDB.ShowDialog();
            bool isConnected = false;
            L3.ConnectionString = "Data Source=" + L3.Server + ";Initial Catalog=" + L3.CompanyID + ";User ID=" + L3.ConnectionUser + ";Password=" + L3.Password + ";Connect Timeout = 5";
            using (SqlConnection connection = new SqlConnection(L3.ConnectionString))   //Check the Connection of Database.
            {
                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        isConnected = true;
                    }
                    else
                    {
                        isConnected = false;
                    }
                    connection.Close();
                }
                catch (SqlException)
                {
                    isConnected = false;
                }
                
            }
            if (isConnected == false)   //When the Connection can't be established.
            {
                btnLogin.Content = "Không thể kết nối!";
                btnLogin.Foreground = new SolidColorBrush(Colors.Red);
                btnLogin.IsEnabled = false;
                lbWrong2.Visibility = Visibility.Visible;
            }
            else
            {
                btnLogin.Content = "Đăng nhập";
                btnLogin.Foreground = new SolidColorBrush(Colors.Green);
                btnLogin.IsEnabled = true;
                lbWrong2.Visibility = Visibility.Hidden;
            }
        }
    }
}
