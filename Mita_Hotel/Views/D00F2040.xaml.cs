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

        private void LoadDefault() //Checks the default permission when window loaded (0:None)
        {
            Checker(0, 0, 0, 0, 0, 0, 0, 0);
        }

        private int[] GetPermission = { 0, 0, 0, 0, 0, 0, 0, 0 };
        //////An integer array hold the permission values for each form below:
        //////VALUE RANGE 0-4 with 0:[None]; 1:[ReadOnly]; 2:[Add]; 3:[Add, Delete, Edit]; 4:[FullPermission].
        //////Forms index:
        //////GetPermission[0]: D00F0040
        //////GetPermission[1]: D00F1040
        //////GetPermission[2]: D05F2010
        //////GetPermission[3]: D91F2140
        //////GetPermission[4]: D07F2010
        //////GetPermission[5]: D91F1240Location
        //////GetPermission[6]: D91F1240UnitID
        //////GetPermission[7]: D91F1240Object

        //The Checker function will do the checking one-by-one forms declare below:
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
        //The index variable of Checker function is similar to GetPermission array.


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            btnSave.Focus();
            if (!btnSave.IsFocused) return;
            StringBuilder sSQL = new StringBuilder();
            sSQL.AppendLine(BLPermission.SQLDeleteD00T2040(Username));
            sSQL.AppendLine(BLPermission.SQLInsertD00T2040(Username, "D00F0040", GetPermission[0]));
            sSQL.AppendLine(BLPermission.SQLInsertD00T2040(Username, "D00F1040", GetPermission[1]));
            sSQL.AppendLine(BLPermission.SQLInsertD00T2040(Username, "D05F2010", GetPermission[2]));
            sSQL.AppendLine(BLPermission.SQLInsertD00T2040(Username, "D91F2140", GetPermission[3]));
            sSQL.AppendLine(BLPermission.SQLInsertD00T2040(Username, "D07F2010", GetPermission[4]));
            sSQL.AppendLine(BLPermission.SQLInsertD00T2040(Username, "D91F1240Location", GetPermission[5]));
            sSQL.AppendLine(BLPermission.SQLInsertD00T2040(Username, "D91F1240UnitID", GetPermission[6]));
            sSQL.AppendLine(BLPermission.SQLInsertD00T2040(Username, "D91F1240Object", GetPermission[7]));
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
            GetPermission = BLPermission.GetPermissionFromDB(Username);     //Load the permission from Database
            Checker(GetPermission[0], GetPermission[1], GetPermission[2], GetPermission[3], GetPermission[4], GetPermission[5], GetPermission[6], GetPermission[7]);
        }


        /////////////////////////CHECKING EVENT BELOW////////////////////////////
        ////////////////////////WARNING: WALL OF LOOP CODE!!!////////////////////
        private void D00F0040_0_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[0] = 0;
        }

        private void D00F0040_1_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[0] = 1;
        }

        private void D00F0040_2_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[0] = 2;
        }

        private void D00F0040_3_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[0] = 3;
        }

        private void D00F0040_4_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[0] = 4;
        }

        private void D00F1040_0_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[1] = 0;
        }

        private void D00F1040_1_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[1] = 1;
        }

        private void D00F1040_2_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[1] = 2;
        }

        private void D00F1040_3_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[1] = 3;
        }

        private void D00F1040_4_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[1] = 4;
        }

        private void D05F2010_0_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[2] = 0;
        }

        private void D05F2010_1_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[2] = 1;
        }

        private void D05F2010_2_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[2] = 2;
        }

        private void D05F2010_3_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[2] = 3;
        }

        private void D05F2010_4_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[2] = 4;
        }

        private void D91F2140_0_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[3] = 0;
        }

        private void D91F2140_1_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[3] = 1;
        }

        private void D91F2140_2_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[3] = 2;
        }

        private void D91F2140_3_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[3] = 3;
        }

        private void D91F2140_4_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[3] = 4;
        }

        private void D07F2010_0_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[4] = 0;
        }

        private void D07F2010_1_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[4] = 1;
        }

        private void D07F2010_2_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[4] = 2;
        }

        private void D07F2010_3_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[4] = 3;
        }

        private void D07F2010_4_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[4] = 4;
        }

        private void D91F1240_UnitID_0_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[5] = 0;
        }

        private void D91F1240_UnitID_1_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[5] = 1;
        }

        private void D91F1240_UnitID_2_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[5] = 2;
        }

        private void D91F1240_UnitID_3_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[5] = 3;
        }

        private void D91F1240_UnitID_4_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[5] = 4;
        }

        private void D91F1240_Location_0_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[6] = 0;
        }

        private void D91F1240_Location_1_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[6] = 1;
        }

        private void D91F1240_Location_2_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[6] = 2;
        }

        private void D91F1240_Location_3_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[6] = 3;
        }

        private void D91F1240_Location_4_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[6] = 4;
        }

        private void D91F1240_Object_0_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[7] = 0;
        }

        private void D91F1240_Object_1_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[7] = 1;
        }

        private void D91F1240_Object_2_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[7] = 2;
        }

        private void D91F1240_Object_3_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[7] = 3;
        }

        private void D91F1240_Object_4_Checked(object sender, RoutedEventArgs e)
        {
            GetPermission[7] = 4;
        }
        //////////////////////END OF CHECKING EVENT FUNCTIONS////////////////////
    }
}
