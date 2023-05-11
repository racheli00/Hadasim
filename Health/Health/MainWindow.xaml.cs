using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;


namespace Health
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //// Calculate the last month, and how many days it has
            int year = DateTime.Today.Month == 01 ? DateTime.Today.Year - 1 : DateTime.Today.Year;
            int month = DateTime.Today.Month == 01 ? 12 : DateTime.Today.Month - 1;
            int days = DateTime.DaysInMonth(year, month);

            Chart chart = new Chart();
            chart.Titles.Add(" מספר אנשים חולים בקורונה בחודש שעבר"+month+"/"+year);

            // Set the chart area (look)
            ChartArea chartArea = new ChartArea();
            chartArea.AxisX.Title = "תאריך";
            chartArea.AxisY.Title = "מספר האנשים";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.LabelStyle.Angle = -45;
            chart.ChartAreas.Add(chartArea);

            // Set the chart series
            Series series = new Series();
            series.Name = "Number of People Sick";
            series.ChartType = SeriesChartType.Column;
            chart.Series.Add(series);

            int day = 0;
            DateTime specificDate;
            // Get the data and add it to the chart series
            for (int currentDay = 1; currentDay <= days; currentDay++)
            {
                day++;
                specificDate = new DateTime(year, month, day);
                int count = BL.countSickInDay(specificDate.ToShortDateString());
                DateTime dateForChart = new DateTime(year, month, currentDay);
                series.Points.AddXY(dateForChart.ToShortDateString(), count);
            }

            // Add the chart to the WindowsFormsHost
            chartHost.Child = chart;

            

        }

    

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            newPatient pt = new newPatient();
            pt.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            vaccinations vs = new vaccinations();
            vs.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            sick s = new sick();
            s.ShowDialog();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (searchId.Text == "")
            {
                MessageBox.Show("הכנס תעודת זהות בבקשה", "Error");
                return;
            }

            if (BL.IsIsraeliIdNumber(searchId.Text) == false)
            {
                MessageBox.Show("תעודת זהות אינה תקינה, נסו שוב", "Error");
                return;
            }

            int exsist = BL.isPatientExist(searchId.Text);
            if (exsist == 0)
            {
                if (MessageBox.Show("תעודת זהות לא קיימת, תרצה להוסיף כעת?,", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    newPatient np = new newPatient();
                    np.id.Text = this.searchId.Text;
                    np.ShowDialog();
                }
                else
                    this.Close();
                return;
            }

            specidficPatient sp = new specidficPatient(searchId.Text);
            sp.ShowDialog();

        }
    }
}
