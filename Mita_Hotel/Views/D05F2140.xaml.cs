using Lemon3;
using Lemon3.Controls.DevExp;
using Lemon3.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Lemon3.Functions;
using System.Threading;
using System.Data.SqlClient;
using System.Windows.Threading;
using Mita_Coffee.BL;

namespace Mita_Coffee.Views
{
    /// <summary>
    /// Interaction logic for D05F2140.xaml
    /// </summary>
    public partial class D05F2140 : L3Window
    {
        public D05F2140()
        {
            InitializeComponent();
            txtItem.TextChanged += txtItem_TextChanged;
        }
        public string TableID { get; set; }
        public string VoucherID { get; set; }
        DataTable dtGrid;
        public bool Saved { get; private set; }
        public int Status { get; set; }
        public decimal AmountPayment { private get; set; }

        void txtItem_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataTable DtSuggest = BLTable.LoadSuggest();
            string typedString = txtItem.Text;
            DataTable AutoTable = DtSuggest.Clone();
            AutoTable.Clear();
            foreach (DataRow item in DtSuggest.Rows)
            {
                if (!string.IsNullOrEmpty(txtItem.Text))
                {
                    if (item["InventoryID"].ToString().Contains(typedString) || item["InventoryName"].ToString().Contains(typedString) || item["BarCode"].ToString().Contains(typedString))
                    {
                        AutoTable.ImportRow(item);
                    }
                }
            }

            if (AutoTable.Rows.Count > 0)
            {
                lbeSuggestion.DisplayMember = "InventoryName";
                lbeSuggestion.ValueMember = "InventoryID";
                lbeSuggestion.ItemsSource = AutoTable;
                lbeSuggestion.Visibility = Visibility.Visible;
            }
            else if (txtItem.Text.Equals(""))
            {
                lbeSuggestion.Visibility = Visibility.Collapsed;
                lbeSuggestion.ItemsSource = null;
            }
            else
            {
                lbeSuggestion.Visibility = Visibility.Collapsed;
                lbeSuggestion.ItemsSource = null;
            }
        }

        private void L3Window_Loaded(object sender, RoutedEventArgs e)
        {
            sePeople.Focus();
            if (VoucherID == "") VoucherID = "PBL" + L3.UserID.ToUpper() + DateTime.Now.ToString("yyyyMMddHHmmss");
            SetInputNumber();
            L3Control.SetBackColorObligatory(sePeople);
            LoadMaster();
            LoadTDBGrid();
            if (GridVoucherInventory.VisibleRowCount == 0)
            {
                btnAddQuatity.IsEnabled = false;
                btnDeleteQuatity.IsEnabled = false;
                btnAdd10Quatity.IsEnabled = false;
            }
        }

        private void SetInputNumber()
        {
            GridVoucherInventory.InputNumber288("n1", false, false, COL_Quantity);
            GridVoucherInventory.InputNumber288("n0", false, false, COL_Amount);
            GridVoucherInventory.InputNumber288("n0", false, false, COL_Price);
            GridVoucherInventory.InputPercent(false, false, 28, 8, COL_VAT);
            GridVoucherInventory.InputNumber288("n0", false, false, COL_InStock);
            seTotalMoney.InputNumber288("n0", false, false);
            sePeople.InputNumber288("n0", false, false);
        }

        private void LoadTDBGrid()
        {
            dtGrid = BLTable.LoadVoucherDataGrid(L3SQLClient.SQLString(VoucherID));
            L3DataSource.LoadDataSource(GridVoucherInventory, dtGrid, true);
            if (dtGrid.Rows.Count == 0)
            {
                btnPay.IsEnabled = false;
                btnDeleteQuatity.IsEnabled = false;
            }

            else
            {
                btnPay.IsEnabled = true;
                btnDeleteQuatity.IsEnabled = true;
            }
        }
        private void LoadMaster()
        {
            lbVoucherID.Content = "Phiếu đặt: " + VoucherID;
            DataTable dtMaster = BLTable.LoadMaster(L3SQLClient.SQLString(VoucherID));
            if (dtMaster.Rows.Count > 0)
            {
                lbUserID.Content = "Thu ngân: " + dtMaster.Rows[0]["UserID"];
                seTotalMoney.EditValue = dtMaster.Rows[0]["Amount"];
                sePeople.EditValue = dtMaster.Rows[0]["CountPerson"];
            }
            else
            {
                lbUserID.Content = "Thu ngân: " + L3.UserID;
                seTotalMoney.EditValue = 0;
                sePeople.EditValue = 0;
                Status = 1;
            }
        }

