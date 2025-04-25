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
using VeterinaryClinic.Model;

namespace VeterinaryClinic
{
        /// <summary>
        /// Логика взаимодействия для EditMedicationWindow.xaml
        /// </summary>
        public partial class EditMedicationWindow : Window
        {
                public string MedicationName => txtName.Text;
                public decimal MedicationPrice => decimal.Parse(txtPrice.Text);
                public int MedicationQuantity => int.Parse(txtQuantity.Text);
                public string MedicationUnit => txtUnit.Text;

                private medicines _medication;

                public EditMedicationWindow(medicines medication)
                {
                        InitializeComponent();
                        _medication = medication;
                        DataContext = _medication;
                }

                private void SaveButton_Click(object sender, RoutedEventArgs e)
                {
                        if (string.IsNullOrWhiteSpace(txtName.Text) ||
                            !decimal.TryParse(txtPrice.Text, out _) ||
                            !int.TryParse(txtQuantity.Text, out _) ||
                            string.IsNullOrWhiteSpace(txtUnit.Text))
                        {
                                MessageBox.Show("Пожалуйста, заполните все поля корректно");
                                return;
                        }

                        DialogResult = true;
                        Close();
                }

                private void CancelButton_Click(object sender, RoutedEventArgs e)
                {
                        DialogResult = false;
                        Close();
                }
        }
}
        

