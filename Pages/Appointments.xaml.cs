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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using VeterinaryClinic.Model;

namespace VeterinaryClinic.Pages
{
        /// <summary>
        /// Логика взаимодействия для Appointments.xaml
        /// </summary>
        public partial class Appointments : Page
        {
                private VeterinaryСlinicEntities context;
                private int currentUserRole;

                public Appointments(int userRole)
                {
                        InitializeComponent();
                        context = new VeterinaryСlinicEntities();
                        currentUserRole = userRole;
                        LoadAppointments();
                }

                private void LoadAppointments()
                {
                        try
                        {
                                AppointmentsListView.ItemsSource = context.contract
                                    .Include(c => c.animal)
                                    .Include(c => c.client)
                                    .Include(c => c.provisionservices.Select(ps => ps.employees))
                                    .ToList();
                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show($"Ошибка при загрузке записей: {ex.Message}",
                                              "Ошибка",
                                              MessageBoxButton.OK,
                                              MessageBoxImage.Error);
                        }
                }

                private void AddAppointmentButton_Click(object sender, RoutedEventArgs e)
                {
                        try
                        {
                                var newAppointment = new contract
                                {
                                        date = DateTime.Now,
                                        id_client = 1, 
                                        id_animal = 1 
                                };

                                context.contract.Add(newAppointment);
                                context.SaveChanges();
                                LoadAppointments();

                                AppointmentsListView.ScrollIntoView(newAppointment);
                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}",
                                              "Ошибка",
                                              MessageBoxButton.OK,
                                              MessageBoxImage.Error);
                        }
                }

                private void EditAppointment_Click(object sender, RoutedEventArgs e)
                {
                        var button = sender as Button;
                        if (button?.Tag is int appointmentId)
                        {
                                var appointment = context.contract.Find(appointmentId);
                                if (appointment != null)
                                {
                                        var editWindow = new EditAppointmentWindow(appointment, context);
                                        if (editWindow.ShowDialog() == true)
                                        {
                                                try
                                                {
                                                        context.SaveChanges();
                                                        LoadAppointments();
                                                }
                                                catch (Exception ex)
                                                {
                                                        MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}",
                                                                      "Ошибка",
                                                                      MessageBoxButton.OK,
                                                                      MessageBoxImage.Error);
                                                }
                                        }
                                }
                        }
                }
                private void DeleteAppointment_Click(object sender, RoutedEventArgs e)
                {
                        if (MessageBox.Show("Вы уверены, что хотите удалить эту запись?",
                                          "Подтверждение удаления",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                                var button = sender as Button;
                                if (button?.Tag is int appointmentId)
                                {
                                        try
                                        {
                                                var appointmentToDelete = context.contract.Find(appointmentId);
                                                if (appointmentToDelete != null)
                                                {
                                                        context.contract.Remove(appointmentToDelete);
                                                        context.SaveChanges();
                                                        LoadAppointments();
                                                }
                                        }
                                        catch (Exception ex)
                                        {
                                                MessageBox.Show($"Ошибка при удалении записи: {ex.Message}\n\nЗапись может быть связана с другими данными.",
                                                              "Ошибка",
                                                              MessageBoxButton.OK,
                                                              MessageBoxImage.Error);
                                        }
                                }
                        }
                }

                private void BackButton_Click(object sender, RoutedEventArgs e)
                {
                        NavigationService.Navigate(new Dashboard(currentUserRole));
                }
        }
}
