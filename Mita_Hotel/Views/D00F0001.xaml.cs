using Lemon3;
using Lemon3.Controls.DevExp;
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

namespace Mita_Hotel.Views
{
    /// <summary>
    /// Interaction logic for D00F0001.xaml
    /// </summary>
    public partial class D00F0001 : L3Window
    {
        public D00F0001()
        {
            InitializeComponent();
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.MitaServer = txtServer.Text;
            Properties.Settings.Default.MitaLogin = txtLogin.Text;
            Properties.Settings.Default.MitaPassword = txtPassword.Text;
            Properties.Settings.Default.MitaDB = txtDB.Text;
            L3.Server = txtServer.Text;
            L3.ConnectionUser = txtLogin.Text;
            L3.Password = txtPassword.Text;
            L3.CompanyID = txtDB.Text;
            Close();
        }

        private void miHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Liên hệ: leminh0796@gmail.com");
        }

        private void miClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
