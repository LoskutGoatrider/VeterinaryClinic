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
        /// Логика взаимодействия для EditAnimalWindow.xaml
        /// </summary>
        public partial class EditAnimalWindow : Window
        {
                private animal _animal;
                private VeterinaryСlinicEntities _context;

                public EditAnimalWindow(animal animal, VeterinaryСlinicEntities context)
                {
                        InitializeComponent();
                        _animal = animal;
                        _context = context;
                        DataContext = animal;

                        cmbAnimalType.ItemsSource = _context.typeanimal.ToList();
                        cmbClient.ItemsSource = _context.client.ToList();
                }

                private void SaveButton_Click(object sender, RoutedEventArgs e)
                {
                        if (string.IsNullOrWhiteSpace(txtNickname.Text) || txtNickname.Text.Length > 50)
                        {
                                MessageBox.Show("Кличка животного обязательна и не должна превышать 50 символов");
                                return;
                        }

                        if (cmbAnimalType.SelectedItem == null)
                        {
                                MessageBox.Show("Пожалуйста, выберите вид животного");
                                return;
                        }

                        if (cmbClient.SelectedItem == null)
                        {
                                MessageBox.Show("Пожалуйста, выберите владельца");
                                return;
                        }

                        if (!string.IsNullOrWhiteSpace(txtColor.Text) && txtColor.Text.Length > 30)
                        {
                                MessageBox.Show("Окрас не должен превышать 30 символов");
                                return;
                        }

                        if (dpBirthDate.SelectedDate.HasValue && dpBirthDate.SelectedDate.Value > DateTime.Now)
                        {
                                MessageBox.Show("Дата рождения не может быть в будущем");
                                return;
                        }

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
        

