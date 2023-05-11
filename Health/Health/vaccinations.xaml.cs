using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Health
{
    /// <summary>
    /// Interaction logic for vaccinations.xaml
    /// </summary>
    public partial class vaccinations : Window
    {
        public vaccinations()
        {
            InitializeComponent();
            manufacturers.Items.Add("פייזר");
            manufacturers.Items.Add("מודרנה");
            manufacturers.Items.Add("אסטרהזניקה");
            manufacturers.Items.Add("נובהווקס");

            vaccinateDate.DisplayDateEnd = DateTime.Today;

            vaccinationsView.ItemsSource = data.vaccinationsDT.DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (id.Text == "" || vaccinateDate.Text == "" || manufacturers.Text == "")
            {
                MessageBox.Show("אחד או יותר מהשדות ריקים, אנא מלאו ונסו שוב", "Error");
                return;
            }

            if (BL.IsIsraeliIdNumber(id.Text) == false)
            {
                MessageBox.Show("תעודת זהות אינה תקינה, נסו שוב", "Error");
                return;
            }

            int exsist = BL.isPatientExist(id.Text);
            if (exsist == 0)
            {
                if (MessageBox.Show("תעודת זהות לא קיימת, תרצה להוסיף כעת?,", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    newPatient np = new newPatient();
                    np.id.Text = this.id.Text;
                    np.ShowDialog();
                }
                else
                    this.Close();
                return;
            }

            
            if (BL.countVaccinations(id.Text)>3)
            {
                if (MessageBox.Show("מטופל כבר התחסן ארבעה חיסונים, תרצה להכניס מטופל אחר?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    id.Text = "";
                    vaccinateDate.SelectedDate = null;
                    manufacturers.SelectedItem = null;
                }
                else
                    this.Close();
                return;
            }
            BL.addVaccination(id.Text, vaccinateDate.Text, manufacturers.Text);
            MessageBox.Show(" חיסון נוסף בהצלחה!", "חיסון");
            data.vaccinationsDT = Dal.getTable("SP_getVaccinations", null);
            vaccinationsView.ItemsSource = data.vaccinationsDT.DefaultView;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
