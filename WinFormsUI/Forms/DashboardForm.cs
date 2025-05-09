﻿using ClientLibrary;
using SharedLibrary.Models;
using SharedLibrary;
using System.Diagnostics;
using System.Globalization;
using System.Security.Policy;
using System.Transactions;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClientUI.Forms
{
    public partial class DashboardForm : Form
    {
        private List<Account> accounts = new();
        private string selectedAccountPassword;
        private Account selectedAccount;
        private long selectedAccountBalance;
        private List<PlaintextTransaction> selectedAccountPlaintextTransactions;

        public DashboardForm()
        {
            InitializeComponent();

            // show the dashboard
            SidebarListBox.SelectedIndex = 0;
        }

        private async Task GetServerData()
        {
            // TODO: display error msg
            try
            {
                accounts = await ClientConfig.ApiAccessor.GetAccounts();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            this.Refresh();
        }

        private async Task GetSelectedAccountData(Account account, string password)
        {
            selectedAccount = accounts.Where(a => a.Id == account.Id).FirstOrDefault();
            selectedAccountPassword = password;

            // order transactions by date (most recent first)
            List<PlaintextTransaction> plaintextTransactions = await ClientConfig.GetPlaintextTransactions(selectedAccount.Id, selectedAccountPassword);
            selectedAccountPlaintextTransactions = plaintextTransactions.OrderByDescending(transaction => transaction.Timestamp).ToList();

            selectedAccountBalance = selectedAccountPlaintextTransactions.Sum(t => t.Amount);

            this.Refresh();
        }

        private async void DashboardForm_Load(object sender, EventArgs e)
        {
            await GetServerData();
        }

        private void RootPanel_Paint(object sender, PaintEventArgs e)
        {
            // draw the line separating the sidebar
            using Pen pen = new(Color.FromArgb(79, 79, 79), 1);
            int xPosition = 194;
            e.Graphics.DrawLine(pen, new Point(xPosition, 0), new Point(xPosition, this.ClientSize.Height));
        }

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

        private void SidebarListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;

            if (listBox == null || listBox.SelectedIndex == -1)
            {
                return;
            }

            string selectedItem = listBox.SelectedItem.ToString();
            if (selectedItem == "🏠 Dashboard")
            {
                ShowDashboardPanel();
            }
            else if (selectedItem == "💳 Accounts")
            {
                ShowAccountsPanel();
            }
        }

        #region DashboardPanel

        private void ShowDashboardPanel()
        {
            DashboardPanel.Visible = true;
            AccountsPanel.Visible = false;
            AccountDetailsPanel.Visible = false;
            TransactPanel.Visible = false;
            CreateAccountPanel.Visible = false;
        }

        private void DashboardPanelUserGuidePictureBox_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = Constants.GitHubReadmeUrl,
                UseShellExecute = true
            });
        }

        private void DashboardPanelUserGuideIconPictureBox_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = Constants.GitHubReadmeUrl,
                UseShellExecute = true
            });
        }

        private void DashboardPanelUserGuideLabel_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = Constants.GitHubReadmeUrl,
                    UseShellExecute = true
                });

            }
            catch (Exception ex)
            {
                DashboardPanelErrorLabel.Text = "An error occurred while opening the user guide";
            }
        }

        private void DashboardPanelCreateAccountIconPictureBox_Click(object sender, EventArgs e)
        {
            SidebarListBox.SelectedIndex = 1;
            ShowCreateAccountPanel();
        }

        private void DashboardPanelCreateAccountPictureBox_Click(object sender, EventArgs e)
        {
            SidebarListBox.SelectedIndex = 1;
            ShowCreateAccountPanel();
        }

        private void DashboardPanelCreateAccountLabel_Click(object sender, EventArgs e)
        {
            SidebarListBox.SelectedIndex = 1;
            ShowCreateAccountPanel();
        }

        #endregion

        #region AccountsPanel

        private void ShowAccountsPanel()
        {
            AccountsPanelListBox.DataSource = accounts;
            TransactPanelErrorLabel.Text = "";

            DashboardPanel.Visible = false;
            AccountsPanel.Visible = true;
            AccountDetailsPanel.Visible = false;
            TransactPanel.Visible = false;
            CreateAccountPanel.Visible = false;

            AccountsPanelPasswordTextBox.Focus();
            AccountsPanelListBox.SelectedIndex = AccountsPanelListBox.Items.Count > 0 ? 0 : -1;
        }

        private bool ValidateAccountsPanelFields()
        {
            bool output = true;

            if (string.IsNullOrWhiteSpace(AccountsPanelPasswordTextBox.Text))
            {
                output = false;
            }

            return output;
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

        private async void AccountsPanelPasswordArrowPictureBox_Click(object sender, EventArgs e)
        {
            if (!ValidateAccountsPanelFields())
            {
                return;
            }

            try
            {
                selectedAccountPassword = AccountsPanelPasswordTextBox.Text;
                selectedAccount = (Account)AccountsPanelListBox.SelectedItem;

                await GetSelectedAccountData(selectedAccount, selectedAccountPassword);

                ShowAccountDetailsPanel();
            }
            catch (Exception ex)
            {
                AccountsPanelErrorLabel.Text = "Could not load the account's data";
            }
        }

        private void AccountsPanelListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            Account account = (Account)AccountsPanelListBox.Items[e.Index];

            // get the background color if selected or not
            Color backgroundColor = e.State.HasFlag(DrawItemState.Selected) ? Color.FromArgb(163, 6, 64) : e.BackColor;

            // draw background
            using SolidBrush backgroundBrush = new(backgroundColor);
            e.Graphics.FillRectangle(backgroundBrush, e.Bounds);

            // draw line to separate rows
            using Pen linePen = new(Color.Gray, 1);
            e.Graphics.DrawLine(linePen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);

            // draw name
            using Font nameFont = new(e.Font.FontFamily, 12, FontStyle.Regular);
            SizeF nameSize = e.Graphics.MeasureString(account.Name, nameFont);
            float nameY = e.Bounds.Y + (e.Bounds.Height - nameSize.Height) / 2;
            e.Graphics.DrawString(account.Name, nameFont, Brushes.White, e.Bounds.X + 5, nameY);

            // draw type
            using Font typeFont = new(e.Font.FontFamily, 12, FontStyle.Regular);
            SizeF typeTextSize = e.Graphics.MeasureString(account.Type.ToString(), typeFont);
            float typeX = e.Bounds.Right - typeTextSize.Width - 5;
            e.Graphics.DrawString(account.Type.ToString(), typeFont, Brushes.White, typeX, e.Bounds.Y);

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

        private void AccountsPanelCreateNewPictureBox_Click(object sender, EventArgs e)
        {
            ShowCreateAccountPanel();
        }

        private void AccountsPanelCreateNewPictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;

            string text = "Create New";

            using Font font = new("Segoe UI Emoji", 12, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            float x = (pictureBox.ClientSize.Width - textSize.Width) / 2;
            float y = (pictureBox.ClientSize.Height - textSize.Height) / 2;

            e.Graphics.DrawString(text, font, brush, new PointF(x, y));
        }

        private async void AccountsPanelDeletePictureBox_Click(object sender, EventArgs e)
        {
            Account account = (Account)AccountsPanelListBox.SelectedItem;

            if (account.Transactions.Count > 0)
            {
                AccountsPanelErrorLabel.Text = "Cannot delete an account with transactions";
                return;
            }

            try
            {
                await ClientConfig.CloseAccount(account.Id, selectedAccountPassword);
            }
            catch (Exception ex)
            {
                AccountsPanelErrorLabel.Text = "An error occurred while deleting the account";
            }

            await GetServerData();
            ShowAccountsPanel();
        }

        private void AccountsPanelDeletePictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;

            string text = "Delete";

            using Font font = new("Segoe UI Emoji", 12, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            float x = (pictureBox.ClientSize.Width - textSize.Width) / 2;
            float y = (pictureBox.ClientSize.Height - textSize.Height) / 2;

            e.Graphics.DrawString(text, font, brush, new PointF(x, y));
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

        private void AccountsPanelPasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    e.SuppressKeyPress = true;
                    AccountsPanelPasswordArrowPictureBox_Click(null, null);
                    break;
                case Keys.Up:
                    if (AccountsPanelListBox.SelectedIndex > 0)
                    {
                        AccountsPanelListBox.SelectedIndex--;
                    }
                    break;
                case Keys.Down:
                    if (AccountsPanelListBox.SelectedIndex < AccountsPanelListBox.Items.Count - 1)
                    {
                        AccountsPanelListBox.SelectedIndex++;
                    }
                    break;
            }
        }

        private void AccountsPanelListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AccountsPanelPasswordTextBox.Text = "";
            AccountsPanelPasswordTextBox.Focus();
        }

        #endregion

        #region AccountDetailsPanel

        private void ShowAccountDetailsPanel()
        {
            AccountDetailsPanelLastIntervalComboBox.SelectedIndex = 4;
            AccountDetailsPanelChartTypeComboBox.SelectedIndex = 0;
            AccountDetailsPanelTransactionsListBox.DataSource = selectedAccountPlaintextTransactions;
            AccountsPanelPasswordTextBox.Text = "";
            LoadDoughnutChart();
            LoadMainChart(DateTime.Now.AddYears(-1));

            if (selectedAccount.Closed == true)
            {
                AccountDetailsPanelClosePictureBox.Visible = false;
                AccountDetailsPanelTransactPictureBox.Visible = false;
            }
            else
            {
                AccountDetailsPanelClosePictureBox.Visible = true;
                AccountDetailsPanelTransactPictureBox.Visible = true;
            }

            DashboardPanel.Visible = false;
            AccountsPanel.Visible = false;
            AccountDetailsPanel.Visible = true;
            TransactPanel.Visible = false;
            CreateAccountPanel.Visible = false;
        }

        private void LoadMainChart(DateTime previousDate)
        {
            // dummy data for testing
            List<PlaintextTransaction> dummyTransactions = new List<PlaintextTransaction>
            {
                new PlaintextTransaction { Amount = 1000, AccountId = 1, Timestamp = DateTime.Now.AddDays(-1) },
                new PlaintextTransaction { Amount = 5000, AccountId = 1, Timestamp = DateTime.Now.AddDays(-5) },
                new PlaintextTransaction { Amount = 1000, AccountId = 1, Timestamp = DateTime.Now.AddDays(-10) },
                new PlaintextTransaction { Amount = 2000, AccountId = 1, Timestamp = DateTime.Now.AddDays(-15) },
                new PlaintextTransaction { Amount = 3000, AccountId = 1, Timestamp = DateTime.Now.AddDays(-20) },
                new PlaintextTransaction { Amount = -1000, AccountId = 1, Timestamp = DateTime.Now.AddDays(-25) },
                new PlaintextTransaction { Amount = 4000, AccountId = 1, Timestamp = DateTime.Now.AddDays(-30) },
                new PlaintextTransaction { Amount = 4000, AccountId = 1, Timestamp = DateTime.Now.AddDays(-40) },
                new PlaintextTransaction { Amount = 2000, AccountId = 1, Timestamp = DateTime.Now.AddDays(-50) },
                new PlaintextTransaction { Amount = 1000, AccountId = 1, Timestamp = DateTime.Now.AddDays(-60) },
                new PlaintextTransaction { Amount = -4000, AccountId = 1, Timestamp = DateTime.Now.AddDays(-70) },
                new PlaintextTransaction { Amount = 3000, AccountId = 1, Timestamp = DateTime.Now.AddDays(-80) },
                new PlaintextTransaction { Amount = 5000, AccountId = 1, Timestamp = DateTime.Now.AddDays(-90) },
                new PlaintextTransaction { Amount = 6000, AccountId = 1, Timestamp = DateTime.Now.AddDays(-100) },
                new PlaintextTransaction { Amount = 7000, AccountId = 1, Timestamp = DateTime.Now.AddDays(-110) },
                new PlaintextTransaction { Amount = 1000, AccountId = 1, Timestamp = DateTime.Now.AddDays(-365) }
            };

            // calculate the balance as it changes over time
            long cumulativeBalance = 0;

            // change to dummyTransactions for testing purposes, or back to selectedAccountPlaintextTransactions
            var balanceOverTime = selectedAccountPlaintextTransactions
                .OrderBy(pt => pt.Timestamp)
                .Select(pt =>
                {
                    cumulativeBalance += pt.Amount;
                    return new { pt.Timestamp, Balance = cumulativeBalance / 100.0 };
                })
                .ToList();


            // filter data within the time range
            DateTime currentDate = DateTime.Now;
            var filteredData = balanceOverTime
                .Where(data => data.Timestamp >= previousDate && data.Timestamp <= currentDate)
                .ToList();

            // find the data point just before the range
            var pointBeforeRange = balanceOverTime
                .Where(data => data.Timestamp < previousDate)
                .OrderByDescending(data => data.Timestamp)
                .FirstOrDefault();

            // add the point before the range (if it exists) to fill in the area before the transaction farthest back
            if (pointBeforeRange != null)
            {
                filteredData.Insert(0, pointBeforeRange);
            }

            // extrapolate the most recent balance to the current date to fill in the area after the lastest transaction
            if (filteredData.Count != 0)
            {
                var lastDataPoint = filteredData.Last();

                // add today's date with the most recent balance
                filteredData.Add(new { Timestamp = currentDate, Balance = lastDataPoint.Balance });
            }

            // remove the existing chart
            AccountDetailsPanelMainChartPanel.Controls.Clear();

            // create a new chart
            Chart chart = new();
            chart.DataSource = filteredData;
            chart.Dock = DockStyle.Fill;
            chart.BackColor = Color.FromArgb(79, 79, 79);

            // define chart area
            ChartArea chartArea = new();
            chartArea.AxisX.Minimum = previousDate.ToOADate();
            chartArea.AxisX.Maximum = currentDate.ToOADate();
            chartArea.BackColor = Color.FromArgb(79, 79, 79);
            chartArea.AxisX.LabelStyle.ForeColor = Color.FromArgb(145, 145, 145);
            chartArea.AxisX.LineWidth = 0;
            chartArea.AxisX.MajorGrid.LineWidth = 0;
            chartArea.AxisX.MajorTickMark.LineColor = Color.FromArgb(145, 145, 145);
            chartArea.AxisX.IsMarginVisible = false;
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(145, 145, 145);
            chartArea.AxisY.MajorTickMark.LineColor = Color.FromArgb(145, 145, 145);
            chartArea.AxisY.LineWidth = 0;
            chartArea.AxisY.MajorTickMark.LineColor = Color.FromArgb(145, 145, 145);
            chartArea.AxisY.LabelStyle.ForeColor = Color.FromArgb(145, 145, 145);
            chartArea.AxisY.LabelStyle.Format = "$#,##0.00;- $#,##0.00";

            // get the time range and format the x-axis labels
            TimeSpan timeRange = currentDate - previousDate;
            if (timeRange.TotalHours <= 25)
            {
                // ex. 3:00 PM for ranges of 1 hour or day
                chartArea.AxisX.LabelStyle.Format = "hh:mm tt";
            }
            else
            {
                // ex. Mar 1 for ranges of 1 week, month, or year
                chartArea.AxisX.LabelStyle.Format = "MMM d";
            }

            // define series
            Series series = new();
            series.ChartType = SeriesChartType.Area;
            series.MarkerStyle = MarkerStyle.Circle;
            series.BorderWidth = 2;
            series.Color = Color.White;
            series.BackSecondaryColor = Color.SteelBlue;
            series.BackGradientStyle = GradientStyle.LeftRight;
            series.BorderDashStyle = ChartDashStyle.Solid;
            series.XValueMember = "Timestamp";
            series.YValueMembers = "Balance";

            // add controls
            chart.ChartAreas.Add(chartArea);
            chart.Series.Add(series);
            AccountDetailsPanelMainChartPanel.Controls.Add(chart);
        }

        private void LoadDoughnutChart()
        {
            // get number of deposits and withdrawals
            int numberOfDeposits = selectedAccountPlaintextTransactions.Count(pt => pt.Amount > 0);
            int numberOfWithdrawals = selectedAccountPlaintextTransactions.Count(pt => pt.Amount <= 0);

            // remove the existing chart
            AccountDetailsPanelDoughnutChartPanel.Controls.Clear();

            // create a new chart
            Chart chart = new();
            chart.Dock = DockStyle.Fill;
            chart.BackColor = Color.FromArgb(79, 79, 79);

            // define chart area
            ChartArea chartArea = new();
            chartArea.BackColor = Color.FromArgb(79, 79, 79);

            // define series
            Series series = new();
            series.IsValueShownAsLabel = true;
            series.ChartType = SeriesChartType.Doughnut;
            series.BorderWidth = 8;
            series.BorderColor = Color.FromArgb(79, 79, 79);
            series.BackSecondaryColor = Color.FromArgb(197, 197, 197);
            series.BackGradientStyle = GradientStyle.DiagonalRight;
            series["PieLabelStyle"] = "Inside";
            series["PieStartAngle"] = "270";

            DataPoint depositPoint = new();
            depositPoint.SetValueXY("Deposits", numberOfDeposits);
            depositPoint.Color = Color.FromArgb(0, 112, 4);
            series.Points.Add(depositPoint);

            DataPoint withdrawalPoint = new();
            withdrawalPoint.SetValueXY("Withdrawals", numberOfWithdrawals);
            withdrawalPoint.Color = Color.FromArgb(176, 0, 0);
            series.Points.Add(withdrawalPoint);

            // define legend
            Legend legend = new();
            legend.Docking = Docking.Bottom;
            legend.Font = new("Segoe UI Emoji", 10, FontStyle.Regular);
            legend.BackColor = Color.FromArgb(79, 79, 79);
            legend.ForeColor = Color.White;

            // add controls
            chart.ChartAreas.Add(chartArea);
            chart.Series.Add(series);
            chart.Legends.Add(legend);
            AccountDetailsPanelDoughnutChartPanel.Controls.Add(chart);
        }

        private void AccountsDetailsPanelNamePictureBox_Paint(object sender, PaintEventArgs e)
        {
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

        private void AccountDetailsPanelTransactionsPictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;

            // draw "Transactions"
            string smallText = "Transactions";

            using Font smallTextFont = new("Segoe UI Emoji", 10, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF smallTextSize = e.Graphics.MeasureString(smallText, smallTextFont);

            float smallTextX = (pictureBox.ClientSize.Width - smallTextSize.Width) / 2;
            float smallTextY = (pictureBox.ClientSize.Height - smallTextSize.Height) / 2;

            smallTextX += -25; // shift left
            smallTextY += -15; // shift up

            e.Graphics.DrawString(smallText, smallTextFont, brush, new PointF(smallTextX, smallTextY));

            // draw number of transactions
            string largeText = selectedAccountPlaintextTransactions.Count.ToString();

            using Font largeTextFont = new("Segoe UI Emoji", 16, FontStyle.Regular, GraphicsUnit.Point);
            SizeF largeTextSize = e.Graphics.MeasureString(largeText, largeTextFont);

            // same location initially
            float largeTextX = smallTextX;
            float largeTextY = smallTextY + 14; // shift down

            e.Graphics.DrawString(largeText, largeTextFont, brush, new PointF(smallTextX, largeTextY));
        }

        private void AccountDetailsPanelStatusPictureBox_Paint(object sender, PaintEventArgs e)
        {
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

        private void AccountDetailsPanelTransactionsListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            PlaintextTransaction plaintextTransaction = (PlaintextTransaction)AccountDetailsPanelTransactionsListBox.Items[e.Index];

            // get the background color if selected or not
            Color backgroundColor = e.State.HasFlag(DrawItemState.Selected) ? Color.FromArgb(163, 6, 64) : e.BackColor;

            // draw background
            using SolidBrush backgroundBrush = new(backgroundColor);
            e.Graphics.FillRectangle(backgroundBrush, e.Bounds);

            // draw line to separate rows
            using Pen linePen = new(Color.Gray, 1);
            e.Graphics.DrawLine(linePen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);

            // draw type
            string type = "Deposit";
            if (plaintextTransaction.Amount < 0)
            {
                type = "Withdrawal";
            }

            using Font typeFont = new(e.Font.FontFamily, 12, FontStyle.Regular);
            SizeF typeSize = e.Graphics.MeasureString(type, typeFont);
            float typeY = e.Bounds.Y + (e.Bounds.Height - typeSize.Height) / 2;
            e.Graphics.DrawString(type, typeFont, Brushes.White, e.Bounds.X + 5, typeY);

            // draw amount
            using Font amountFont = new(e.Font.FontFamily, 12, FontStyle.Regular);
            SizeF amountTextSize = e.Graphics.MeasureString(plaintextTransaction.FormattedAmount, typeFont);
            float amountX = e.Bounds.Right - amountTextSize.Width - 5;
            e.Graphics.DrawString(plaintextTransaction.FormattedAmount, amountFont, Brushes.White, amountX, e.Bounds.Y);

            // draw date
            using Font dateFont = new(e.Font.FontFamily, 8, FontStyle.Regular);
            using SolidBrush dateBrush = new(Color.FromArgb(145, 145, 145));
            SizeF dateTextSize = e.Graphics.MeasureString(plaintextTransaction.Timestamp.ToString(), dateFont);
            float dateX = e.Bounds.Right - dateTextSize.Width - 5;
            e.Graphics.DrawString(plaintextTransaction.Timestamp.ToString(), dateFont, dateBrush, dateX, e.Bounds.Y + 22);
        }

        private void AccountDetailsPanelTransactionsListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 40;
        }

        private void AccountDetailsPanelTranasctionsListPictureBox_Paint(object sender, PaintEventArgs e)
        {
            // draw "History"
            string text = "History";

            using Font textFont = new("Segoe UI Emoji", 16, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, textFont);

            float textX = 12;  // n pixels from the left side
            float textY = 7;  // m pixels from the top

            e.Graphics.DrawString(text, textFont, brush, new PointF(textX, textY));
        }

        private void AccountDetailsPanelDoughnutChartPictureBox_Paint(object sender, PaintEventArgs e)
        {
            // draw "Transactions"
            string transactionsText = "Transactions";

            using Font transactionsFont = new("Segoe UI Emoji", 16, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush transactionsBrush = new(Color.White);
            SizeF transactionsSize = e.Graphics.MeasureString(transactionsText, transactionsFont);

            float transactionsTextX = 12;  // n pixels from the left side
            float transactionsTextY = 7;  // m pixels from the top

            e.Graphics.DrawString(transactionsText, transactionsFont, transactionsBrush, new PointF(transactionsTextX, transactionsTextY));

            // draw balance
            NumberFormatInfo customFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            customFormat.CurrencyNegativePattern = 1;
            string balanceText = (selectedAccountBalance * .01).ToString("C", customFormat);

            using Font balanceFont = new("Segoe UI Emoji", 16, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush balanceBrush = new(Color.White);
            SizeF balanceSize = e.Graphics.MeasureString(transactionsText, transactionsFont);

            float balanceX = ((sender as Control).ClientSize.Width / 2 - balanceSize.Width / 2) + 30;
            float balanceY = 350;  // m pixels from the top

            e.Graphics.DrawString(balanceText, balanceFont, balanceBrush, new PointF(balanceX, balanceY));
        }

        private void AccountDetailsPanelBackArrowPictureBox_Click(object sender, EventArgs e)
        {
            ShowAccountsPanel();
        }

        private void AccountDetailsPanelLastIntervalPictureBox_Click(object sender, EventArgs e)
        {
            AccountDetailsPanelLastIntervalComboBox.DroppedDown = true;
        }

        private void AccountDetailsPanelLastIntervalPictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;

            // draw time interval
            string text = (string)AccountDetailsPanelLastIntervalComboBox.SelectedItem;
            using Font font = new("Segoe UI Emoji", 12, FontStyle.Regular);
            using SolidBrush brush = new(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            float x = 10;
            float y = (pictureBox.Height - textSize.Height) / 2;

            e.Graphics.DrawString(text, font, brush, x, y);
        }

        private void AccountDetailsPanelLastIntervalComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AccountDetailsPanelLastIntervalPictureBox.Invalidate();

            string selectedItem = (string)AccountDetailsPanelLastIntervalComboBox.SelectedItem;
            if (selectedItem == "Hour")
            {
                LoadMainChart(DateTime.Now.AddHours(-1));
            }
            else if (selectedItem == "Day")
            {
                LoadMainChart(DateTime.Now.AddDays(-1));
            }
            else if (selectedItem == "Week")
            {
                LoadMainChart(DateTime.Now.AddDays(-7));
            }
            else if (selectedItem == "Month")
            {
                LoadMainChart(DateTime.Now.AddMonths(-1));
            }
            else if (selectedItem == "Year")
            {
                LoadMainChart(DateTime.Now.AddYears(-1));
            }

            AccountDetailsPanelChartTypeComboBox_SelectedIndexChanged(null, null);
        }

        private void AccountDetailsPanelChartTypePictureBox_Click(object sender, EventArgs e)
        {
            AccountDetailsPanelChartTypeComboBox.DroppedDown = true;
        }

        private void AccountDetailsPanelChartTypePictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;

            string text = (string)AccountDetailsPanelChartTypeComboBox.SelectedItem;
            using Font font = new("Segoe UI Emoji", 12, FontStyle.Regular);
            using SolidBrush brush = new(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            float x = 10;
            float y = (pictureBox.Height - textSize.Height) / 2;

            e.Graphics.DrawString(text, font, brush, x, y);
        }

        private void AccountDetailsPanelChartTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AccountDetailsPanelChartTypePictureBox.Invalidate();

            string selectedItem = (string)AccountDetailsPanelChartTypeComboBox.SelectedItem;

            Chart chart = AccountDetailsPanelMainChartPanel.Controls.OfType<Chart>().FirstOrDefault();
            if (chart == null)
            {
                return;
            }

            if (selectedItem == "Area")
            {
                chart.Series[0].ChartType = SeriesChartType.Area;
            }
            else if (selectedItem == "Line")
            {
                chart.Series[0].ChartType = SeriesChartType.Line;
            }
            else if (selectedItem == "Point")
            {
                chart.Series[0].ChartType = SeriesChartType.Point;
                chart.Series[0].MarkerStyle = MarkerStyle.Circle;
                AccountDetailsPanelPointsCheckBox.Checked = true;
            }
        }

        private void AccountDetailsPanelPointsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Chart chart = AccountDetailsPanelMainChartPanel.Controls.OfType<Chart>().FirstOrDefault();
            if (chart == null)
            {
                return;
            }

            if (AccountDetailsPanelPointsCheckBox.Checked == true)
            {
                chart.Series[0].MarkerStyle = MarkerStyle.Circle;
            }
            else
            {
                chart.Series[0].MarkerStyle = MarkerStyle.None;
            }
        }

        private void AccountDetailsPanelTransactPictureBox_Click(object sender, EventArgs e)
        {
            ShowTransactPanel();
        }

        private void AccountDetailsPanelTransactPictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;

            // draw "Transact"
            string text = "Transact";
            using Font font = new("Segoe UI Emoji", 16, FontStyle.Regular);
            using SolidBrush brush = new(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            float x = (pictureBox.Width - textSize.Width) / 2;
            float y = (pictureBox.Height - textSize.Height) / 2;

            e.Graphics.DrawString(text, font, brush, x, y);
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
            // TODO: add confirm check
            try
            {
                await ClientConfig.CloseAccount(selectedAccount.Id, selectedAccountPassword);
                await GetServerData();
                await GetSelectedAccountData(selectedAccount, selectedAccountPassword);
            }
            catch (Exception ex)
            {
                TransactPanelErrorLabel.Text = "An error occurred while closing the account";
            }
        }

        #endregion

        #region TransactPanel

        private void ShowTransactPanel()
        {
            TransactPanelDepositLabel_Click(null, null);
            TransactPanelAmountTextBox.Text = "";
            TransactPanelRangeValueLabel.Text = (ClientConfig.GetMaxAmount(selectedAccount.Id) * 0.01).ToString("C");
            TransactPanelErrorLabel.Text = "";

            DashboardPanel.Visible = false;
            AccountsPanel.Visible = false;
            AccountDetailsPanel.Visible = false;
            TransactPanel.Visible = true;
            CreateAccountPanel.Visible = false;
        }

        private bool ValidateTransactPanelFields()
        {
            bool output = true;
            string amount = TransactPanelAmountTextBox.Text;

            if (string.IsNullOrWhiteSpace(amount))
            {
                output = false;
            }
            if (!double.TryParse(amount, out double value))
            {
                output = false;
            }
            int decimalPlaces = BitConverter.GetBytes(decimal.GetBits((decimal)value)[3])[2];
            if (decimalPlaces > 2)
            {
                output = false;
            }

            return output;
        }

        private void TransactPanelConfirmPictureBox_Paint(object sender, PaintEventArgs e)
        {
            string text = "Confirm";

            using Font font = new("Segoe UI Emoji", 12, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            float x = (TransactPanelConfirmPictureBox.ClientSize.Width - textSize.Width) / 2;
            float y = (TransactPanelConfirmPictureBox.ClientSize.Height - textSize.Height) / 2;

            e.Graphics.DrawString(text, font, brush, new PointF(x, y));
        }

        private async void TransactPanelConfirmPictureBox_Click(object sender, EventArgs e)
        {
            if (!ValidateTransactPanelFields())
            {
                TransactPanelErrorLabel.Text = "Amount is not in correct format";
                return;
            }

            try
            {
                // TODO: make it easier to input an amount and fix bug where a value like $.23 results in $23
                double.TryParse(TransactPanelAmountTextBox.Text, out double amountDouble);

                // if it's a withdrawal, make negative
                if (TransactPanelWithdrawPictureBox.Visible == true)
                {
                    amountDouble *= -1;
                }

                long amountLong = (long)(amountDouble * 100);

                await ClientConfig.AddTransaction(selectedAccount.Id, amountLong, selectedAccountPassword);
                await GetSelectedAccountData(selectedAccount, selectedAccountPassword);

                TransactPanelAmountTextBox.Text = "";
                ShowAccountDetailsPanel();
            }
            catch (Exception ex)
            {
                TransactPanelErrorLabel.Text = "An error occurred; your account may have been administratively closed";
            }
        }

        private void TransactPanelDepositLabel_Click(object sender, EventArgs e)
        {
            TransactPanelDepositPictureBox.Visible = true;
            TransactPanelWithdrawPictureBox.Visible = false;
            TransactPanelDepositLabel.BackColor = Color.FromArgb(79, 79, 79);
            TransactPanelWithdrawLabel.BackColor = Color.FromArgb(45, 45, 45);
        }

        private void TransactPanelWithdrawLabel_Click(object sender, EventArgs e)
        {
            TransactPanelDepositPictureBox.Visible = false;
            TransactPanelWithdrawPictureBox.Visible = true;
            TransactPanelDepositLabel.BackColor = Color.FromArgb(45, 45, 45);
            TransactPanelWithdrawLabel.BackColor = Color.FromArgb(79, 79, 79);
        }

        private void TransactPanelBackArrowPictureBox_Click(object sender, EventArgs e)
        {
            ShowAccountDetailsPanel();
        }

        private void TransactPanelAmountTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.SuppressKeyPress = true;
            TransactPanelConfirmPictureBox_Click(null, null);
        }

        #endregion

        #region CreateAccountPanel

        private void ShowCreateAccountPanel()
        {
            CreateAccountPanelTypeComboBox.SelectedIndex = 0;
            CreateAccountPanelErrorLabel.Text = "";

            DashboardPanel.Visible = false;
            AccountsPanel.Visible = false;
            AccountDetailsPanel.Visible = false;
            TransactPanel.Visible = false;
            AccountDetailsPanel.Visible = false;
            CreateAccountPanel.Visible = true;
        }

        private bool ValidateCreateAccountPanelFields()
        {
            bool output = true;

            if (string.IsNullOrWhiteSpace(CreateAccountPanelNameTextBox.Text))
            {
                CreateAccountPanelErrorLabel.Text = "No name provided";
                output = false;
            }
            if (accounts.Any(a => a.Name == CreateAccountPanelNameTextBox.Text))
            {
                CreateAccountPanelErrorLabel.Text = "An account with this name already exists";
                output = false;
            }
            if (CreateAccountPanelTypeComboBox.SelectedItem == null)
            {
                CreateAccountPanelErrorLabel.Text = "Invalid account type";
                output = false;
            }
            string selectedType = CreateAccountPanelTypeComboBox.SelectedItem.ToString();
            if (!Enum.TryParse(selectedType, out AccountType type))
            {
                CreateAccountPanelErrorLabel.Text = "Invalid account type";
                output = false;
            }
            // TODO: ensure this meets the minimum password requirements
            if (string.IsNullOrWhiteSpace(CreateAccountPanelPasswordTextBox.Text))
            {
                CreateAccountPanelErrorLabel.Text = "No password provided";
                output = false;
            }

            return output;
        }

        private void CreateAccountPanelTypePictureBox_Click(object sender, EventArgs e)
        {
            CreateAccountPanelTypeComboBox.DroppedDown = true;
        }

        private async void CreateAccountPanelCreatePictureBox_Click(object sender, EventArgs e)
        {
            if (!ValidateCreateAccountPanelFields())
            {
                return;
            }

            Enum.TryParse(CreateAccountPanelTypeComboBox.SelectedItem.ToString(), out AccountType type);

            try
            {
                await ClientConfig.CreateAccount(CreateAccountPanelNameTextBox.Text,
                                                 type,
                                                 CreateAccountPanelPasswordTextBox.Text);

                // clear fields
                CreateAccountPanelNameTextBox.Text = "";
                CreateAccountPanelTypeComboBox.SelectedIndex = 0;
                CreateAccountPanelPasswordTextBox.Text = "";

                await GetServerData();
                ShowAccountsPanel();
            }
            catch (Exception ex)
            {
                CreateAccountPanelErrorLabel.Text = "An error occurred while creating the account";
            }
        }

        private void CreateAccountPanelCreatePictureBox_Paint(object sender, PaintEventArgs e)
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

        private void CreateAccountPanelTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateAccountPanelTypePictureBox.Invalidate();
        }

        private void CreateAccountPanelTypePictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;

            string text = (string)CreateAccountPanelTypeComboBox.SelectedItem;
            using Font font = new("Segoe UI Emoji", 12, FontStyle.Regular);
            using SolidBrush brush = new(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            float x = 10;
            float y = (pictureBox.Height - textSize.Height) / 2;

            e.Graphics.DrawString(text, font, brush, x, y);
        }

        private void CreateAccountPanelBackArrowPictureBox_Click(object sender, EventArgs e)
        {
            ShowAccountsPanel();
        }

        private void CreateAccountPanelPasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.SuppressKeyPress = true;
            CreateAccountPanelCreatePictureBox_Click(null, null);
        }

        #endregion
    }
}
