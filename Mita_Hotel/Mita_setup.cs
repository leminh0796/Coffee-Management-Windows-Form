using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lemon3;
using Mita_Hotel.Views;
using Mita_Hotel;

namespace Mita_Hotel
{
    class Mita_setup
    {
        [STAThread]
        public static void Main()
        {
            Setdefault();
            frmLogin frm = new frmLogin();
            frm.ShowDialog();
            if (frm.LoginSuccess)
            {
                MainWindow Main = new MainWindow();
                Main.WindowState = System.Windows.WindowState.Maximized;
                Main.ShowDialog();
            }
        }
        private static void Setdefault()
        {
            L3.ModuleIDRunning = "DA1";
            L3.IsUniCode = true;
            L3.Language = EnumLanguage.Vietnamese;
            L3.STRLanguage = "84";
            L3.ApplicationPath = Environment.CurrentDirectory;
            L3.ApplicationSetup = L3.ApplicationPath;
            L3.Server = @"10.0.0.221";
            L3.ConnectionUser = "sa";
            L3.Password = "123";
            L3.CompanyID = "MitaEmployees";
            L3.UserID = "LEMONADMIN";
            L3.ConnectionString = "Data Source=" + L3.Server + ";Initial Catalog=" + L3.CompanyID + ";User ID=" + L3.ConnectionUser + ";Password=" + L3.Password + ";Connect Timeout = 0";
        }
    }
}
