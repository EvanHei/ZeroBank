using ClientLibrary;
using ClientLibrary.Models;
using Microsoft.Identity.Client;
using SharedLibrary.Models;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WinFormsUI
{
    public partial class DashboardForm : Form
    {
        private int accountsCount = 0;
        private List<Account> accounts = new();

        public DashboardForm()
        {
            InitializeComponent();
            PopulateForm();
            ShowDashboardPanel();
        }

        private void PopulateForm()
        {
            AccountsListBox.DataSource = accounts;

            //LoadChart();
        }

        private void LoadChart()
        {
            Chart chart = new()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(79, 79, 79)
            };

            ChartArea chartArea = new("MainArea");
            chart.ChartAreas.Add(chartArea);

            Series series = new()
            {
                Name = "SplineAreaSeries",
                ChartType = SeriesChartType.SplineArea,
                BorderWidth = 2
            };

            series.Points.AddXY(1, 10);
            series.Points.AddXY(2, 25);
            series.Points.AddXY(3, 15);
            series.Points.AddXY(4, 35);
            series.Points.AddXY(5, 25);
            series.Color = Color.Purple;
            series.BackSecondaryColor = Color.DarkRed;
            series.BackGradientStyle = GradientStyle.LeftRight;
            series.BorderDashStyle = ChartDashStyle.Solid;

            chart.Series.Add(series);
            chart.Titles.Add("Account Balance");

            ChartPanel.Controls.Add(chart);
        }

        private void ShowDashboardPanel()
        {
            DashboardPanel.Visible = true;
            AccountsPanel.Visible = false;
            AccountDetailsPanel.Visible = false;
        }

        private void ShowAccountsPanel()
        {
            DashboardPanel.Visible = false;
            AccountsPanel.Visible = true;
            AccountDetailsPanel.Visible = false;
        }

        private async void ShowAccountDetailsPanel()
        {
            if (string.IsNullOrWhiteSpace(PasswordTextBox.Text))
            {
                return;
            }

            try
            {
                Account account = (Account)AccountsListBox.SelectedItem;
                List<PlaintextTransaction> plainTransactions = await ClientConfig.GetPlaintextTransactions(account.Id, PasswordTextBox.Text);

                // TODO: populate the panel

                DashboardPanel.Visible = false;
                AccountsPanel.Visible = false;
                AccountDetailsPanel.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AccountsLabel_Click(object sender, EventArgs e)
        {
            ShowAccountsPanel();
        }

        private void DashboardLabel_Click(object sender, EventArgs e)
        {
            ShowDashboardPanel();
        }

        private void AccountsPictureBox_Paint(object sender, PaintEventArgs e)
        {
            // draw "Accounts"
            string accountsText = "Accounts";

            using Font accountsFont = new("Segoe UI", 12, FontStyle.Regular, GraphicsUnit.Point);
            using Brush brush = new SolidBrush(Color.White);
            SizeF accountsTextSize = e.Graphics.MeasureString(accountsText, accountsFont);

            float accountsX = (AccountsPictureBox.ClientSize.Width - accountsTextSize.Width) / 2;
            float accountsY = (AccountsPictureBox.ClientSize.Height - accountsTextSize.Height) / 2;

            float accountsOffsetX = 15;  // shift n pixels left
            float accountsOffsetY = 15;   // shift n pixels up
            accountsX -= accountsOffsetX;
            accountsY -= accountsOffsetY;

            e.Graphics.DrawString(accountsText, accountsFont, brush, new PointF(accountsX, accountsY));

            // draw number of accounts
            string numberOfAccountsText = accountsCount.ToString();

            using Font numberOfAccountsFont = new("Segoe UI", 16, FontStyle.Regular, GraphicsUnit.Point);
            SizeF numberOfAccountsTextSize = e.Graphics.MeasureString(numberOfAccountsText, numberOfAccountsFont);

            // same location initially
            float numberOfAccountsX = accountsX;
            float numberOfAccountsY = accountsY;

            float numberOfAccountsOffsetY = -18;   // shift -n pixels down
            numberOfAccountsY -= numberOfAccountsOffsetY;

            e.Graphics.DrawString(numberOfAccountsText, numberOfAccountsFont, brush, new PointF(accountsX, numberOfAccountsY));
        }

        private async void DashboardForm_Load(object sender, EventArgs e)
        {
            // get async data
            accounts = await ClientConfig.ApiAccessor.GetAccounts();
            accountsCount = accounts.Count;

            PopulateForm();
        }

        private void AccountsListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            Account account = (Account)AccountsListBox.Items[e.Index];

            // set the background color of the selected item
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color backgroundColor = isSelected ? Color.FromArgb(163, 6, 64) : e.BackColor;
            Color textColor = isSelected ? Color.White : e.ForeColor;
            using Brush backgroundBrush = new SolidBrush(backgroundColor);
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
            using Brush dateCreatedBrush = new SolidBrush(Color.FromArgb(145, 145, 145));
            SizeF dateCreatedTextSize = e.Graphics.MeasureString(dateCreatedText, dateCreatedFont);
            float dateCreatedX = e.Bounds.Right - dateCreatedTextSize.Width - 5;
            e.Graphics.DrawString(dateCreatedText, dateCreatedFont, dateCreatedBrush, dateCreatedX, e.Bounds.Y + 22);

            e.DrawFocusRectangle();
        }

        private void AccountsListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 40;
        }

        private async void PasswordArrowPictureBox_Click(object sender, EventArgs e)
        {
            ShowAccountDetailsPanel();
        }
    }
}
