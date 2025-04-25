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
using System.Windows.Threading;
using VeterinaryClinic.Services;
using VeterinaryClinic.Model;

namespace VeterinaryClinic.Pages
{
        /// <summary>
        /// Логика взаимодействия для PasswordRecovery.xaml
        /// </summary>
        public partial class PasswordRecovery : Page
        {
                private string verificationCode;
                private DateTime codeExpirationTime;
                private int userId;
                private DispatcherTimer timer;
                private int remainingSeconds;
                private readonly VeterinaryСlinicEntities context = new VeterinaryСlinicEntities();
                private readonly Brush originalSendCodeBackground;

                public PasswordRecovery()
                {
                        InitializeComponent();
                        originalSendCodeBackground = btnSendCode.Background; 
                }

                private void BackAuthorization_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
                {
                        NavigationService.Navigate(new Authorization());
                }

                private void btnChangePassword_Click(object sender, RoutedEventArgs e)
                {
                        string newPassword = txtNewPassword.Password;
                        string confirmPassword = txtConfirmPassword.Password;

                        if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
                        {
                                MessageBox.Show("Пожалуйста, введите новый пароль и подтвердите его.");
                                return;
                        }

                        if (newPassword != confirmPassword)
                        {
                                MessageBox.Show("Пароли не совпадают.");
                                return;
                        }

                        try
                        {
                                var user = context.User.FirstOrDefault(u => u.id_user == userId);
                                if (user != null)
                                {
                                        user.password = Hash.HashPassword(newPassword);
                                        context.SaveChanges();
                                        MessageBox.Show("Пароль успешно изменен!");
                                        NavigationService.Navigate(new Authorization());
                                }
                                else
                                {
                                        MessageBox.Show("Ошибка: Пользователь не найден.");
                                }
                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show($"Ошибка при смене пароля: {ex.Message}");
                        }
                }

                private void btnSendCode_Click(object sender, RoutedEventArgs e)
                {
                        string email = txtEmail.Text.Trim();

                        if (string.IsNullOrEmpty(email))
                        {
                                MessageBox.Show("Пожалуйста, введите email");
                                return;
                        }

                        var user = context.client.FirstOrDefault(u => u.email == email);
                        if (user == null)
                        {
                                MessageBox.Show("Пользователь с таким email не найден");
                                return;
                        }

                        userId = user.id_user;

                        try
                        {
                                var emailService = new ConfirmationCode();
                                verificationCode = emailService.SendEmail(email);

                                if (verificationCode != null)
                                {
                                        codeExpirationTime = DateTime.Now.AddMinutes(1); 

                                        codeVerificationPanel.Visibility = Visibility.Visible;
                                        btnVerifyCode.Visibility = Visibility.Visible;

                                        btnSendCode.IsEnabled = false;
                                        btnSendCode.Background = Brushes.Gray;

                                        StartCountdownTimer();
                                        MessageBox.Show("Код подтверждения отправлен на вашу почту");
                                }
                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show($"Ошибка при отправке кода: {ex.Message}");
                        }
                }

                private void StartCountdownTimer()
                {
                        remainingSeconds = 60; 
                        txtTimer.Text = TimeSpan.FromSeconds(remainingSeconds).ToString(@"mm\:ss");

                        timer = new DispatcherTimer();
                        timer.Interval = TimeSpan.FromSeconds(1);
                        timer.Tick += Timer_Tick;
                        timer.Start();
                }

                private void Timer_Tick(object sender, EventArgs e)
                {
                        remainingSeconds--;
                        txtTimer.Text = TimeSpan.FromSeconds(remainingSeconds).ToString(@"mm\:ss");

                        if (remainingSeconds <= 0)
                        {
                                timer.Stop();
                                MessageBox.Show("Время действия кода истекло.");
                                codeVerificationPanel.Visibility = Visibility.Collapsed;
                                btnVerifyCode.Visibility = Visibility.Collapsed;
                                btnSendCode.IsEnabled = true;
                                btnSendCode.Background = originalSendCodeBackground;
                        }
                }

                private void btnVerifyCode_Click(object sender, RoutedEventArgs e)
                {
                        string enteredCode = txtVerificationCode.Text.Trim();

                        if (string.IsNullOrEmpty(enteredCode))
                        {
                                MessageBox.Show("Пожалуйста, введите код подтверждения.");
                                return;
                        }

                        if (enteredCode == verificationCode && DateTime.Now <= codeExpirationTime)
                        {
                                MessageBox.Show("Код подтвержден!");
                                codeVerificationPanel.Visibility = Visibility.Collapsed;
                                btnVerifyCode.Visibility = Visibility.Collapsed;
                                newPasswordPanel.Visibility = Visibility.Visible;
                                btnChangePassword.Visibility = Visibility.Visible;

                                timer?.Stop();
                        }
                        else
                        {
                                MessageBox.Show("Неверный код или время действия кода истекло.");
                        }
                }
        }
}
