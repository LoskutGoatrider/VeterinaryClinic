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
using VeterinaryClinic.Services;
using VeterinaryClinic.Model;
using System.Data.Entity.Validation;
using System.Text.RegularExpressions;

namespace VeterinaryClinic
{
        /// <summary>
        /// Логика взаимодействия для EditEmployeeWindow.xaml
        /// </summary>
        public partial class EditEmployeeWindow : Window
        {
                private readonly employees _employee;
                private readonly VeterinaryСlinicEntities _context;

                public EditEmployeeWindow(employees employee, VeterinaryСlinicEntities context)
                {
                        InitializeComponent();
                        _employee = employee;
                        _context = context;
                        DataContext = _employee;

                        // Загрузка данных для ComboBox'ов
                        cmbRole.ItemsSource = _context.role.ToList();
                        cmbGender.ItemsSource = _context.gender.ToList();

                        // Установка значений по умолчанию для новых сотрудников
                        if (_employee.id_employee == 0)
                        {
                                _employee.datebirth = DateTime.Now.AddYears(-30);
                                _employee.passportseries = "0000";
                                _employee.passportnumber = "000000";
                                _employee.salary = 30000m;
                                _employee.id_gender = _context.gender.FirstOrDefault()?.id_gender ?? 1;
                                _employee.address = "Не указан";
                                _employee.email = "";
                        }

                        // Установка выбранных значений
                        cmbRole.SelectedValue = _employee.User?.id_role;
                        cmbGender.SelectedValue = _employee.id_gender;
                }

                private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
                {
                        Regex regex = new Regex("[^0-9]+");
                        e.Handled = regex.IsMatch(e.Text);
                }

                private void DecimalValidationTextBox(object sender, TextCompositionEventArgs e)
                {
                        Regex regex = new Regex("[^0-9.,]+");
                        e.Handled = regex.IsMatch(e.Text);
                }

                private void SaveButton_Click(object sender, RoutedEventArgs e)
                {
                        try
                        {
                                // Валидация обязательных полей согласно таблице
                                if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                                    string.IsNullOrWhiteSpace(txtName.Text) ||
                                    string.IsNullOrWhiteSpace(txtAddress.Text) ||
                                    string.IsNullOrWhiteSpace(txtPhone.Text) ||
                                    string.IsNullOrWhiteSpace(txtPassportSeries.Text) ||
                                    string.IsNullOrWhiteSpace(txtPassportNumber.Text) ||
                                    string.IsNullOrWhiteSpace(txtLogin.Text) ||
                                    txtPassword.Password.Length == 0 ||
                                    cmbRole.SelectedItem == null ||
                                    cmbGender.SelectedItem == null ||
                                    dpBirthDate.SelectedDate == null)
                                {
                                        MessageBox.Show("Пожалуйста, заполните все обязательные поля (помеченные *)",
                                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        return;
                                }

                                if (txtLastName.Text.Length > 50 || txtName.Text.Length > 50 || txtMiddleName.Text.Length > 50)
                                {
                                        MessageBox.Show("ФИО не должно превышать 50 символов",
                                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        return;
                                }

                                if (txtPassportSeries.Text.Length != 4)
                                {
                                        MessageBox.Show("Серия паспорта должна содержать 4 цифры",
                                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        return;
                                }

                                if (txtPassportNumber.Text.Length != 6)
                                {
                                        MessageBox.Show("Номер паспорта должен содержать 6 цифр",
                                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        return;
                                }

                                if (!Regex.IsMatch(txtPhone.Text, @"^\+?\d{10,15}$"))
                                {
                                        MessageBox.Show("Пожалуйста, введите корректный номер телефона (10-15 цифр)",
                                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        return;
                                }

                                if (txtEmail.Text.Length > 100)
                                {
                                        MessageBox.Show("Email не должен превышать 100 символов",
                                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        return;
                                }

                                _employee.lastname = txtLastName.Text.Trim();
                                _employee.name = txtName.Text.Trim();
                                _employee.middlename = txtMiddleName.Text.Trim();
                                _employee.address = txtAddress.Text.Trim();
                                _employee.phonenumber = txtPhone.Text.Trim();
                                _employee.email = txtEmail.Text.Trim();
                                _employee.datebirth = dpBirthDate.SelectedDate.Value;
                                _employee.passportseries = txtPassportSeries.Text.Trim();
                                _employee.passportnumber = txtPassportNumber.Text.Trim();
                                _employee.salary = decimal.Parse(txtSalary.Text);
                                _employee.id_gender = (int)cmbGender.SelectedValue;

                                if (_employee.User == null)
                                {
                                        _employee.User = new User
                                        {
                                                login = txtLogin.Text.Trim(),
                                                password = Hash.HashPassword(txtPassword.Password),
                                                id_role = (int)cmbRole.SelectedValue
                                        };
                                }
                                else
                                {
                                        _employee.User.login = txtLogin.Text.Trim();
                                        if (!string.IsNullOrEmpty(txtPassword.Password))
                                        {
                                                _employee.User.password = Hash.HashPassword(txtPassword.Password);
                                        }
                                        _employee.User.id_role = (int)cmbRole.SelectedValue;
                                }

                                if (_employee.id_employee == 0)
                                {
                                        _context.employees.Add(_employee);
                                }

                                try
                                {
                                        _context.SaveChanges();
                                        DialogResult = true;
                                        Close();
                                }
                                catch (DbEntityValidationException ex)
                                {
                                        var errorMessage = new StringBuilder("Ошибки валидации в базе данных:\n");
                                        foreach (var entityErr in ex.EntityValidationErrors)
                                        {
                                                foreach (var error in entityErr.ValidationErrors)
                                                {
                                                        errorMessage.AppendLine($"- {error.PropertyName}: {error.ErrorMessage}");
                                                }
                                        }
                                        MessageBox.Show(errorMessage.ToString(),
                                            "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);

                                        if (_employee.id_employee == 0)
                                        {
                                                _context.Entry(_employee).State = System.Data.Entity.EntityState.Detached;
                                                if (_employee.User != null)
                                                {
                                                        _context.Entry(_employee.User).State = System.Data.Entity.EntityState.Detached;
                                                }
                                        }
                                }
                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show($"Ошибка при сохранении: {ex.Message}\n\n{ex.InnerException?.Message}",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                }

                private void CancelButton_Click(object sender, RoutedEventArgs e)
                {
                        DialogResult = false;
                        Close();
                }
        }
}

