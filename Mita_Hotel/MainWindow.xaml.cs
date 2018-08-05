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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lemon3;
using Mita_Hotel;
using Mita_Hotel.Views;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using Lemon3.Data;
using Lemon3.Controls.DevExp;
using DevExpress.Xpf.Docking;
using Mita_Hotel.BL;
using Mita_Hotel.Models;

namespace Mita_Hotel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : L3Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.ShowInTaskbar = true;
        }

        private int[] PermissionValues = { 0, 0, 0, 0, 0, 0, 0, 0 };
        //Index: [0]:D00F0040, [1]:D00F1040, [2]:D05F2010, [3]:D91F2140, [4]:D07F2010, [5]:D91F1240Location, [6]:D91F1240UnitID, [7]:D91F1240Object
        //Value: 0:None, 1:Read, 2:Add, 3:AddDeleteEdit, 4:Full

        private void Window_Activated(object sender, EventArgs e)
        {
            if (L3.IsUseWebService == false)
            {
                lblServerName.Content = "Server: " + L3.Server.ToUpper();
                lblDatabases.Content = "Database: " + L3.CompanyID.ToUpper();
            }
            else
            {
                lblServerName.Content = "";
                lblDatabases.Content = "Web Service: " + L3.ServerServicePath.ToUpper();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UserLogin UserLogged = new UserLogin();

            PermissionValues = BLPermission.GetPermissionFromDB(L3.UserID); //Permission 0 value Setting
            for(int i = 0; i<=7; i++)
            {
                if(PermissionValues[i] == 0)
                {
                    switch(i)
                    {
                        case 0:         //D00F0040
                            miListUser.IsEnabled = false;
                            break;
                        case 1:         //D00F1040
                            miListItems.IsEnabled = false;
                            break;
                        case 2:         //D05F2010
                            miTable.IsEnabled = false;
                            break;
                        case 3:         //D91F2140
                            miVoucher.IsEnabled = false;
                            break;
                        case 4:         //D07F2010
                            miStorehouse.IsEnabled = false;
                            break;
                        case 5:         //D91F1240Location
                            miLocation.IsEnabled = false;
                            break;
                        case 6:         //D91F1240UnitID
                            miUnitID.IsEnabled = false;
                            break;
                        case 7:         //D91F1240Object
                            miObject.IsEnabled = false;
                            break;
                    }
                }
            }   //End of Permission 0 value Setting

            DataTable dt = BLMainWindow.GetUserLogged(L3.UserID);   //Get info from User and show the Label
            try
            {
                UserLogged.Fullname = dt.Rows[0]["Fullname"].ToString();
                string LastLogin = dt.Rows[0]["LastLogin"].ToString();
                UserLogged.LastLogin = Convert.ToDateTime(LastLogin);
                UserLogged.RoleID = dt.Rows[0]["RoleID"].ToString();
                lblUserID.Content = "User: " + UserLogged.Fullname;
            }
            catch (Exception)
            {
                MessageBox.Show("Tài khoản của bạn đã bị hacked, đầu hàng đi!");
                Properties.Settings.Default.checkerAuto = false;
            } //End of Get info
        }

        private void bbcMain_RibbonPopupMenuShowing(object sender, DevExpress.Xpf.Ribbon.RibbonPopupMenuShowingEventArgs e)
        {
            e.Cancel = true;
        }

        private void CallWindow(string sPageName)   //Calling page forms except D91F1240s
        {
            BaseLayoutItem docPanelFind = documentGroup.Items.FirstOrDefault(f => f.Caption.ToString().Contains(sPageName));
            if (docPanelFind == null)
            {
                switch (sPageName)
                {
                    case "D00F0040":
                        D00F0040 pageListUser = new D00F0040();
                        switch(PermissionValues[0])
                        {
                            case 1:
                                pageListUser.tsbAdd.IsVisible = false;
                                pageListUser.tsbDelete.IsVisible = false;
                                pageListUser.tsbEdit.IsVisible = false;
                                pageListUser.mnsUserRight.IsEnabled = false;
                                break;
                            case 2:
                                pageListUser.tsbDelete.IsVisible = false;
                                pageListUser.tsbEdit.IsVisible = false;
                                pageListUser.mnsUserRight.IsEnabled = false;
                                break;
                            case 3:
                                pageListUser.mnsUserRight.IsEnabled = false;
                                break;
                            case 4:
                                //Because of FULL permission so i don't know what to do here =]]z//
                                break;
                        }
                        CreateDocumentPanel(pageListUser);
                        break;
                    case "D00F1040":
                        D00F1040 pageListItem = new D00F1040();
                        switch (PermissionValues[1])
                        {
                            case 1:
                                pageListItem.tsbAdd.IsVisible = false;
                                pageListItem.tsbDelete.IsVisible = false;
                                pageListItem.tsbEdit.IsVisible = false;
                                break;
                            case 2:
                                pageListItem.tsbDelete.IsVisible = false;
                                pageListItem.tsbEdit.IsVisible = false;
                                break;
                            case 3:
                                //There are no more things to show =]]z
                                break;
                            case 4:
                                //Because of FULL permission so i don't know what to do here =]]z//
                                break;
                        }
                        CreateDocumentPanel(pageListItem);
                        break;
                    case "D05F2010":
                        D05F2010 pageTable = new D05F2010();
                        switch (PermissionValues[2])
                        {
                            case 1:
                                pageTable.tsbAdd.IsVisible = false;
                                pageTable.tsbDelete.IsVisible = false;
                                pageTable.tsbEdit.IsVisible = false;
                                pageTable.mnsAdd.IsEnabled = false;
                                pageTable.mnsNew.IsEnabled = false;
                                pageTable.mnsPay.IsEnabled = false;
                                break;
                            case 2:
                                pageTable.tsbDelete.IsVisible = false;
                                pageTable.tsbEdit.IsVisible = false;
                                pageTable.mnsAdd.IsEnabled = false;
                                pageTable.mnsNew.IsEnabled = false;
                                pageTable.mnsPay.IsEnabled = false;
                                break;
                            case 3:
                                pageTable.mnsAdd.IsEnabled = false;
                                pageTable.mnsNew.IsEnabled = false;
                                pageTable.mnsPay.IsEnabled = false;
                                break;
                            case 4:
                                //Because of FULL permission so i don't know what to do here =]]z//
                                break;
                        }
                        CreateDocumentPanel(pageTable);
                        break;
                    case "D91F2140":
                        D91F2140 pageVoucher = new D91F2140(); //This form is a read-only form so I will not hide anything here.
                        CreateDocumentPanel(pageVoucher);
                        break;
                    case "D07F2010":
                        D07F2010 pageStorage = new D07F2010();
                        switch (PermissionValues[4])
                        {
                            case 1:
                                pageStorage.tsbAdd.IsVisible = false;
                                pageStorage.tsbDelete.IsVisible = false;
                                pageStorage.tsbEdit.IsVisible = false;
                                break;
                            case 2:
                                pageStorage.tsbDelete.IsVisible = false;
                                pageStorage.tsbEdit.IsVisible = false;
                                break;
                            case 3:
                                //There are no more things to show =]]z
                                break;
                            case 4:
                                //Because of FULL permission so i don't know what to do here =]]z//
                                break;
                        }
                        CreateDocumentPanel(pageStorage);
                        break;
                }
            }
            else
            {
                dockLayoutManager.DockController.Activate(docPanelFind);
            }
        }

        private void CallD91F1240(string ListTypeID, string CaptionForm) //Calling D91F1240 page forms
        {
            D91F1240 pageListGeneral = new D91F1240();
            pageListGeneral.Title = CaptionForm;
            pageListGeneral.ListTypeID = ListTypeID;
            int temp = 0;
            switch (ListTypeID)
            {
                case "UnitID":
                    temp = 6;
                    break;
                case "TT":
                    temp = 5;
                    break;
                case "ObjectID":
                    temp = 7;
                    break;
            }
            switch (PermissionValues[temp])
            {
                case 1:
                    pageListGeneral.tsbAdd.IsVisible = false;
                    pageListGeneral.tsbDelete.IsVisible = false;
                    pageListGeneral.tsbEdit.IsVisible = false;
                    break;
                case 2:
                    pageListGeneral.tsbDelete.IsVisible = false;
                    pageListGeneral.tsbEdit.IsVisible = false;
                    break;
                case 3:
                    //There are no more things to show =]]z
                    break;
                case 4:
                    //Because of FULL permission so i don't know what to do here =]]z//
                    break;
            }
            CreateDocumentPanel(pageListGeneral);
        }

        private void CreateDocumentPanel(Page page)
        {
            DocumentPanel newDocumentPanel = new DocumentPanel();
            newDocumentPanel.Caption = page.Title;
            Frame frame = new Frame();
            frame.Navigate(page);
            newDocumentPanel.Content = frame;
            
            newDocumentPanel.IsActive = true;
            documentGroup.Add(newDocumentPanel);
            dockLayoutManager.DockController.Activate(newDocumentPanel);
        }

        private void miLogOut_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Properties.Settings.Default.checkerAuto = false;
            Properties.Settings.Default.Save();
            System.Windows.Forms.Application.Restart();
            this.Close();
        }

        private void miExit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Environment.Exit(0);
        }

        private void miListUser_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            CallWindow("D00F0040");
        }

        private void miListItems_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            CallWindow("D00F1040");
        }
    
        private void miUnitID_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            CallD91F1240("UnitID","Danh mục Đơn vị tính");

        }

        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            CallD91F1240("TT", "Danh mục Tỉnh thành");
        }

        private void miTable_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            CallWindow("D05F2010");
        }

        private void miPBL_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            CallWindow("D91F2140");
        }

        private void miStorehouse_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            CallWindow("D07F2010");
        }

        private void miObject_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            CallD91F1240("ObjectID", "Danh mục Nhà cung cấp");
        }
    }
}
