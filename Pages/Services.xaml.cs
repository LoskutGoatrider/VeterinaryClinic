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
using VeterinaryClinic.Model;

namespace VeterinaryClinic.Pages
{
        /// <summary>
        /// Логика взаимодействия для Services.xaml
        /// </summary>
        public partial class Services : Page
        {

                private VeterinaryСlinicEntities context;
                private int currentUserRole;
                private bool _isAdmin => currentUserRole == 1;

                public Services(int userRole)
                {
                        InitializeComponent();
                        context = new VeterinaryСlinicEntities();
                        currentUserRole = userRole;
                        LoadServices();
                        SetUIByUserRole();
                }

                private void SetUIByUserRole()
                {
                        AddServiceButton.Visibility = _isAdmin ? Visibility.Visible : Visibility.Collapsed;
                }

                private void LoadServices()
                {
                        try
                        {
                                ServicesListView.ItemsSource = context.services.ToList();
                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show($"Ошибка при загрузке услуг: {ex.Message}",
                                              "Ошибка",
                                              MessageBoxButton.OK,
                                              MessageBoxImage.Error);
                        }
                }

                private void AddServiceButton_Click(object sender, RoutedEventArgs e)
                {
                        try
                        {
                                var newService = new Model.services
                                {
                                        nameservices = "Новая услуга",
                                        price = 0
                                };

                                context.services.Add(newService);
                                context.SaveChanges();
                                LoadServices();

                                ServicesListView.ScrollIntoView(newService);
                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show($"Ошибка при добавлении услуги: {ex.Message}",
                                              "Ошибка",
                                              MessageBoxButton.OK,
                                              MessageBoxImage.Error);
                        }
                }

                private void EditService_Click(object sender, RoutedEventArgs e)
                {
                        if (!_isAdmin) return;

                        var button = sender as Button;
                        if (button?.Tag is int serviceId)
                        {
                                var service = context.services.Find(serviceId);
                                if (service != null)
                                {
                                        var editWindow = new EditServiceWindow(service);
                                        if (editWindow.ShowDialog() == true)
                                        {
                                                try
                                                {
                                                        service.nameservices = editWindow.ServiceName;
                                                        service.price = editWindow.ServicePrice;
                                                        context.SaveChanges();
                                                        LoadServices();
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

                private void DeleteService_Click(object sender, RoutedEventArgs e)
                {
                        if (!_isAdmin) return;

                        if (MessageBox.Show("Вы уверены, что хотите удалить эту услугу?",
                                          "Подтверждение удаления",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                                var button = sender as Button;
                                if (button?.Tag is int serviceId)
                                {
                                        try
                                        {
                                                var serviceToDelete = context.services.Find(serviceId);
                                                if (serviceToDelete != null)
                                                {
                                                        context.services.Remove(serviceToDelete);
                                                        context.SaveChanges();
                                                        LoadServices();
                                                }
                                        }
                                        catch (Exception ex)
                                        {
                                                MessageBox.Show($"Ошибка при удалении услуги: {ex.Message}\n\nУслуга может быть связана с другими записями.",
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