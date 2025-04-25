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
using VeterinaryClinic.Services;

namespace VeterinaryClinic.Pages
{
        /// <summary>
        /// Логика взаимодействия для Registration.xaml
        /// </summary>
        public partial class Registration : Page
        {
                public Registration()
                {
                        InitializeComponent();
                }

                private void Button_Click(object sender, RoutedEventArgs e)
                {

                }
                private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
                {

                }
                private void BtnRegister_Click(object sender, RoutedEventArgs e)
                {
                        var validationResult = Validator.Validate(
                            lastName: txtLastName.Text.Trim(),
                            firstName: txtName.Text.Trim(),
                            middleName: txtMiddleName.Text.Trim(),
                            address: txtAddress.Text.Trim(),
                            phoneNumber: txtPhoneNumber.Text.Trim(),
                            email: txtEmail.Text.Trim(),
                            login: txtLogin.Text.Trim(),
                            password: txtPassword.Password,
                            confirmPassword: txtConfirmPassword.Password,
                            isMaleSelected: rbMale.IsChecked == true,
                            isFemaleSelected: rbFemale.IsChecked == true
                        );

                        if (!validationResult.IsValid)
                        {
                                MessageBox.Show(string.Join("\n", validationResult.Errors));
                                return;
                        }

                        int genderId = rbMale.IsChecked == true ? 1 : 2;

                        using (var db = new VeterinaryСlinicEntities())
                        {
                                if (db.User.Any(u => u.login == txtLogin.Text.Trim()))
                                {
                                        MessageBox.Show("Пользователь с таким логином уже существует.");
                                        return;
                                }

                                var newUser = new User
                                {
                                        login = txtLogin.Text.Trim(),
                                        password = Hash.HashPassword(txtPassword.Password),
                                        id_role = 3,
                                };

                                db.User.Add(newUser);
                                db.SaveChanges();

                                var newClient = new client
                                {
                                        lastname = txtLastName.Text.Trim(),
                                        name = txtName.Text.Trim(),
                                        middlename = txtMiddleName.Text.Trim(),
                                        address = txtAddress.Text.Trim(),
                                        phonenumber = txtPhoneNumber.Text.Trim(),
                                        email = txtEmail.Text.Trim(),
                                        id_user = newUser.id_user,
                                        id_gender = genderId
                                };

                                db.client.Add(newClient);
                                db.SaveChanges();

                                MessageBox.Show("Регистрация прошла успешно.");
                                NavigationService.GoBack();
                        }
                }
                private void BackAuthorization_MouseDown(object sender, MouseButtonEventArgs e)
                {
                        NavigationService.Navigate(new Authorization());
                }

        }         
}
