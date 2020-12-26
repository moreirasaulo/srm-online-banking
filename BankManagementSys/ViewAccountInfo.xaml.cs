﻿using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BankManagementSys
{
    /// <summary>
    /// Interaction logic for ViewAccountInfo.xaml
    /// </summary>
    public partial class ViewAccountInfo : Window
    {
        User currentUser;
        Account currentAccount;
        public ViewAccountInfo(User user,Account account)
        {
            InitializeComponent();
            currentUser = user;
            currentAccount = account;
            comboHistory.ItemsSource = Utilities.transactionHistoryDays;
            comboHistory.SelectedIndex = 0;
            lblFullName.Content = user.FullName;
            lblDateOfBirth.Content = user.DateOfBirth.ToShortDateString();
            lblNatId.Content = user.NationalId;
            lblAccNo.Content = account.Id;
            lblOpenDate.Content = account.OpenDate.ToShortDateString();
            lblBalance.Content ="$ " + account.Balance;
            lblAccType.Content = account.AccountType.Description;
            lblInterestFeeDate.Content = account.InterestFeeDate.ToShortDateString();

            if(account.AccountType.Description == "Checking")
            {
                lblMonthlyFee.Content = "$ " + account.MonthlyFee;
                lblInterest.Content = "0 %";
                lblInterestDivid.Content = "Monthly interest:";
                lblInterestOrFee.Content = "Next fee date:";
            }
            else if (account.AccountType.Description == "Savings")
            {
                lblMonthlyFee.Content = "$ 0";
                lblInterestDivid.Content = "Interest:";
                lblInterest.Content = account.Interest + " %";
                lblInterestOrFee.Content = "Next interest date:";
            }
            else if (account.AccountType.Description == "Investment")
            {
                lblMonthlyFee.Content = "$ 0";
                lblInterestDivid.Content = "Monthly dividents:";
                lblInterest.Content = account.Interest + " %";
                lblInterestOrFee.Content = "Next dividents date:";
            }
        }

        private void btOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void SortTransactionsByTypeAndDate()
        {
            if(currentAccount == null)
            {
                return;
            }
            List<Transaction> accTransactions = currentAccount.Transactions.ToList();   //FIX exception
            if (comboHistory.SelectedIndex == 0)
            {
                accTransactions = accTransactions.FindAll(t => (DateTime.Now - t.Date).TotalDays <= 7);
                accTransactions = SortTransactionsByType(accTransactions);
            }
            else if (comboHistory.SelectedIndex == 1)
            {
                accTransactions = accTransactions.FindAll(t => (DateTime.Now - t.Date).TotalDays <= 30);
                accTransactions = SortTransactionsByType(accTransactions);
            }
            else if (comboHistory.SelectedIndex == 2)
            {
                accTransactions = accTransactions.FindAll(t => (DateTime.Now - t.Date).TotalDays <= 60);
                accTransactions = SortTransactionsByType(accTransactions);
            }
            else
            {
                accTransactions = accTransactions.FindAll(t => (DateTime.Now - t.Date).TotalDays <= 7);
                accTransactions = SortTransactionsByType(accTransactions);
            }
            lvTransactions.ItemsSource = accTransactions;
        }

        private List<Transaction> SortTransactionsByType(List<Transaction> list)
        {
            //by type
            if (rbTransactAll.IsChecked == true)
            {
                return list;
            }
            else if (rbTransacDeposits.IsChecked == true)
            {
                return list = list.Where(t => t.Type == "Deposit").ToList();
            }
            else if (rbTransacWithdrawals.IsChecked == true)
            {
                return list = list.Where(t => t.Type == "Withdrawal").ToList();
            }
            else if (rbTransacPayments.IsChecked == true)
            {
                return list = list.Where(t => t.Type == "Payment").ToList();
            }
            else if (rbTransacTransfers .IsChecked== true)
            {
                return list = list.Where(t => t.Type == "Transfer").ToList();
            }
            else
            {
                return list;
            }
        }

        private void comboHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortTransactionsByTypeAndDate();
            
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SortTransactionsByTypeAndDate();
        }

        private void CallTransactionDialog(string type)
        {
            TransactionDialog transacDlg = new TransactionDialog(currentUser, currentAccount, type);
            transacDlg.Owner = this;
            bool? result = transacDlg.ShowDialog();
            if (result == true)
            {
                SortTransactionsByTypeAndDate();
            }
        }

        //deposit
        private void btDeposit_Click(object sender, RoutedEventArgs e)
        {
            CallTransactionDialog("Deposit");
        }

        

        //withdrawal
        private void btWithdrawal_Click(object sender, RoutedEventArgs e)
        {
            CallTransactionDialog("Withdrawal");
        }

        //transfer
        private void btTransfer_Click(object sender, RoutedEventArgs e)
        {
            CallTransactionDialog("Transfer");
        }

        //payment
        private void btPayment_Click(object sender, RoutedEventArgs e)
        {
            CallTransactionDialog("Payment");
        }



        private void lvTransactions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(lvTransactions.Items.Count == 0 || lvTransactions.SelectedIndex == -1)
            {
                return;
            }
            Transaction currTrans = (Transaction)lvTransactions.SelectedItem;
           
                Receipt receiptDlg = new Receipt(currentAccount, 0, currTrans, currentUser, false);
                receiptDlg.Owner = this;
                receiptDlg.ShowDialog();
        }

        
    }
}
