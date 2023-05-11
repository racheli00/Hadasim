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

namespace Health
{
    /// <summary>
    /// Interaction logic for specidficPatient.xaml
    /// </summary>
    public partial class specidficPatient : Window
    {
        public specidficPatient(string idSended)
        {
            InitializeComponent();

            DataTable myPatients = BL.getDataById(idSended);
            id.Text = idSended;
            name.Content = myPatients.Rows[0]["lastName"].ToString() + " " + myPatients.Rows[0]["firstName"].ToString();
            birthDate.Text = myPatients.Rows[0]["birthDate"].ToString();
            phone.Text = myPatients.Rows[0]["phone"].ToString() == "" ? "אין" : myPatients.Rows[0]["phone"].ToString();
            cellPhone.Text = myPatients.Rows[0]["cellPhone"].ToString();
            city.Text = myPatients.Rows[0]["city"].ToString();

            if (myPatients.Rows[0]["positiveDate"].ToString()!="")// is the patient got sick
            {
                sickLabel.Visibility = Visibility.Visible;
                positiveDateLabel.Visibility = Visibility.Visible;
                positiveDate.Visibility = Visibility.Visible;
                positiveDate.Text = myPatients.Rows[0]["positiveDate"].ToString();

                if(myPatients.Rows[0]["recorevyDate"].ToString() != "")// is the patient got recovered
                {
                    recoveryDate.Visibility = Visibility.Visible;
                    recoveryDateLabel.Visibility = Visibility.Visible;
                    recoveryDate.Text = myPatients.Rows[0]["recorevyDate"].ToString();

                }
            }
            if (myPatients.Rows[0]["vaccionationDate"].ToString() != "")
            {
                vaccinationsTable.Visibility = Visibility.Visible;
                List<Item> items = new List<Item>();
                for (int i = 0; i < myPatients.Rows.Count; i++)
                {
                    Item newItem = new Item()
                    {
                        VaccinationDate = myPatients.Rows[i]["vaccionationDate"].ToString(),
                        Manufacturer = myPatients.Rows[i]["manufacturer"].ToString()
                    };
                    items.Add(newItem);
                }
                vaccinationsTable.ItemsSource = items;
            }

        }


    public class Item
        {
            public string VaccinationDate  { get; set; }
            public string Manufacturer { get; set; }
        }
    }
}
