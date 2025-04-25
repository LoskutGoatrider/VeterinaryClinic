using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using VeterinaryClinic.Services;

namespace VeterinaryClinic.Pages
{
        /// <summary>
        /// Логика взаимодействия для Dashboard.xaml
        /// </summary>
        public partial class Dashboard : Page
        {
                private int _userRole;

                public bool IsAdmin => _userRole == 1;
                public bool IsVet => _userRole == 2;
                public bool IsClient => _userRole == 3;

                public Dashboard(int userRole)
                {
                        InitializeComponent();
                        _userRole = userRole;
                        DataContext = this;
                }

                private void ServicesButton_Click(object sender, RoutedEventArgs e)
                {
                        NavigationService.Navigate(new Services(_userRole));
                }

                private void VeterinariansButton_Click(object sender, RoutedEventArgs e)
                {
                        NavigationService.Navigate(new Veterinarians(_userRole));
                }

                private void AppointmentButton_Click(object sender, RoutedEventArgs e)
                {
                        NavigationService.Navigate(new Appointments(_userRole));
                }

                private void MedicationsButton_Click(object sender, RoutedEventArgs e)
                {
                        NavigationService.Navigate(new Medications(_userRole));
                }

                private void AnimalCardsButton_Click(object sender, RoutedEventArgs e)
                {
                        NavigationService.Navigate(new AnimalCards(_userRole));
                }

                private void EmployeesButton_Click(object sender, RoutedEventArgs e)
                {
                        if (IsAdmin)
                        {
                                NavigationService.Navigate(new Employees(_userRole)); 
                        }
                }

                private void LogoutButton_Click(object sender, RoutedEventArgs e)
                {
                        if (NavigationService?.CanGoBack == true)
                        {
                                while (NavigationService?.CanGoBack == true)
                                {
                                        NavigationService.RemoveBackEntry();
                                }
                        }
                        NavigationService?.Navigate(new Authorization());
                }
        }
}
