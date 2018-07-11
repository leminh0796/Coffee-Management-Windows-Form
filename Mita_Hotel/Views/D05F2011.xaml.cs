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
using Mita_Hotel.Models;
using Mita_Hotel.BL;
using System.Data;

namespace Mita_Hotel.Views
{
    /// <summary>
    /// Interaction logic for D05F2011.xaml
    /// </summary>
    public partial class D05F2011 : L3Window
    {
        public D05F2011()
        {
            InitializeComponent();
        }
        public string TableID { get; set; }
        private EnumFormState _fromState = EnumFormState.FormAdd;
        public EnumFormState FormState
        {
            set
            {
                _fromState = value;
                switch (_fromState)
                {

                    case EnumFormState.FormAdd:
                        LoadAdd();
                        txtTableID.Focus();
                        break;
                    case EnumFormState.FormEdit:
                        LoadEdit();
                        txtTableID.IsEnabled = false;
                        txtTableName.Focus();
                        break;
                    default:
                        break;
                }
            }
        }
        bool AllowSave()
        {
            if (txtTableID.Text.Trim() == "")
            {
                D99D0041.D99C0008.Msg("Nhập mã bàn đi anh");
                txtTableID.Focus();
                return false;
            }
            if (txtTableName.Text.Trim() == "")
            {
                D99D0041.D99C0008.Msg("Hãy nhập tên bàn");
                txtTableName.Focus();
                return false;
            }
            if (txtPosition.Text.Trim() == "")
            {
                D99D0041.D99C0008.Msg("Hãy nhập vị trí");
                txtPosition.Focus();
                return false;
            }
            else return true;
        }
        private void LoadAdd()
        {
            _fromState = EnumFormState.FormAdd;
            txtTableID.EditValue = "";
            txtTableName.EditValue = "";
            txtPosition.EditValue = "";
        }
        private void LoadEdit()
        {
            DataTable dt = BLTable.GetTable(TableID);
            if (dt.Rows.Count > 0)
            {
                txtTableID.Text = dt.Rows[0]["TableID"].ToString();
                txtTableName.Text = dt.Rows[0]["TableName"].ToString();
                txtPosition.Text = dt.Rows[0]["Position"].ToString();
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            btnSave.Focus();
            if (!btnSave.IsFocused) return;
            if (!AllowSave()) return;
            Models.Table table = new Models.Table();
            table.TableID = txtTableID.Text;
            table.TableName = txtTableName.Text;
            table.Position = txtPosition.Text;
            bool bRet = false;
            btnSave.IsEnabled = false;

            if (_fromState == EnumFormState.FormAdd)
            {
                bRet = BLTable.AddTable(table);
            }
            else
            {
                bRet = BLTable.UpdateTable(table);
            }

            if (bRet)
            {
                Lemon3.Messages.L3Msg.SaveOK();
                TableID = txtTableID.Text;
                Close();
                btnSave.IsEnabled = true;
            }
            else
            {
                Lemon3.Messages.L3Msg.SaveNotOK();
                btnSave.IsEnabled = true;
            }
        }
    }
}
