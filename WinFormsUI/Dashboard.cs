using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WinFormsUI
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();

            Chart chart = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(79, 79, 79)// Set the chart to fill the panel
            }; 

            // Add a ChartArea to the Chart
            ChartArea chartArea = new ChartArea("MainArea");
            chart.ChartAreas.Add(chartArea);

            // Create a Series with SplineArea chart type
            Series series = new Series
            {
                Name = "SplineAreaSeries",
                ChartType = SeriesChartType.SplineArea,
                BorderWidth = 2
            };

            // Add data points to the series
            series.Points.AddXY(1, 10);
            series.Points.AddXY(2, 25);
            series.Points.AddXY(3, 15);
            series.Points.AddXY(4, 35);
            series.Points.AddXY(5, 25);

            // Customize the series appearance
            series.Color = System.Drawing.Color.Purple;
            series.BackSecondaryColor = System.Drawing.Color.DarkRed;
            series.BackGradientStyle = GradientStyle.LeftRight;
            series.BorderDashStyle = ChartDashStyle.Solid;

            // Add the Series to the Chart
            chart.Series.Add(series);

            // Add a title to the chart
            chart.Titles.Add("Spline Area Chart Example");

            // Add the Chart control to the Panel
            ChartPanel.Controls.Add(chart);

        }
    }
}
