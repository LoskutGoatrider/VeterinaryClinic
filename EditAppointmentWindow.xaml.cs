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
        /// Логика взаимодействия для EditAppointmentWindow.xaml
        /// </summary>
        public partial class EditAppointmentWindow : Window
        {
                private contract _appointment;
                private VeterinaryСlinicEntities _context;

                public EditAppointmentWindow(contract appointment, VeterinaryСlinicEntities context)
                {
                        InitializeComponent();
                        _appointment = appointment;
                        _context = context;
                        DataContext = _appointment;

                        cmbClient.ItemsSource = _context.client.ToList();
                        cmbAnimal.ItemsSource = _context.animal.ToList();

                        // Установка времени из существующей записи
                        if (_appointment.date != null)
                        {
                                HoursCombo.SelectedIndex = _appointment.date.Hour;
                                MinutesCombo.SelectedIndex = _appointment.date.Minute / 15;
                        }
                        else
                        {
                                HoursCombo.SelectedIndex = 0;
                                MinutesCombo.SelectedIndex = 0;
                        }
                }

                private void SaveButton_Click(object sender, RoutedEventArgs e)
                {
                        if (dpDate.SelectedDate == null || cmbClient.SelectedItem == null || cmbAnimal.SelectedItem == null)
                        {
                                MessageBox.Show("Пожалуйста, заполните все поля");
                                return;
                        }

                        int hours = int.Parse((HoursCombo.SelectedItem as ComboBoxItem).Content.ToString());
                        int minutes = int.Parse((MinutesCombo.SelectedItem as ComboBoxItem).Content.ToString());

                        _appointment.date = dpDate.SelectedDate.Value.Date + new TimeSpan(hours, minutes, 0);

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