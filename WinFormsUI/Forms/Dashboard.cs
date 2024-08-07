using ClientLibrary;
using Microsoft.Identity.Client;
using SharedLibrary.Models;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WinFormsUI
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            PopulateForm();
            ShowDashboardPanel();
        }

        private async void PopulateForm()
        {
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

        private void ShowAccountsPanel()
        {
            DashboardPanel.Visible = false;
            AccountsPanel.Visible = true;
            AccountDetailsPanel.Visible = false;
        }

        private void ShowDashboardPanel()
        {
            DashboardPanel.Visible = true;
            AccountsPanel.Visible = false;
            AccountDetailsPanel.Visible = false;

            int numberOfAccounts = ClientConfig.DataAccessor.LoadAccounts().Count;
            NumberOfAccountsLabel.Text = numberOfAccounts == 1 ? $"{numberOfAccounts} Account" : $"{numberOfAccounts} Accounts";

            int numberOfTransactions = ClientConfig.DataAccessor.LoadAllCiphertextTransactions().Count;
            NumberOfTransactionsLabel.Text = numberOfTransactions == 1 ? $"{numberOfTransactions} Transaction" : $"{numberOfTransactions} Transactions";
        }

        private void AccountsLabel_Click(object sender, EventArgs e)
        {
            ShowAccountsPanel();
        }

        private void DashboardLabel_Click(object sender, EventArgs e)
        {
            ShowDashboardPanel();
        }
    }
}
