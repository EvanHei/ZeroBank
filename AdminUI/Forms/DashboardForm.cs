﻿using AdminLibrary;
using SharedLibrary;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminUI.Forms
{
    public partial class DashboardForm : Form
    {
        private List<Account> accounts = new();
        private Dictionary<int, string> keys = new();
        private List<User> users;
        private List<User> admins;
        private List<CiphertextTransaction> selectedAccountCiphertextTransactions;
        private List<PlaintextTransaction> selectedAccountPlaintextTransactions;
        private Account selectedAccount;
        private string selectedAccountKeyString = "";

        public DashboardForm()
        {
            InitializeComponent();

            // show the accounts
            SidebarListBox.SelectedIndex = 0;
        }

        private async Task GetServerData()
        {
            try
            {
                accounts = await AdminConfig.ApiAccessor.GetAccounts();
                keys = await AdminConfig.ApiAccessor.GetKeys();
                users = await AdminConfig.ApiAccessor.GetUsers();
                admins = await AdminConfig.ApiAccessor.GetAdmins();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            this.Refresh();
        }

        private void GetSelectedAccountData(Account account)
        {
            selectedAccount = accounts.Where(a => a.Id == account.Id).FirstOrDefault();

            if (keys.TryGetValue(selectedAccount.Id, out selectedAccountKeyString))
            {
                selectedAccountPlaintextTransactions = AdminConfig.GetPlaintextTransactions(selectedAccount, selectedAccountKeyString).OrderByDescending(transaction => transaction.Timestamp).ToList();
                selectedAccountCiphertextTransactions = null;
            }
            else
            {
                selectedAccountCiphertextTransactions = selectedAccount.Transactions.OrderByDescending(transaction => transaction.Timestamp).ToList();
                selectedAccountPlaintextTransactions = null;
            }

            this.Refresh();
        }

        private void RootPanel_Paint(object sender, PaintEventArgs e)
        {
            // draw the line separating the sidebar
            using Pen pen = new(Color.FromArgb(79, 79, 79), 1);
            int xPosition = 194;
            e.Graphics.DrawLine(pen, new Point(xPosition, 0), new Point(xPosition, this.ClientSize.Height));
        }

        #region Sidebar

        private void SidebarListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            string text = SidebarListBox.Items[e.Index].ToString();
            Color backgroundColor;

            if (e.State.HasFlag(DrawItemState.Selected))
            {
                backgroundColor = Color.FromArgb(79, 79, 79);
            }
            else
            {
                backgroundColor = Color.FromArgb(45, 45, 45);
            }

            using SolidBrush backgroundBrush = new(backgroundColor);
            e.Graphics.FillRectangle(backgroundBrush, e.Bounds);

            SizeF textSize = e.Graphics.MeasureString(text, e.Font);
            float textY = e.Bounds.Y + (e.Bounds.Height - textSize.Height) / 2;

            using SolidBrush foregroundBrush = new(e.ForeColor);
            e.Graphics.DrawString(text, e.Font, foregroundBrush, e.Bounds.X, textY);
        }

        private async void SidebarListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;

            if (listBox == null || listBox.SelectedIndex == -1)
            {
                return;
            }

            string selectedItem = listBox.SelectedItem.ToString();

            if (selectedItem == "👥 Add Admin")
            {
                ShowCreateAdminPanel();
            }
            else if (selectedItem == "💳 Accounts")
            {
                await ShowAccountsPanel();
            }
        }

        private async void SidebarRefreshLabel_Click(object sender, EventArgs e)
        {
            await GetServerData();

            string selectedItem = SidebarListBox.SelectedItem.ToString();
            if (selectedItem == "💳 Accounts")
            {
                await ShowAccountsPanel();
            }
        }

        #endregion

        #region AccountsPanel

        private async Task ShowAccountsPanel()
        {
            if (accounts.Count == 0)
            {
                await GetServerData();
            }

            AccountsPanelListBox.DataSource = accounts;

            CreateAdminPanel.Visible = false;
            AccountsPanel.Visible = true;
            AccountDetailsPanel.Visible = false;

            AccountsPanelListBox.SelectedIndex = AccountsPanelListBox.Items.Count > 0 ? 0 : -1;
        }

        private void AccountsPanelAccountsPictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;

            // draw "Accounts"
            string smallText = "Accounts";

            using Font smallTextFont = new("Segoe UI Emoji", 12, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF smallTextSize = e.Graphics.MeasureString(smallText, smallTextFont);

            float smallTextX = (pictureBox.ClientSize.Width - smallTextSize.Width) / 2;
            float smallTextY = (pictureBox.ClientSize.Height - smallTextSize.Height) / 2;

            smallTextX += -15; // shift left
            smallTextY += -15; // shift up

            e.Graphics.DrawString(smallText, smallTextFont, brush, new PointF(smallTextX, smallTextY));

            // draw number of accounts
            string largeText = accounts.Count.ToString();

            using Font largeTextFont = new("Segoe UI", 16, FontStyle.Regular, GraphicsUnit.Point);
            SizeF largeTextSize = e.Graphics.MeasureString(largeText, largeTextFont);

            // same location initially
            float largeTextX = smallTextX;
            float largeTextY = smallTextY + 18; // shift down

            e.Graphics.DrawString(largeText, largeTextFont, brush, new PointF(smallTextX, largeTextY));
        }

        private void AccountsPanelRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (AccountsPanelAllRadioButton.Checked)
            {
                AccountsPanelListBox.DataSource = accounts;
            }
            else if (AccountsPanelOpenRadioButton.Checked)
            {
                AccountsPanelListBox.DataSource = accounts.Where(a => a.Closed == false).ToList();
            }
            else if (AccountsPanelClosedRadioButton.Checked)
            {
                AccountsPanelListBox.DataSource = accounts.Where(a => a.Closed == true).ToList();
            }
        }

        private void AccountsPanelViewDetailsPictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                selectedAccount = (Account)AccountsPanelListBox.SelectedItem;

                GetSelectedAccountData(selectedAccount);

                ShowAccountDetailsPanel();
            }
            catch (Exception ex)
            {
                AccountsPanelErrorLabel.Text = "Could not load the account's data";
            }
        }

        private void AccountsPanelViewDetailsPictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;

            string text = "View Details";

            using Font font = new("Segoe UI Emoji", 12, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            float x = (pictureBox.ClientSize.Width - textSize.Width) / 2;
            float y = (pictureBox.ClientSize.Height - textSize.Height) / 2;

            e.Graphics.DrawString(text, font, brush, new PointF(x, y));
        }

        private void AccountsPanelListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            Account account = (Account)AccountsPanelListBox.Items[e.Index];
            User user = users.Where(u => u.Id == account.UserId).FirstOrDefault();

            // get the background color if selected or not
            Color backgroundColor = e.State.HasFlag(DrawItemState.Selected) ? Color.FromArgb(163, 6, 64) : e.BackColor;

            // draw background
            using SolidBrush backgroundBrush = new(backgroundColor);
            e.Graphics.FillRectangle(backgroundBrush, e.Bounds);

            // draw line to separate rows
            using Pen linePen = new(Color.Gray, 1);
            e.Graphics.DrawLine(linePen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);

            // draw account name
            using Font nameFont = new(e.Font.FontFamily, 12, FontStyle.Regular);
            SizeF nameSize = e.Graphics.MeasureString(account.Name, nameFont);
            float nameY = e.Bounds.Y + (e.Bounds.Height - nameSize.Height) / 2;
            e.Graphics.DrawString(account.Name, nameFont, Brushes.White, e.Bounds.X + 5, nameY);

            // draw username
            using Font typeFont = new(e.Font.FontFamily, 12, FontStyle.Regular);
            SizeF typeTextSize = e.Graphics.MeasureString(user.Username, typeFont);
            float typeX = e.Bounds.Right - typeTextSize.Width - 5;
            e.Graphics.DrawString(user.Username, typeFont, Brushes.White, typeX, e.Bounds.Y);

            // draw date created
            string dateCreatedText = "Created " + account.DateCreated.ToShortDateString();
            using Font dateCreatedFont = new(e.Font.FontFamily, 8, FontStyle.Regular);
            using SolidBrush dateCreatedBrush = new(Color.FromArgb(145, 145, 145));
            SizeF dateCreatedTextSize = e.Graphics.MeasureString(dateCreatedText, dateCreatedFont);
            float dateCreatedX = e.Bounds.Right - dateCreatedTextSize.Width - 5;
            e.Graphics.DrawString(dateCreatedText, dateCreatedFont, dateCreatedBrush, dateCreatedX, e.Bounds.Y + 22);
        }

        private void AccountsPanelListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 40;
        }

        #endregion

        #region AccountDetailsPanel

        private void ShowAccountDetailsPanel()
        {
            if (!string.IsNullOrWhiteSpace(selectedAccountKeyString))
            {
                AccountDetailsPanelTransactionsListBox.DataSource = selectedAccountPlaintextTransactions;
            }
            else
            {
                AccountDetailsPanelTransactionsListBox.DataSource = selectedAccountCiphertextTransactions;
            }

            CreateAdminPanel.Visible = false;
            AccountsPanel.Visible = false;
            AccountDetailsPanel.Visible = true;
            AccountDetailsPanelErrorLabel.Text = "";
        }

        private async void AccountDetailsPanelBackArrowPictureBox_Click(object sender, EventArgs e)
        {
            await ShowAccountsPanel();
        }

        private void AccountDetailsPanelNamePictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (selectedAccount == null)
            {
                return;
            }

            PictureBox pictureBox = (PictureBox)sender;

            // draw "Name"
            string smallText = "Name";

            using Font smallTextFont = new("Segoe UI Emoji", 10, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF smallTextSize = e.Graphics.MeasureString(smallText, smallTextFont);

            float smallTextX = (pictureBox.ClientSize.Width - smallTextSize.Width) / 2;
            float smallTextY = (pictureBox.ClientSize.Height - smallTextSize.Height) / 2;

            smallTextX += -42; // shift left
            smallTextY += -15; // shift up

            e.Graphics.DrawString(smallText, smallTextFont, brush, new PointF(smallTextX, smallTextY));

            // draw actual name of account
            string largeText = selectedAccount.Name;

            using Font largeTextFont = new("Segoe UI Emoji", 16, FontStyle.Regular, GraphicsUnit.Point);
            SizeF largeTextSize = e.Graphics.MeasureString(largeText, largeTextFont);

            // same location initially
            float largeTextX = smallTextX;
            float largeTextY = smallTextY + 14;  // shift down

            e.Graphics.DrawString(largeText, largeTextFont, brush, new PointF(smallTextX, largeTextY));
        }

        private void AccountDetailsPanelDateCreatedPictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (selectedAccount == null)
            {
                return;
            }

            PictureBox pictureBox = (PictureBox)sender;

            // draw "Date Created"
            string smallText = "Date Created";

            using Font smallTextFont = new("Segoe UI Emoji", 10, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF smallTextSize = e.Graphics.MeasureString(smallText, smallTextFont);

            float smallTextX = (pictureBox.ClientSize.Width - smallTextSize.Width) / 2;
            float smallTextY = (pictureBox.ClientSize.Height - smallTextSize.Height) / 2;

            smallTextX += -5; // shift left
            smallTextY += -15; // shift up

            e.Graphics.DrawString(smallText, smallTextFont, brush, new PointF(smallTextX, smallTextY));

            // draw date created
            string largeText = selectedAccount.DateCreated.ToString("MM/dd/yyyy");

            using Font largeTextFont = new("Segoe UI Emoji", 16, FontStyle.Regular, GraphicsUnit.Point);
            SizeF largeTextSize = e.Graphics.MeasureString(largeText, largeTextFont);

            // same location initially
            float largeTextX = smallTextX;
            float largeTextY = smallTextY + 14; // shift down

            e.Graphics.DrawString(largeText, largeTextFont, brush, new PointF(smallTextX, largeTextY));
        }

        private void AccountDetailsPanelStatusPictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (selectedAccount == null)
            {
                return;
            }

            PictureBox pictureBox = (PictureBox)sender;

            // draw "Status"
            string smallText = "Status";

            using Font smallTextFont = new("Segoe UI Emoji", 10, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF smallTextSize = e.Graphics.MeasureString(smallText, smallTextFont);

            float smallTextX = (pictureBox.ClientSize.Width - smallTextSize.Width) / 2;
            float smallTextY = (pictureBox.ClientSize.Height - smallTextSize.Height) / 2;

            smallTextX -= 33; // shift n pixels left
            smallTextY -= 15; // shift m pixels up

            e.Graphics.DrawString(smallText, smallTextFont, brush, new PointF(smallTextX, smallTextY));

            // draw open/closed
            string largeText = selectedAccount.Closed ? "Closed" : "Open";

            using Font largeTextFont = new("Segoe UI Emoji", 16, FontStyle.Regular, GraphicsUnit.Point);
            SizeF largeTextSize = e.Graphics.MeasureString(largeText, largeTextFont);

            float largeTextX = smallTextX;
            float largeTextY = smallTextY + 14; // shift down

            e.Graphics.DrawString(largeText, largeTextFont, brush, new PointF(smallTextX, largeTextY));
        }

        private void AccountDetailsPanelMaxTransactionSizePictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (selectedAccount == null)
            {
                return;
            }

            PictureBox pictureBox = (PictureBox)sender;

            // draw "Max Transaction Size"
            string smallText = "Max Transaction Size";

            using Font smallTextFont = new("Segoe UI Emoji", 10, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF smallTextSize = e.Graphics.MeasureString(smallText, smallTextFont);

            float smallTextX = (pictureBox.ClientSize.Width - smallTextSize.Width) / 2;
            float smallTextY = (pictureBox.ClientSize.Height - smallTextSize.Height) / 2;

            smallTextX -= 33; // shift n pixels left
            smallTextY -= 15; // shift m pixels up

            e.Graphics.DrawString(smallText, smallTextFont, brush, new PointF(smallTextX, smallTextY));

            // draw dollar value
            string largeText = ((AdminConfig.LoadPlainModulus(selectedAccount) - 1) / 2 * .01).ToString("C");

            using Font largeTextFont = new("Segoe UI Emoji", 16, FontStyle.Regular, GraphicsUnit.Point);
            SizeF largeTextSize = e.Graphics.MeasureString(largeText, largeTextFont);

            float largeTextX = smallTextX;
            float largeTextY = smallTextY + 14; // shift down

            e.Graphics.DrawString(largeText, largeTextFont, brush, new PointF(smallTextX, largeTextY));
        }

        private void AccountDetailsPanelTransactionsPictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (selectedAccount == null)
            {
                return;
            }

            // get the number of transactions, either plaintext or ciphertext
            int transactionCount = 0;
            if (selectedAccountCiphertextTransactions != null)
            {
                transactionCount = selectedAccountCiphertextTransactions.Count;
            }
            else
            {
                transactionCount = selectedAccountPlaintextTransactions.Count;
            }

            PictureBox pictureBox = (PictureBox)sender;

            // draw "Transactions"
            string smallText = "Transactions";

            using Font smallTextFont = new("Segoe UI Emoji", 10, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF smallTextSize = e.Graphics.MeasureString(smallText, smallTextFont);

            float smallTextX = (pictureBox.ClientSize.Width - smallTextSize.Width) / 2;
            float smallTextY = (pictureBox.ClientSize.Height - smallTextSize.Height) / 2;

            smallTextX -= 58; // shift n pixels left
            smallTextY -= 15; // shift m pixels up

            e.Graphics.DrawString(smallText, smallTextFont, brush, new PointF(smallTextX, smallTextY));

            // draw transaction count
            string largeText = transactionCount.ToString();

            using Font largeTextFont = new("Segoe UI Emoji", 16, FontStyle.Regular, GraphicsUnit.Point);
            SizeF largeTextSize = e.Graphics.MeasureString(largeText, largeTextFont);

            float largeTextX = smallTextX;
            float largeTextY = smallTextY + 14; // shift down

            e.Graphics.DrawString(largeText, largeTextFont, brush, new PointF(smallTextX, largeTextY));
        }

        private void AccountDetailsPanelRangeOrBalancePictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (selectedAccount == null)
            {
                return;
            }

            PictureBox pictureBox = (PictureBox)sender;
            string smallText;
            string largeText;

            // if the key is known, display the balance
            if (!string.IsNullOrWhiteSpace(selectedAccountKeyString))
            {
                smallText = "Balance";
                largeText = AdminConfig.GetFormattedBalance(selectedAccountPlaintextTransactions);
            }
            // if the key is unknown, display the range
            else
            {
                smallText = "Range";
                largeText = "±" + (((AdminConfig.LoadPlainModulus(selectedAccount) - 1) / 2 * .01) * selectedAccountCiphertextTransactions.Count).ToString("C");
            }

            // draw "Range" / "Balance"
            using Font smallTextFont = new("Segoe UI Emoji", 10, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF smallTextSize = e.Graphics.MeasureString(smallText, smallTextFont);

            float smallTextX = (pictureBox.ClientSize.Width - smallTextSize.Width) / 2;
            float smallTextY = (pictureBox.ClientSize.Height - smallTextSize.Height) / 2;

            smallTextX -= 75; // shift n pixels left
            smallTextY -= 15; // shift m pixels up

            e.Graphics.DrawString(smallText, smallTextFont, brush, new PointF(smallTextX, smallTextY));

            // draw range / balance
            using Font largeTextFont = new("Segoe UI Emoji", 16, FontStyle.Regular, GraphicsUnit.Point);
            SizeF largeTextSize = e.Graphics.MeasureString(largeText, largeTextFont);

            float largeTextX = smallTextX;
            float largeTextY = smallTextY + 14; // shift down

            e.Graphics.DrawString(largeText, largeTextFont, brush, new PointF(smallTextX, largeTextY));
        }

        private void AccountDetailsPanelClosePictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;

            // draw "Close"
            string text = "Close";
            using Font font = new("Segoe UI Emoji", 16, FontStyle.Regular);
            using SolidBrush brush = new(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            float x = (pictureBox.Width - textSize.Width) / 2;
            float y = (pictureBox.Height - textSize.Height) / 2;

            e.Graphics.DrawString(text, font, brush, x, y);
        }

        private async void AccountDetailsPanelClosePictureBox_Click(object sender, EventArgs e)
        {
            // TODO: test
            try
            {
                await AdminConfig.ApiAccessor.CloseAccount(selectedAccount.Id);
            }
            catch (Exception ex)
            {
                AccountDetailsPanelErrorLabel.Text = "Could not close the account";
            }
        }

        private void AccountDetailsPanelTranasctionsListPictureBox_Paint(object sender, PaintEventArgs e)
        {
            // draw "History"
            string text = "History";

            using Font textFont = new("Segoe UI Emoji", 16, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, textFont);

            float textX = 12;  // n pixels from the left side
            float textY = 9;  // m pixels from the top

            e.Graphics.DrawString(text, textFont, brush, new PointF(textX, textY));
        }

        private void AccountDetailsPanelTransactionsListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            // get the background color if selected or not
            Color backgroundColor = e.State.HasFlag(DrawItemState.Selected) ? Color.FromArgb(163, 6, 64) : e.BackColor;

            // draw background
            using SolidBrush backgroundBrush = new(backgroundColor);
            e.Graphics.FillRectangle(backgroundBrush, e.Bounds);

            // draw line to separate rows
            using Pen linePen = new(Color.Gray, 1);
            e.Graphics.DrawLine(linePen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);

            string timestamp;
            string type;
            string amount;

            // if the key is known, display deposit / withdrawal and the amount
            if (!string.IsNullOrWhiteSpace(selectedAccountKeyString))
            {
                PlaintextTransaction plaintextTransaction = (PlaintextTransaction)AccountDetailsPanelTransactionsListBox.Items[e.Index];
                timestamp = plaintextTransaction.Timestamp.ToString();
                type = plaintextTransaction.Amount < 0 ? "Withdrawal" : "Deposit";
                amount = plaintextTransaction.FormattedAmount;
            }
            // if the key is unknown, display ***** and $$$
            else
            {
                CiphertextTransaction ciphertextTransaction = (CiphertextTransaction)AccountDetailsPanelTransactionsListBox.Items[e.Index];
                timestamp = ciphertextTransaction.Timestamp.ToString();
                type = "*****";
                amount = "$$$";
            }

            // draw type
            using Font typeFont = new(e.Font.FontFamily, 12, FontStyle.Regular);
            SizeF typeSize = e.Graphics.MeasureString(type, typeFont);
            float typeY = e.Bounds.Y + (e.Bounds.Height - typeSize.Height) / 2;
            e.Graphics.DrawString(type, typeFont, Brushes.White, e.Bounds.X + 5, typeY);

            // draw date
            using Font dateFont = new(e.Font.FontFamily, 8, FontStyle.Regular);
            using SolidBrush dateBrush = new(Color.FromArgb(145, 145, 145));
            SizeF dateTextSize = e.Graphics.MeasureString(timestamp, dateFont);
            float dateX = e.Bounds.Right - dateTextSize.Width - 5;
            e.Graphics.DrawString(timestamp, dateFont, dateBrush, dateX, e.Bounds.Y + 22);

            // draw amount
            using Font amountFont = new(e.Font.FontFamily, 12, FontStyle.Regular);
            SizeF amountTextSize = e.Graphics.MeasureString(amount, typeFont);
            float amountX = e.Bounds.Right - amountTextSize.Width - 5;
            e.Graphics.DrawString(amount, amountFont, Brushes.White, amountX, e.Bounds.Y);
        }

        private void AccountDetailsPanelTransactionsListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 40;
        }

        #endregion

        #region CreateAdminPanel

        private void ShowCreateAdminPanel()
        {
            CreateAdminPanel.Visible = true;
            AccountsPanel.Visible = false;
            AccountDetailsPanel.Visible = false;
        }

        private void CreateAdminPanelCreatePictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;

            string text = "Create";

            using Font font = new("Segoe UI Emoji", 12, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            float x = (pictureBox.ClientSize.Width - textSize.Width) / 2;
            float y = (pictureBox.ClientSize.Height - textSize.Height) / 2;

            e.Graphics.DrawString(text, font, brush, new PointF(x, y));
        }

        private async void CreateAdminPanelCreatePictureBox_Click(object sender, EventArgs e)
        {
            if (!ValidateCreateAccountPanelFields())
            {
                return;
            }

            try
            {
                Credentials newAdminCredentials = new(CreateAdminPanelUsernameTextBox.Text, CreateAdminPanelPasswordTextBox.Text);
                await AdminConfig.AdminCreate(newAdminCredentials);

                // clear fields
                CreateAdminPanelUsernameTextBox.Text = "";
                CreateAdminPanelPasswordTextBox.Text = "";

                await GetServerData();
                SidebarListBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                CreateAdminPanelErrorLabel.Text = "An error occurred while creating the admin";
            }
        }

        private bool ValidateCreateAccountPanelFields()
        {
            bool output = true;

            if (string.IsNullOrWhiteSpace(CreateAdminPanelUsernameTextBox.Text))
            {
                CreateAdminPanelErrorLabel.Text = "No username provided";
                output = false;
            }
            if (admins.Any(a => a.Username == CreateAdminPanelUsernameTextBox.Text))
            {
                CreateAdminPanelErrorLabel.Text = "An admin with this username already exists";
                output = false;
            }
            // TODO: ensure this meets the minimum password requirements
            if (string.IsNullOrWhiteSpace(CreateAdminPanelPasswordTextBox.Text))
            {
                CreateAdminPanelErrorLabel.Text = "No password provided";
                output = false;
            }

            return output;
        }

        #endregion
    }
}
