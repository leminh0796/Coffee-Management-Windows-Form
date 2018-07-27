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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Mita_Hotel.BL;
using Lemon3.Functions;

namespace Mita_Hotel.Views
{
    /// <summary>
    /// Interaction logic for D07F2011.xaml
    /// </summary>
    public partial class D07F2011 : L3Window
    {
        public D07F2011()
        {
            InitializeComponent();
        }

        public string VoucherID { get; set; }

        private DataTable dtGrid;
        private int IsPayment = 0;
        private bool bAdd = false;

        private void LoadTDBDropdown()
        {
            DataTable dt1 = BLStore.ReturnObjectIDTable();
            DataTable dt2 = BLStore.ReturnLookupInventory();
            L3DataSource.LoadDataSource(lkeSupplier, dt1, true);
            L3DataSource.LoadDataSource(lkeInventoryID, dt2, true);
        }

        private void LoadTDBGrid()
        {
            dtGrid = L3SQLServer.ReturnDataTable(BLStore.SQLStoreD07P2011(VoucherID, 1));
            L3DataSource.LoadDataSource(GridVoucherInventory, dtGrid, true);
        }

        private void LoadAdd()
        {
            VoucherID = "PNK" + L3.UserID.ToUpper() + DateTime.Now.ToString("yyyyMMddHHmmss");
            lbVoucherID.Content = "Mã phiếu: " + VoucherID;
            chePaid.IsChecked = false;
        }

        private void LoadDefault()
        {
            LoadTDBGrid();
        }

        private void LoadMaster()
        {
            lbVoucherID.Content = "Mã phiếu: " + VoucherID;
            DataTable dtMaster = L3SQLServer.ReturnDataTable(BLStore.SQLStoreD07P2011(VoucherID, 0));
            if (dtMaster.Rows.Count > 0)
            {
                deVoucherDate.EditValue = dtMaster.Rows[0]["VoucherDate"];
                lkeSupplier.EditValue = dtMaster.Rows[0]["ObjectID"];
                seTotalMoney.EditValue = dtMaster.Rows[0]["Amount"];
                if (dtMaster.Rows[0]["IsPayment"].ToString() == "1")
                {
                    chePaid.IsChecked = true;
                    IsPayment = 1;
                    chePaid.IsReadOnly = true;
                    btnSave.IsEnabled = false;
                    COL_InventoryID.AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    COL_Quantity.AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    COL_Price.AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    COL_VAT.AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    COL_Discount.AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                }
                else
                {
                    chePaid.IsChecked = false;
                }
            }
        }

        private void LoadEdit()
        {
            LoadMaster();
            LoadTDBGrid();
        }

        private void LoadNumberFormat()
        {
            GridVoucherInventory.InputNumber288("n0", false, false, COL_Price);
            GridVoucherInventory.InputNumber288("n0", false, false, COL_Amount);
            GridVoucherInventory.InputNumber288("n0", false, false, COL_Discount);
            GridVoucherInventory.InputNumber288("n0", false, false, COL_Quantity);
            GridVoucherInventory.InputPercent(false, false, 28, 8, COL_Discount, COL_VAT);
            seTotalMoney.InputNumber288("n0", false, false);
        }

        private EnumFormState _fromState = EnumFormState.FormAdd;
        public EnumFormState FormState
        {
            set
            {

                _fromState = value;
                LoadTDBDropdown();
                switch (_fromState)
                {
                    case EnumFormState.FormAdd:
                        LoadAdd();
                        LoadDefault();
                        lkeSupplier.Focus();
                        break;
                    case EnumFormState.FormView:
                        LoadEdit();
                        btnSave.IsEnabled = false;
                        break;
                    case EnumFormState.FormEdit:
                        LoadEdit();
                        break;
                    default:
                        break;
                }
            }
        }



        private void L3Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadNumberFormat();
            GridVoucherInventory.SetDefaultGridControlInput();
        }

