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
        /// Логика взаимодействия для Veterinarians.xaml
        /// </summary>
        public partial class Veterinarians : Page
        {
                private readonly VeterinaryСlinicEntities _context;
                private readonly int _userRole;

                public Veterinarians(int userRole)
                {
                        InitializeComponent();
                        _context = new VeterinaryСlinicEntities();
                        _userRole = userRole;
                        LoadVeterinarians();
                }

                private void LoadVeterinarians()
                {
                        try
                        {
                                VeterinariansListView.ItemsSource = _context.employees
                                    .Where(e => e.User.id_role == 2)
                                    .ToList();
                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show($"Ошибка при загрузке ветеринаров: {ex.Message}",
                                              "Ошибка",
                                              MessageBoxButton.OK,
                                              MessageBoxImage.Error);
                        }
                }

                private void BackButton_Click(object sender, RoutedEventArgs e)
                {
                        NavigationService.Navigate(new Dashboard(_userRole));
                }
        }
}


