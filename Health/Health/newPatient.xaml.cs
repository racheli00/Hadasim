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
using System.Data;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;

namespace Health
{
    /// <summary>
    /// Interaction logic for newPatient.xaml
    /// </summary>
    public partial class newPatient : Window
    {
        public newPatient()
        {
            InitializeComponent();
            
            patients.ItemsSource = data.patientsDT.DefaultView;
            birthDate.DisplayDateEnd = DateTime.Today;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (id.Text == "" || firstName.Text == "" || lastName.Text == "" || birthDate==null || cellPhone.Text == "")
            {
                MessageBox.Show("אחד או יותר מהשדות ריקים, אנא מלאו ונסו שוב", "Error");
                return;
            }

            if (BL.IsIsraeliIdNumber(id.Text)==false)
            {
                MessageBox.Show("תעודת זהות אינה תקינה, נסו שוב", "Error");
                return;
            }

            int exsist = BL.isPatientExist(id.Text);
            if (exsist == 1)
            {
                MessageBox.Show("תעודת זהות קיימת כבר במאגר");
                return;
            }

            BL.addPatient(id.Text, firstName.Text, lastName.Text, birthDate.Text, phone.Text, cellPhone.Text, city.Text, street.Text, int.Parse(streetNumber.Text));
            MessageBox.Show(firstName.Text+" "+lastName.Text+" נוסף בהצלחה!", "מטופל חדש");
            data.patientsDT = Dal.getTable("SP_getPatients", null);
            patients.ItemsSource = data.patientsDT.DefaultView;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