        private bool AllowSave()
        {
            if (lkeSupplier.DisplayText == "")
            {
                D99D0041.D99C0008.MsgNotYetEnter("Nhà cung cấp");
                lkeSupplier.Focus();
                return false;
            }
            if (deVoucherDate.DisplayText == "")
            {
                D99D0041.D99C0008.MsgNotYetEnter("Ngày nhập");
                deVoucherDate.Focus();
                return false;
            }
            return true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (chePaid.IsChecked == true)
            {
                IsPayment = 1;
            }
            else
            {
                IsPayment = 0;
            }
            CalTotalMoney();
            btnSave.Focus();
            if (!btnSave.IsFocused) return;
            if (!AllowSave()) return;
            btnSave.IsEnabled = false;
            double Amount = CalTotalMoney();
            StringBuilder sSQL = new StringBuilder();
            sSQL.AppendLine(BLStore.SQLDeleteD07T2011(VoucherID));
            sSQL.AppendLine(BLStore.SQLDeleteD07T2010(VoucherID));
            sSQL.AppendLine(BLStore.SQLInsertD07T2011s(VoucherID, dtGrid));
            sSQL.AppendLine(BLStore.SQLInsertD07T2010(VoucherID, lkeSupplier.EditValue.ToString(), Amount, IsPayment));
            if (bAdd) sSQL.AppendLine(BLStore.UpdateStockD91T1040(dtGrid));
            if (L3SQLServer.ExecuteSQL(sSQL.ToString()))
            {
                Lemon3.Messages.L3Msg.SaveOK();
                LoadMaster();
                VoucherID = "";
            }
            else
            {
                Lemon3.Messages.L3Msg.SaveNotOK();
                btnSave.IsEnabled = true;
            }
        }

        private void CallAmount()
        {
            object oPrice = GridVoucherInventory.GetFocusedRowCellValue(COL_Price);
            object oVAT = GridVoucherInventory.GetFocusedRowCellValue(COL_VAT);
            object oDiscount = GridVoucherInventory.GetFocusedRowCellValue(COL_Discount);
            object oQuantity = GridVoucherInventory.GetFocusedRowCellValue(COL_Quantity);
            object oAmount = L3ConvertType.Number(oQuantity) * L3ConvertType.Number(oPrice) * (1 - L3ConvertType.Number(oDiscount) + L3ConvertType.Number(oVAT));
            GridVoucherInventory.SetFocusedRowCellValue(COL_Amount, oAmount);
        }

        private double CalTotalMoney()
        {
            double dValue = 0;

            foreach (DataRow row in dtGrid.Rows)
            {
                dValue += L3ConvertType.Number(row["Amount"], "n0");
            }
            seTotalMoney.EditValue = dValue;
            return dValue;
        }

        private void gridview_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            btnSave.IsEnabled = true;
            switch (e.Column.FieldName)
            {
                case "InventoryID":
                    GridVoucherInventory.SetFocusedRowCellValue(COL_InventoryName, lkeInventoryID.ReturnValue("InventoryName"));
                    GridVoucherInventory.SetFocusedRowCellValue(COL_UnitID, lkeInventoryID.ReturnValue("UnitID"));
                    GridVoucherInventory.SetFocusedRowCellValue(COL_VAT, lkeInventoryID.ReturnValue("VAT"));
                    GridVoucherInventory.SetFocusedRowCellValue(COL_Price, lkeInventoryID.ReturnValue("Price"));
                    break;
                case "VAT":
                    CallAmount();
                    break;
                case "Discount":
                    CallAmount();
                    break;
                case "Quantity":
                    CallAmount();
                    break;
                case "Price":
                    CallAmount();
                    break;
                default:
                    CalTotalMoney();
                    break;
            }
            CalTotalMoney();
        }

        private void GridVoucherInventory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) CalTotalMoney();
        }

        private void GridVoucherInventory_CurrentItemChanged(object sender, DevExpress.Xpf.Grid.CurrentItemChangedEventArgs e)
        {
            CalTotalMoney();
        }

        private void chePaid_Checked(object sender, RoutedEventArgs e)
        {
            bAdd = true;
        }
    }
}