        private void CalAmountGrid(int irow)
        {
            try
            {
                GridVoucherInventory.SetCellValue(irow, "Amount", L3ConvertType.Number(GridVoucherInventory.GetCellValue(irow, "Quantity")) * L3ConvertType.Number(GridVoucherInventory.GetCellValue(irow, "Price")) * (1 - L3ConvertType.Number(GridVoucherInventory.GetCellValue(irow, "VAT"))));
            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng trỏ đúng!");
            }

        }

        private void txtItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                lbeSuggestion.Focus();
            }
        }

        private void lbeSuggestion_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            string sInventoryID = lbeSuggestion.EditValue.ToString();
            DataTable dt = BLTable.SelectInventoryID(sInventoryID);
            string sInventoryName = "";
            if (dt.Rows.Count > 0 && Convert.ToDecimal(dt.Rows[0]["InStock"]) > 0)
            {
                btnPay.IsEnabled = true;
                sInventoryName = L3ConvertType.L3String(dt.Rows[0]["InventoryName"]);
                int irow = GridVoucherInventory.FindRowByValue("InventoryID", sInventoryID);
                if (irow >= 0)
                {
                    int value_Old = L3ConvertType.L3Int(GridVoucherInventory.GetCellValue(irow, COL_Quantity));
                    int stock_Old = L3ConvertType.L3Int(GridVoucherInventory.GetCellValue(irow, COL_InStock));
                    if (stock_Old > 0)
                    {
                        GridVoucherInventory.SetCellValue(irow, "Quantity", value_Old + 1);
                        GridVoucherInventory.SetCellValue(irow, "InStock", stock_Old - 1);
                    }
                    CalAmountGrid(irow);
                }
                else
                {
                    DataRow dr = dtGrid.NewRow();
                    dr["InventoryID"] = sInventoryID;
                    dr["InventoryName"] = dt.Rows[0]["InventoryName"];
                    dr["UnitID"] = dt.Rows[0]["UnitID"];
                    dr["InStock"] = Convert.ToDecimal(dt.Rows[0]["InStock"]) - 1;
                    dr["Quantity"] = 1;
                    dr["Price"] = dt.Rows[0]["Price"];
                    dr["VAT"] = dt.Rows[0]["VAT"];
                    dr["Amount"] = L3ConvertType.Number(dr["Quantity"]) * L3ConvertType.Number(dr["Price"]) * (1 - L3ConvertType.Number(dr["VAT"]));
                    dtGrid.Rows.Add(dr);
                }
                btnSave.IsEnabled = true;
                btnDeleteQuatity.IsEnabled = true;
                btnAddQuatity.IsEnabled = true;
                btnAdd10Quatity.IsEnabled = true;
            }
            else MessageBox.Show("Đã hết hàng!");
            CalAmount();
            txtItem.Text = "";
            lbeSuggestion.Visibility = Visibility.Collapsed;
            lbeSuggestion.EditValue = null;
        }

        private double CalAmount()
        {
            double dValue = 0;

            foreach (DataRow row in dtGrid.Rows)
            {
                dValue += L3ConvertType.Number(row["Amount"], "n0");
            }
            seTotalMoney.EditValue = dValue;
            return dValue;
        }
        private bool AllowSave()
        {
            if (sePeople.Text == "" || sePeople.Text == "0")
            {
                D99D0041.D99C0008.MsgNotYetEnter("Số người");
                sePeople.Focus();
                return false;
            }
            return true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            btnSave.Focus();
            if (!btnSave.IsFocused) return;
            if (!AllowSave()) return;
            btnSave.IsEnabled = false;
            DoSaveSQL();
        }

        private void DoSaveSQL()
        {
            if (AmountPayment > 0)
            {
                Status = 2;
            }
            StringBuilder sSQL = new StringBuilder();
            sSQL.AppendLine(BLTable.SQLDeleteD91T2141(VoucherID));
            sSQL.AppendLine(BLTable.SQLDeleteD91T2140(VoucherID));
            sSQL.AppendLine(BLTable.SQLInsertD91T2140(VoucherID, TableID, sePeople.EditValue, sePeople.NumberFormat, seTotalMoney.EditValue, seTotalMoney.NumberFormat, Status, AmountPayment));
            sSQL.AppendLine(BLTable.SQLInsertD91T2141s(dtGrid, VoucherID));
            sSQL.AppendLine(BLTable.SQLUpdateStockD91T1040s(dtGrid));
            if (L3SQLServer.ExecuteSQL(sSQL.ToString()))
            {
                if (Status == 1)
                {
                    BLTable.UpdateStatus1(L3SQLClient.SQLMoney(seTotalMoney.EditValue, seTotalMoney.NumberFormat), L3SQLClient.SQLMoney(sePeople.EditValue, sePeople.NumberFormat), TableID);
                }
                else if (Status == 2)
                {
                    BLTable.UpdateStatus2(L3SQLClient.SQLMoney(seTotalMoney.EditValue, seTotalMoney.NumberFormat), L3SQLClient.SQLMoney(sePeople.EditValue, sePeople.NumberFormat), TableID);
                }
                Lemon3.Messages.L3Msg.SaveOK();
                Saved = true;
            }
            else
            {
                Lemon3.Messages.L3Msg.SaveNotOK();
                btnSave.IsEnabled = true;
            }
        }

        private bool AllowPay()
        {
            if (Status != 1 || sePeople.Value == 0)
            {
                sePeople.Focus();
                return false;
            }
            return true;
        }
        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            if (!AllowPay()) return;
            D05F2141 frmPayment = new D05F2141();
            frmPayment.TotalMoney = Convert.ToDecimal(seTotalMoney.EditValue);
            frmPayment.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmPayment.ShowDialog();
            if (!frmPayment.bClicked) return;
            btnPay.IsEnabled = false;
            txtItem.IsEnabled = false;
            AmountPayment = frmPayment.TotalMoney;
            DoSaveSQL();
            Close();
        }

        private void btnDeleteQuatity_Click(object sender, RoutedEventArgs e)
        {
            if (dtGrid.Rows.Count > 0)
            {
                string InventoryID = GridVoucherInventory.GetFocusedRowCellValue(COL_InventoryID).ToString();
                int n = BLVoucher.GetCurrentRowIndex(GridVoucherInventory, "InventoryID");
                int value_Old = L3ConvertType.L3Int(GridVoucherInventory.GetCellValue(n, COL_Quantity));
                int stock_Old = L3ConvertType.L3Int(GridVoucherInventory.GetCellValue(n, COL_InStock));
                if (value_Old > 1)
                {
                    GridVoucherInventory.SetCellValue(n, "Quantity", value_Old - 1);
                    GridVoucherInventory.SetCellValue(n, "InStock", stock_Old + 1);
                    CalAmountGrid(n);
                }
                else if (value_Old == 1)
                {
                    GridVoucherInventory.DeleteRowFocusEvent();
                }
                CalAmount();
                btnSave.IsEnabled = true;
            }
            if (GridVoucherInventory.VisibleRowCount == 0)
            {
                btnAddQuatity.IsEnabled = false;
                btnDeleteQuatity.IsEnabled = false;
                btnAdd10Quatity.IsEnabled = false;
            }
        }

        private void btnAddQuatity_Click(object sender, RoutedEventArgs e)
        {
            int n = BLVoucher.GetCurrentRowIndex(GridVoucherInventory, "InventoryID");
            try
            {
                int value_Old = L3ConvertType.L3Int(GridVoucherInventory.GetCellValue(n, COL_Quantity));
                int stock_Old = L3ConvertType.L3Int(GridVoucherInventory.GetCellValue(n, COL_InStock));
                if (stock_Old > 0)
                {
                    GridVoucherInventory.SetCellValue(n, "Quantity", value_Old + 1);
                    GridVoucherInventory.SetCellValue(n, "InStock", stock_Old - 1);
                }
                CalAmountGrid(n);
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi!");
            }
            CalAmount();
            btnSave.IsEnabled = true;
        }

        private void btnAdd10Quatity_Click(object sender, RoutedEventArgs e)
        {
            int n = BLVoucher.GetCurrentRowIndex(GridVoucherInventory, "InventoryID");
            try
            {
                int value_Old = L3ConvertType.L3Int(GridVoucherInventory.GetCellValue(n, COL_Quantity));
                int stock_Old = L3ConvertType.L3Int(GridVoucherInventory.GetCellValue(n, COL_InStock));
                if (stock_Old >= 10)
                {
                    GridVoucherInventory.SetCellValue(n, "Quantity", value_Old + 10);
                    GridVoucherInventory.SetCellValue(n, "InStock", stock_Old - 10);
                }
                CalAmountGrid(n);
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi!");
            }
            CalAmount();
            btnSave.IsEnabled = true;
        }
    }
}
