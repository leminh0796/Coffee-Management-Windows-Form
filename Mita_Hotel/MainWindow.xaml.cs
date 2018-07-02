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
            UserLogged.Username = frmLogin.UserLogged;
            L3.UserID = frmLogin.UserLogged;
            SqlCommand cmd = new SqlCommand("select Fullname, LastLogin, RoleID from D00T0040 WHERE Username = '"  + L3.UserID + "'");
            DataTable dt = L3SQLServer.ReturnDataTable(cmd.CommandText);
            UserLogged.Fullname = dt.Rows[0]["Fullname"].ToString();
            string LastLogin = dt.Rows[0]["LastLogin"].ToString();
            UserLogged.LastLogin = Convert.ToDateTime(LastLogin);
            UserLogged.RoleID = dt.Rows[0]["RoleID"].ToString();
            lblUserID.Content = "User: " + UserLogged.Fullname;
            if (UserLogged.RoleID == "GUEST" || UserLogged.RoleID == "REP") rpgListUser.IsVisible = false;
        }

        private void bbcMain_RibbonPopupMenuShowing(object sender, DevExpress.Xpf.Ribbon.RibbonPopupMenuShowingEventArgs e)
        {
            e.Cancel = true;
        }

        private void CallWindow(string sPageName)
        {
            BaseLayoutItem docPanelFind = documentGroup.Items.FirstOrDefault(f => f.Caption.ToString().Contains(sPageName));
            if (docPanelFind == null)
            {
                switch (sPageName)
                {
                    case "pageListUser":
                        pageListUser pageListUser = new pageListUser();
                        CreateDocumentPanel(pageListUser);
                        break;
                    case "pageListItem":
                        pageListItem pageListItem = new pageListItem();
                        CreateDocumentPanel(pageListItem);
                        break;
                }
            }
            else
            {
                dockLayoutManager.DockController.Activate(docPanelFind);

            }
        }
        private void CreateDocumentPanel(Page page)
        {
            DocumentPanel newDocumentPanel = new DocumentPanel();
            newDocumentPanel.Caption = page.Title;
            //this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            Frame frame = new Frame();
            frame.Navigate(page);
            newDocumentPanel.Content = frame;

            //newDocumentPanel.MDISize = new Size(window.Width, window.Height);
            newDocumentPanel.IsActive = true;
            documentGroup.Add(newDocumentPanel);
            dockLayoutManager.DockController.Activate(newDocumentPanel);
        }

        private void mbtnReg_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            frmRegister frmRegister = new frmRegister();
            frmRegister.Owner = this;
            frmRegister.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            frmRegister.ShowDialog();
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
            CallWindow("pageListUser");
        }

        private void miListItems_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            CallWindow("pageListItem");
        }

        private void CallD91F1240(string ListTypeID, string CaptionForm )
        {
           // BaseLayoutItem docPanelFind = documentGroup.Items.FirstOrDefault(f => f.Caption.ToString().Contains(sPageName));
            //if (docPanelFind == null)
            //{
                        D91F1240 pageListGeneral = new D91F1240();
                        pageListGeneral.Title = CaptionForm;
                        pageListGeneral.ListTypeID = ListTypeID;
                        CreateDocumentPanel(pageListGeneral);
            //}
        }
        private void miUnitID_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            CallD91F1240("UnitID","Danh mục Đơn vị tính");

        }

        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            CallD91F1240("TT", "Danh mục Tỉnh thành");
        }
    }
}
