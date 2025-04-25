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
using VeterinaryClinic.Services;
using VeterinaryClinic.Model;

namespace VeterinaryClinic.Pages
{
        /// <summary>
        /// Логика взаимодействия для Employees.xaml
        /// </summary>
        public partial class Employees : Page
        {
                private readonly VeterinaryСlinicEntities _context;
                private readonly int _userRole;

                public Employees(int userRole)
                {
                        InitializeComponent();
                        _context = new VeterinaryСlinicEntities();
                        _userRole = userRole;
                        LoadEmployees();

                        if (_userRole != 1)
                        {
                                AddEmployeeButton.Visibility = Visibility.Collapsed;
                        }
                }

                private void LoadEmployees()
                {
                        try
                        {
                                EmployeesListView.ItemsSource = _context.employees
                                    .Include(e => e.User)
                                    .Include(e => e.User.role)
                                    .ToList();
                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show($"Ошибка при загрузке сотрудников: {ex.Message}",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                }

                private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
                {
                        try
                        {
                                var newUser = new User
                                {
                                        login = "newuser_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                                        password = Hash.HashPassword("tempPassword123"),
                                        id_role = 2
                                };

                                var newEmployee = new employees
                                {
                                        lastname = "Фамилия",
                                        name = "Имя",
                                        middlename = "Отчество",
                                        address = "Не указан",
                                        phonenumber = "+79000000000",
                                        email = "",
                                        datebirth = DateTime.Now.AddYears(-25),
                                        passportseries = "0000",
                                        passportnumber = "000000",
                                        salary = 30000m,
                                        id_gender = _context.gender.FirstOrDefault()?.id_gender ?? 1,
                                        User = newUser
                                };

                                _context.employees.Add(newEmployee);
                                _context.SaveChanges();
                                LoadEmployees();
                                EmployeesListView.ScrollIntoView(newEmployee);
                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show($"Ошибка при добавлении сотрудника: {ex.Message}\n{ex.InnerException?.Message}",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                }

                private void EditEmployee_Click(object sender, RoutedEventArgs e)
                {
                        var button = sender as Button;
                        if (button?.Tag is int employeeId)
                        {
                                var employee = _context.employees.Find(employeeId);
                                if (employee != null)
                                {
                                        var editWindow = new EditEmployeeWindow(employee, _context);
                                        if (editWindow.ShowDialog() == true)
                                        {
                                                try
                                                {
                                                        _context.SaveChanges();
                                                        LoadEmployees();
                                                }
                                                catch (Exception ex)
                                                {
                                                        MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}",
                                                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                                }
                                        }
                                }
                        }
                }

                private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
                {
                        if (MessageBox.Show("Вы уверены, что хотите удалить этого сотрудника?",
                            "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                                var button = sender as Button;
                                if (button?.Tag is int employeeId)
                                {
                                        try
                                        {
                                                var employeeToDelete = _context.employees.Find(employeeId);
                                                if (employeeToDelete != null)
                                                {
                                                        _context.employees.Remove(employeeToDelete);
                                                        _context.SaveChanges();
                                                        LoadEmployees();
                                                }
                                        }
                                        catch (Exception ex)
                                        {
                                                MessageBox.Show($"Ошибка при удалении сотрудника: {ex.Message}\n\nСотрудник может быть связан с другими записями.",
                                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                        }
                                }
                        }
                }

                private void BackButton_Click(object sender, RoutedEventArgs e)
                {
                        NavigationService.Navigate(new Dashboard(_userRole));
                }
        }
}

