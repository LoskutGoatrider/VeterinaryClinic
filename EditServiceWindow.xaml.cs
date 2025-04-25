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
        /// Логика взаимодействия для EditServiceWindow.xaml
        /// </summary>
        public partial class EditServiceWindow : Window
        {
                public string ServiceName => txtName.Text;
                public decimal ServicePrice => decimal.Parse(txtPrice.Text);

                private services _service;

                public EditServiceWindow(services service)
                {
                        InitializeComponent();
                        _service = service;
                        DataContext = _service;
                }

                private void SaveButton_Click(object sender, RoutedEventArgs e)
                {
                        if (string.IsNullOrWhiteSpace(txtName.Text) ||
                            !decimal.TryParse(txtPrice.Text, out _))
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
