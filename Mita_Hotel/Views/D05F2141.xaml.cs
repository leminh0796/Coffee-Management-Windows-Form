using Lemon3.Controls.DevExp;
using Lemon3.Functions;
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
    /// Interaction logic for D05F2141.xaml
    /// </summary>
    public partial class D05F2141 : L3Window
    {
        public D05F2141()
        {
            InitializeComponent();
        }
        public decimal TotalMoney { get; set; }

        private bool AllowSave()
        {
            if (seAmountPayment.Text == "" || seAmountPayment.Text == "0" || seAmountPayment.Value < TotalMoney)
            {
                D99D0041.D99C0008.MsgNotYetEnter("Tiền khách trả");
                seAmountPayment.Focus();
                return false;
            }
            return true;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            decimal exchange = seAmountPayment.Value - TotalMoney;
            btnOK.Focus();
            if (!btnOK.IsFocused) return;
            if (!AllowSave()) return;
            MessageBox.Show("Tiền thối lại: " + L3ConvertType.Number(exchange.ToString()));
            TotalMoney = seAmountPayment.Value;
            Close();
        }

        private void frmPayment_Loaded(object sender, RoutedEventArgs e)
        {
            seAmountPayment.InputNumber288("n0", false, false);
            seAmountPayment.EditValue = TotalMoney;
        }
    }
}
