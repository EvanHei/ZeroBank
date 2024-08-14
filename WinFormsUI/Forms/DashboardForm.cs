using ClientLibrary;
using ClientLibrary.Models;
using SharedLibrary.Models;
using System.Globalization;
using System.Transactions;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WinFormsUI
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

        private async Task GetData()
        {
            accounts = await ClientConfig.ApiAccessor.GetAccounts();
        }

        private class DailyBalance
        {
            public DateTime Date { get; set; }
            public double Balance { get; set; }
        }

        private void LoadMainChart(List<PlaintextTransaction> plaintextTransactions)
        {
            // dummy data for testing
            List<PlaintextTransaction> list = new List<PlaintextTransaction>
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
            var balanceOverTime = list
                .OrderBy(pt => pt.Timestamp)
                .Select(pt =>
                {
                    cumulativeBalance += pt.Amount;
                    return new { pt.Timestamp, Balance = cumulativeBalance / 100.0 };
                })
                .ToList();

            // determine time range
            int previousDays = -7;
            if (AccountDetailsPanelLastMonthPictureBox.Visible)
            {
                previousDays = -30;
            }
            else if (AccountDetailsPanelLastYearPictureBox.Visible)
            {
                previousDays = -365;
            }
            DateTime currentDate = DateTime.Now;
            DateTime previousDate = currentDate.AddDays(previousDays);

            // filter data within the range
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

            // TODO: update to formet better and set the horizontal lines to be a more muted color
            chartArea.AxisY.LabelStyle.Format = "${}";

            // define series
            Series series = new();
            series.ChartType = SeriesChartType.Area;
            series.BorderWidth = 2;
            series.Color = Color.Purple;
            series.BackSecondaryColor = Color.DarkRed;
            series.BackGradientStyle = GradientStyle.LeftRight;
            series.BorderDashStyle = ChartDashStyle.Solid;
            series.XValueMember = "Timestamp";
            series.YValueMembers = "Balance";

            // add controls
            chart.ChartAreas.Add(chartArea);
            chart.Series.Add(series);
            AccountDetailsPanelMainChartPanel.Controls.Add(chart);
        }

        private void LoadDoughnutChart(List<PlaintextTransaction> plaintextTransactions)
        {
            // get number of deposits and withdrawals
            int numberOfDeposits = plaintextTransactions.Count(pt => pt.Amount <= 0);
            int numberOfWithdrawals = plaintextTransactions.Count(pt => pt.Amount > 0);

            Chart chart = new();
            chart.Dock = DockStyle.Fill;
            chart.BackColor = Color.FromArgb(79, 79, 79);

            ChartArea chartArea = new();
            chartArea.BackColor = Color.FromArgb(79, 79, 79);
            chart.ChartAreas.Add(chartArea);

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
            chart.Series.Add(series);

            Legend legend = new();
            legend.Docking = Docking.Bottom;
            legend.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            legend.BackColor = Color.FromArgb(79, 79, 79);
            legend.ForeColor = Color.White;
            chart.Legends.Add(legend);

            AccountDetailsPanelDoughnutChartPanel.Controls.Add(chart);
        }

        private void ShowDashboardPanel()
        {
            DashboardPanel.Visible = true;
            AccountsPanel.Visible = false;
            AccountDetailsPanel.Visible = false;
            TransactPanel.Visible = false;
        }

        private void ShowAccountsPanel()
        {
            AccountsPanelListBox.DataSource = accounts;

            DashboardPanel.Visible = false;
            AccountsPanel.Visible = true;
            AccountDetailsPanel.Visible = false;
            TransactPanel.Visible = false;
        }

        private void ShowTransactPanel()
        {
            DashboardPanel.Visible = false;
            AccountsPanel.Visible = false;
            AccountDetailsPanel.Visible = false;
            TransactPanel.Visible = true;
        }

        private void ShowAccountDetailsPanel()
        {
            DashboardPanel.Visible = false;
            AccountsPanel.Visible = false;
            AccountDetailsPanel.Visible = true;
            TransactPanel.Visible = false;

            AccountDetailsPanelTransactionsListBox.DataSource = selectedAccountPlaintextTransactions;
            AccountsPanelPasswordTextBox.Text = "";
            LoadDoughnutChart(selectedAccountPlaintextTransactions);
            LoadMainChart(selectedAccountPlaintextTransactions);
        }

        private void AccountsPictureBox_Paint(object sender, PaintEventArgs e)
        {
            // draw "Accounts"
            string smallText = "Accounts";

            using Font smallTextFont = new("Segoe UI", 12, FontStyle.Regular, GraphicsUnit.Point);
            using Brush brush = new SolidBrush(Color.White);
            SizeF smallTextSize = e.Graphics.MeasureString(smallText, smallTextFont);

            float smallTextX = (AccountsPanelAccountsPictureBox.ClientSize.Width - smallTextSize.Width) / 2;
            float smallTextY = (AccountsPanelAccountsPictureBox.ClientSize.Height - smallTextSize.Height) / 2;

            float smallTextOffsetX = 15;  // shift n pixels left
            float smallTextOffsetY = 15;   // shift n pixels up
            smallTextX -= smallTextOffsetX;
            smallTextY -= smallTextOffsetY;

            e.Graphics.DrawString(smallText, smallTextFont, brush, new PointF(smallTextX, smallTextY));

            // draw number of accounts
            string largeText = accounts.Count.ToString();

            using Font largeTextFont = new("Segoe UI", 16, FontStyle.Regular, GraphicsUnit.Point);
            SizeF largeTextSize = e.Graphics.MeasureString(largeText, largeTextFont);

            // same location initially
            float largeTextX = smallTextX;
            float largeTextY = smallTextY;

            float largeTextOffsetY = -18;   // shift -n pixels down
            largeTextY -= largeTextOffsetY;

            e.Graphics.DrawString(largeText, largeTextFont, brush, new PointF(smallTextX, largeTextY));
        }

        private async void DashboardForm_Load(object sender, EventArgs e)
        {
            await GetData();
        }

        private async void PasswordArrowPictureBox_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AccountsPanelPasswordTextBox.Text))
            {
                return;
            }

            try
            {
                selectedAccountPassword = AccountsPanelPasswordTextBox.Text;
                selectedAccount = (Account)AccountsPanelListBox.SelectedItem;

                await GetAccountData(selectedAccount, selectedAccountPassword);

                ShowAccountDetailsPanel();
            }
            catch (Exception ex)
            {
                // TODO: display error
                MessageBox.Show(ex.Message);
            }
        }

        private async Task GetAccountData(Account account, string password)
        {
            // populate variables
            selectedAccount = account;
            selectedAccountPassword = password;
            selectedAccountBalance = await ClientConfig.GetBalance(selectedAccount.Id, selectedAccountPassword);

            // order transactions by date (most recent first)
            List<PlaintextTransaction> plaintextTransactions = await ClientConfig.GetPlaintextTransactions(selectedAccount.Id, selectedAccountPassword);
            selectedAccountPlaintextTransactions = plaintextTransactions.OrderByDescending(transaction => transaction.Timestamp).ToList();
        }

        private void BalancePictureBox_Paint(object sender, PaintEventArgs e)
        {
            // draw "Name"
            string smallText = "Name";

            using Font smallTextFont = new("Segoe UI", 10, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF smallTextSize = e.Graphics.MeasureString(smallText, smallTextFont);

            float smallTextX = (AccountsPanelAccountsPictureBox.ClientSize.Width - smallTextSize.Width) / 2;
            float smallTextY = (AccountsPanelAccountsPictureBox.ClientSize.Height - smallTextSize.Height) / 2;

            float smallTextOffsetX = 58;  // shift n pixels left
            float smallTextOffsetY = 24;   // shift m pixels up
            smallTextX -= smallTextOffsetX;
            smallTextY -= smallTextOffsetY;

            e.Graphics.DrawString(smallText, smallTextFont, brush, new PointF(smallTextX, smallTextY));

            // draw actual name of account
            string largeText = selectedAccount.Name;

            using Font largeTextFont = new("Segoe UI", 16, FontStyle.Regular, GraphicsUnit.Point);
            SizeF largeTextSize = e.Graphics.MeasureString(largeText, largeTextFont);

            // same location initially
            float largeTextX = smallTextX;
            float largeTextY = smallTextY;

            float largeTextOffsetY = -14;   // shift -n pixels down
            largeTextY -= largeTextOffsetY;

            e.Graphics.DrawString(largeText, largeTextFont, brush, new PointF(smallTextX, largeTextY));
        }

        private void TransactionsPictureBox_Paint(object sender, PaintEventArgs e)
        {
            // draw "Transactions"
            string smallText = "Transactions";

            using Font smallTextFont = new("Segoe UI", 10, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF smallTextSize = e.Graphics.MeasureString(smallText, smallTextFont);

            float smallTextX = (AccountsPanelAccountsPictureBox.ClientSize.Width - smallTextSize.Width) / 2;
            float smallTextY = (AccountsPanelAccountsPictureBox.ClientSize.Height - smallTextSize.Height) / 2;

            float smallTextOffsetX = 41;  // shift n pixels left
            float smallTextOffsetY = 24;   // shift m pixels up
            smallTextX -= smallTextOffsetX;
            smallTextY -= smallTextOffsetY;

            e.Graphics.DrawString(smallText, smallTextFont, brush, new PointF(smallTextX, smallTextY));

            // draw number of transactions
            string largeText = selectedAccount.Transactions.Count.ToString();

            using Font largeTextFont = new("Segoe UI", 16, FontStyle.Regular, GraphicsUnit.Point);
            SizeF largeTextSize = e.Graphics.MeasureString(largeText, largeTextFont);

            // same location initially
            float largeTextX = smallTextX;
            float largeTextY = smallTextY;

            float largeTextOffsetY = -14;   // shift -n pixels down
            largeTextY -= largeTextOffsetY;

            e.Graphics.DrawString(largeText, largeTextFont, brush, new PointF(smallTextX, largeTextY));
        }

        private void BlockchainPictureBox_Paint(object sender, PaintEventArgs e)
        {
            // draw "Switch To"
            string smallText = "Switch To";

            using Font smallTextFont = new("Segoe UI", 10, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF smallTextSize = e.Graphics.MeasureString(smallText, smallTextFont);

            float smallTextX = (AccountsPanelAccountsPictureBox.ClientSize.Width - smallTextSize.Width) / 2;
            float smallTextY = (AccountsPanelAccountsPictureBox.ClientSize.Height - smallTextSize.Height) / 2;

            float smallTextOffsetX = 50;  // shift n pixels left
            float smallTextOffsetY = 24;   // shift m pixels up
            smallTextX -= smallTextOffsetX;
            smallTextY -= smallTextOffsetY;

            e.Graphics.DrawString(smallText, smallTextFont, brush, new PointF(smallTextX, smallTextY));

            // draw "Blockchain View"
            string largeText = "Blockchain View";

            using Font largeTextFont = new("Segoe UI", 16, FontStyle.Regular, GraphicsUnit.Point);
            SizeF largeTextSize = e.Graphics.MeasureString(largeText, largeTextFont);

            // same location initially
            float largeTextX = smallTextX;
            float largeTextY = smallTextY;

            float largeTextOffsetY = -14;   // shift -n pixels down
            largeTextY -= largeTextOffsetY;

            e.Graphics.DrawString(largeText, largeTextFont, brush, new PointF(smallTextX, largeTextY));
        }

        private void AccountsListBox_DrawItem(object sender, DrawItemEventArgs e)
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

        private void AccountsListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 40;
        }

        private void TransactionsListBox_DrawItem(object sender, DrawItemEventArgs e)
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

        private void TransactionsListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 40;
        }

        private void TranasctionsPictureBox_Paint(object sender, PaintEventArgs e)
        {
            // draw "History"
            string text = "History";

            using Font textFont = new("Segoe UI", 16, FontStyle.Regular, GraphicsUnit.Point);
            using SolidBrush brush = new(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, textFont);

            float textX = 12;  // n pixels from the left side
            float textY = 7;  // m pixels from the top

            e.Graphics.DrawString(text, textFont, brush, new PointF(textX, textY));
        }

        private void DoughnutChartPictureBox_Paint(object sender, PaintEventArgs e)
        {
            // draw "Transactions"
            string transactionsText = "Transactions";

            using Font transactionsFont = new("Segoe UI", 16, FontStyle.Regular, GraphicsUnit.Point);
            using Brush transactionsBrush = new SolidBrush(Color.White);
            SizeF transactionsSize = e.Graphics.MeasureString(transactionsText, transactionsFont);

            float transactionsTextX = 12;  // n pixels from the left side
            float transactionsTextY = 7;  // m pixels from the top

            e.Graphics.DrawString(transactionsText, transactionsFont, transactionsBrush, new PointF(transactionsTextX, transactionsTextY));

            // draw balance
            NumberFormatInfo customFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            customFormat.CurrencyNegativePattern = 1;
            string balanceText = (selectedAccountBalance * .01).ToString("C", customFormat);

            using Font balanceFont = new("Segoe UI", 16, FontStyle.Regular, GraphicsUnit.Point);
            using Brush balanceBrush = new SolidBrush(Color.White);
            SizeF balanceSize = e.Graphics.MeasureString(transactionsText, transactionsFont);

            float balanceX = ((sender as Control).ClientSize.Width / 2 - balanceSize.Width / 2) + 30;
            float balanceY = 350;  // m pixels from the top

            e.Graphics.DrawString(balanceText, balanceFont, balanceBrush, new PointF(balanceX, balanceY));
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

        private void DashboardForm_Paint(object sender, PaintEventArgs e)
        {
            // draw the line separating the sidebar
            using Pen pen = new(Color.FromArgb(79, 79, 79), 2);
            int xPosition = 195;
            e.Graphics.DrawLine(pen, new Point(xPosition, 0), new Point(xPosition, this.ClientSize.Height));
        }

        private void MakeTransactionPictureBox_Click(object sender, EventArgs e)
        {
            ShowTransactPanel();
        }

        private void ConfirmPictureBox_Paint(object sender, PaintEventArgs e)
        {
            string text = "Confirm";

            using Font font = new("Segoe UI Emoji", 12, FontStyle.Regular, GraphicsUnit.Point);
            using Brush brush = new SolidBrush(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            float x = (TransactPanelConfirmPictureBox.ClientSize.Width - textSize.Width) / 2;
            float y = (TransactPanelConfirmPictureBox.ClientSize.Height - textSize.Height) / 2;

            e.Graphics.DrawString(text, font, brush, new PointF(x, y));
        }

        private async void ConfirmPictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: make AmountTextBox.Text have .00 add add error message for incorrect password (also check if null or whitespace)
                long amount;
                if (!long.TryParse(TransactPanelAmountTextBox.Text, out amount))
                {
                    return;
                }

                // if it's a withdrawal, make negative
                if (TransactPanelWithdrawPictureBox.Visible == true)
                {
                    amount *= -1;
                }

                await ClientConfig.AddTransaction(selectedAccount.Id, amount, selectedAccountPassword);
                TransactPanelAmountTextBox.Text = "";
                await GetAccountData(selectedAccount, selectedAccountPassword);
            }
            catch (Exception ex)
            {
                // TODO: display error
            }
            finally
            {
                ShowAccountDetailsPanel();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowTransactPanel();
        }

        private void DepositLabel_Click(object sender, EventArgs e)
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

        private void AccountDetailsPanelLastWeekLabel_Click(object sender, EventArgs e)
        {
            AccountDetailsPanelLastWeekPictureBox.Visible = true;
            AccountDetailsPanelLastMonthPictureBox.Visible = false;
            AccountDetailsPanelLastYearPictureBox.Visible = false;
            AccountDetailsPanelLastWeekLabel.BackColor = Color.FromArgb(79, 79, 79);
            AccountDetailsPanelLastMonthLabel.BackColor = Color.FromArgb(45, 45, 45);
            AccountDetailsPanelLastYearLabel.BackColor = Color.FromArgb(45, 45, 45);

            LoadMainChart(selectedAccountPlaintextTransactions);
        }

        private void AccountDetailsPanelLastMonthLabel_Click(object sender, EventArgs e)
        {
            AccountDetailsPanelLastWeekPictureBox.Visible = false;
            AccountDetailsPanelLastMonthPictureBox.Visible = true;
            AccountDetailsPanelLastYearPictureBox.Visible = false;
            AccountDetailsPanelLastWeekLabel.BackColor = Color.FromArgb(45, 45, 45);
            AccountDetailsPanelLastMonthLabel.BackColor = Color.FromArgb(79, 79, 79);
            AccountDetailsPanelLastYearLabel.BackColor = Color.FromArgb(45, 45, 45);

            LoadMainChart(selectedAccountPlaintextTransactions);
        }

        private void AccountDetailsPanelLastYearLabel_Click(object sender, EventArgs e)
        {
            AccountDetailsPanelLastWeekPictureBox.Visible = false;
            AccountDetailsPanelLastMonthPictureBox.Visible = false;
            AccountDetailsPanelLastYearPictureBox.Visible = true;
            AccountDetailsPanelLastWeekLabel.BackColor = Color.FromArgb(45, 45, 45);
            AccountDetailsPanelLastMonthLabel.BackColor = Color.FromArgb(45, 45, 45);
            AccountDetailsPanelLastYearLabel.BackColor = Color.FromArgb(79, 79, 79);

            LoadMainChart(selectedAccountPlaintextTransactions);
        }
    }
}
