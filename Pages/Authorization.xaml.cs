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
        /// Логика взаимодействия для Authorization.xaml
        /// </summary>
        public partial class Authorization : Page
        {
                public Authorization()
                {
                        InitializeComponent();
                }

                private void btnLogin_Click(object sender, RoutedEventArgs e)
                {
                        string login = txtLogin.Text.Trim();
                        string password = txtPassword.Password.Trim();

                        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) )
                        {
                                MessageBox.Show("Введите логин и пароль");
                                return;
                        }

                        using (var db = new VeterinaryСlinicEntities())
                        {
                                var user = db.User.FirstOrDefault(u => u.login == login);
                                if (user != null)
                                {
                                        string hashedInputPassword = Hash.HashPassword(password);

                                        if (user.password == hashedInputPassword)
                                        {
                                                switch (user.id_role)
                                                {
                                                        case 1: //Администратор
                                                                NavigationService.Navigate(new Dashboard(1));
                                                                break;
                                                        case 2: //Ветеринар
                                                                NavigationService.Navigate(new Dashboard(2));
                                                                break;
                                                        case 3: //Клиент
                                                                NavigationService.Navigate(new Dashboard(3));
                                                                break;
                                                        default:
                                                                MessageBox.Show("Неизвестная роль пользователя");
                                                                break;
                                                }
                                        }
                                        else
                                        {
                                                MessageBox.Show("Неверный логин или пароль");
                                        }
                                }
                                else
                                {
                                        MessageBox.Show("Неверный логин или пароль");
                                }
                        }
                }

                private void CreateAccount_MouseDown(object sender, MouseButtonEventArgs e)
                {
                        NavigationService.Navigate(new Registration());
                }
                private void ForgotPassword_MouseDown(object sender, MouseButtonEventArgs e)
                {
                        NavigationService.Navigate(new PasswordRecovery());
                }
        }
    
}
