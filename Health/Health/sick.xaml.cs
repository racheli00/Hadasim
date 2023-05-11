using System;
using System.Windows;
using System.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace Health
{
    /// <summary>
    /// Interaction logic for sick.xaml
    /// </summary>
    public partial class sick : Window
    {
        public sick()
        {
            InitializeComponent();
            positiveDate.DisplayDateEnd = DateTime.Today;
            sickView.ItemsSource = data.sickDT.DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (id.Text=="" || positiveDate.SelectedDate==null)
            {
                MessageBox.Show("אחד או יותר מהשדות ריקים, אנא מלאו ונסו שוב", "Error");
                return;
            }

            int exsist = BL.isPatientExist(id.Text);//is patient exist?
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

            DateTime recovery= new DateTime();
            DateTime positive = new DateTime();

            if (recoveryDate.SelectedDate == null)
            {
                if (MessageBox.Show("מטופל ללא תאריך החלמה, תרצה להמשיך בכל זאת?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    this.Close();
                    return;
                }
            }

            else
            {
                 recovery = recoveryDate.SelectedDate.Value;
                 positive = positiveDate.SelectedDate.Value;

                if (recovery < positive || (recovery - positive).TotalDays < 5)
                {
                    MessageBox.Show("תאריך החלמה חייב להיות גדול לפחות במחישה ימים מתאריך קבלת תוצאה חיובית", "Error");
                    return;
                }
            }

            DataTable dt = BL.isAlreadySick(id.Text);
            if (dt!=null && dt.Rows.Count>0)//there id of patient that is sick
            {
                if(dt.Rows[0][2].ToString()=="")//no recovery date
                {
                    if (recoveryDate.SelectedDate != null)
                    {
                        BL.updateRecoveryDate(id.Text, recoveryDate.Text);
                        MessageBox.Show("תאריך החלמה עודכן בהצלחה");
                        data.sickDT = Dal.getTable("SP_getSick", null);
                        sickView.ItemsSource = data.sickDT.DefaultView;
                        return;
                    }

                    if (MessageBox.Show("מטופל כבר קיים במערכת ללא תאריך החלמה, תרצה לעדכן כעת?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        DateTime positiveBySql = DateTime.Parse(dt.Rows[0][1].ToString());
                       
                        if (recovery < positiveBySql || (recovery - positiveBySql).TotalDays < 5)
                        {
                            MessageBox.Show("תאריך החלמה חייב להיות גדול לפחות במחישה ימים מתאריך קבלת תוצאה חיובית", "Error");
                            return;
                        }
                        BL.updateRecoveryDate(id.Text, recoveryDate.Text);
                        MessageBox.Show("תאריך החלמה עודכן בהצלחה");
                        data.sickDT = Dal.getTable("SP_getSick", null);
                        sickView.ItemsSource = data.sickDT.DefaultView;
                        return;
                    }
                    else//user dont want to update recovery date
                    {
                        id.Text = "";
                        recoveryDate.SelectedDate = null;
                        positiveDate.SelectedDate = null;
                        return;
                    }
                    
                }

                else
                {
                    if (MessageBox.Show("מטופל כבר חלה פעם אחת, תרצה להכניס מטופל אחר?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        id.Text = "";
                        recoveryDate.SelectedDate = null;
                        positiveDate.SelectedDate = null;
                    }

                    else
                        this.Close();
                    return;

                }
            }


            BL.insertSick(id.Text, positiveDate.Text, recoveryDate.Text);
            MessageBox.Show("מטופל נוסף בהצלחה!");
            data.sickDT = Dal.getTable("SP_getSick", null);
            sickView.ItemsSource = data.sickDT.DefaultView;

            id.Text = "";
            positiveDate.SelectedDate = null;
            recoveryDate.SelectedDate = null;

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void positiveDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (positiveDate.SelectedDate == null)
                return;
            DataTable dt = BL.isAlreadySick(id.Text);
            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][2].ToString() == "")//there id of patient that is sick
                positiveDate.SelectedDate = DateTime.Parse(dt.Rows[0][1].ToString());
            recoveryDate.DisplayDateStart = positiveDate.SelectedDate.Value.AddDays(5);
        }
    }
}
