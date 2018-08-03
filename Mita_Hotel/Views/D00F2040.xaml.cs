using Lemon3.Controls.DevExp;
using Lemon3.Data;
using Mita_Hotel.BL;
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
    /// Interaction logic for D00F2040.xaml
    /// </summary>
    public partial class D00F2040 : L3Window
    {
        public D00F2040()
        {
            InitializeComponent();
        }
        private static string Username { get; set; }

        private void LoadDefault()
        {
            Checker(0, 0, 0, 0, 0, 0, 0, 0);
        }

        private int[] GetPermission;

        private void Checker(int D00F0040, int D00F1040, int D05F2010, int D91F2140, int D07F2010, int D91F1240Location, int D91F1240UnitID, int D91F1240Object)
        {
            switch (D00F0040)
            {
                case 0:
                    D00F0040_0.IsChecked = true;
                    break;
                case 1:
                    D00F0040_1.IsChecked = true;
                    break;
                case 2:
                    D00F0040_2.IsChecked = true;
                    break;
                case 3:
                    D00F0040_3.IsChecked = true;
                    break;
                case 4:
                    D00F0040_4.IsChecked = true;
                    break;
                default:
                    D00F0040_0.IsChecked = true;
                    break;
            }
            switch (D00F1040)
            {
                case 0:
                    D00F1040_0.IsChecked = true;
                    break;
                case 1:
                    D00F1040_1.IsChecked = true;
                    break;
                case 2:
                    D00F1040_2.IsChecked = true;
                    break;
                case 3:
                    D00F1040_3.IsChecked = true;
                    break;
                case 4:
                    D00F1040_4.IsChecked = true;
                    break;
                default:
                    D00F1040_0.IsChecked = true;
                    break;
            }
            switch (D05F2010)
            {
                case 0:
                    D05F2010_0.IsChecked = true;
                    break;
                case 1:
                    D05F2010_1.IsChecked = true;
                    break;
                case 2:
                    D05F2010_2.IsChecked = true;
                    break;
                case 3:
                    D05F2010_3.IsChecked = true;
                    break;
                case 4:
                    D05F2010_4.IsChecked = true;
                    break;
                default:
                    D05F2010_0.IsChecked = true;
                    break;
            }
            switch (D91F2140)
            {
                case 0:
                    D91F2140_0.IsChecked = true;
                    break;
                case 1:
                    D91F2140_1.IsChecked = true;
                    break;
                case 2:
                    D91F2140_2.IsChecked = true;
                    break;
                case 3:
                    D91F2140_3.IsChecked = true;
                    break;
                case 4:
                    D91F2140_4.IsChecked = true;
                    break;
                default:
                    D91F2140_0.IsChecked = true;
                    break;
            }
            switch (D07F2010)
            {
                case 0:
                    D07F2010_0.IsChecked = true;
                    break;
                case 1:
                    D07F2010_1.IsChecked = true;
                    break;
                case 2:
                    D07F2010_2.IsChecked = true;
                    break;
                case 3:
                    D07F2010_3.IsChecked = true;
                    break;
                case 4:
                    D07F2010_4.IsChecked = true;
                    break;
                default:
                    D07F2010_0.IsChecked = true;
                    break;
            }
            switch (D91F1240Location)
            {
                case 0:
                    D91F1240_Location_0.IsChecked = true;
                    break;
                case 1:
                    D91F1240_Location_1.IsChecked = true;
                    break;
                case 2:
                    D91F1240_Location_2.IsChecked = true;
                    break;
                case 3:
                    D91F1240_Location_3.IsChecked = true;
                    break;
                case 4:
                    D91F1240_Location_4.IsChecked = true;
                    break;
                default:
                    D91F1240_Location_0.IsChecked = true;
                    break;
            }
            switch (D91F1240UnitID)
            {
                case 0:
                    D91F1240_UnitID_0.IsChecked = true;
                    break;
                case 1:
                    D91F1240_UnitID_1.IsChecked = true;
                    break;
                case 2:
                    D91F1240_UnitID_2.IsChecked = true;
                    break;
                case 3:
                    D91F1240_UnitID_3.IsChecked = true;
                    break;
                case 4:
                    D91F1240_UnitID_4.IsChecked = true;
                    break;
                default:
                    D91F1240_UnitID_0.IsChecked = true;
                    break;
            }
            switch (D91F1240Object)
            {
                case 0:
                    D91F1240_Object_0.IsChecked = true;
                    break;
                case 1:
                    D91F1240_Object_1.IsChecked = true;
                    break;
                case 2:
                    D91F1240_Object_2.IsChecked = true;
                    break;
                case 3:
                    D91F1240_Object_3.IsChecked = true;
                    break;
                case 4:
                    D91F1240_Object_4.IsChecked = true;
                    break;
                default:
                    D91F1240_Object_0.IsChecked = true;
                    break;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ////////////////////////TODO///////////////////////////
            btnSave.Focus();
            if (!btnSave.IsFocused) return;
            StringBuilder sSQL = new StringBuilder();
            sSQL.AppendLine(BLPermission.SQLDeleteD00T2040(Username));
            sSQL.AppendLine(BLPermission.SQLInsertD00T2040(Username, "D00F0040", GetPermission[0]));
            if (L3SQLServer.ExecuteSQL(sSQL.ToString()))
            {
                Lemon3.Messages.L3Msg.SaveOK();
                return;
            }
            else
            {
                Lemon3.Messages.L3Msg.SaveNotOK();
                btnSave.IsEnabled = true;
            }
        }

        private void L3Window_Loaded(object sender, RoutedEventArgs e)
        {
            Username = txtUsername.Text;
            LoadDefault();
            GetPermission = BLPermission.GetPermission(Username);
            Checker(GetPermission[0], GetPermission[1], GetPermission[2], GetPermission[3], GetPermission[4], GetPermission[5], GetPermission[6], GetPermission[7]);
        }
    }
}
