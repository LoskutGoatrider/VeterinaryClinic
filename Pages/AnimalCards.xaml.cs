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
        /// Логика взаимодействия для AnimalCards.xaml
        /// </summary>
        public partial class AnimalCards : Page
        {
                private VeterinaryСlinicEntities context;
                private int currentUserRole;

                public bool IsAdminOrVet => currentUserRole == 1 || currentUserRole == 2;

                public AnimalCards(int userRole)
                {
                        InitializeComponent();
                        context = new VeterinaryСlinicEntities();
                        currentUserRole = userRole;
                        DataContext = this;
                        LoadAnimals();
                }

                private void LoadAnimals()
                {
                        try
                        {
                                AnimalsListView.ItemsSource = context.animal
                                    .Include("typeanimal")
                                    .Include("client")
                                    .Include("gender")
                                    .ToList();
                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show($"Ошибка при загрузке карт животных: {ex.Message}",
                                              "Ошибка",
                                              MessageBoxButton.OK,
                                              MessageBoxImage.Error);
                        }
                }

                private void AddAnimalButton_Click(object sender, RoutedEventArgs e)
                {
                        try
                        {
                                var newAnimal = new animal
                                {
                                        nickname = "Новое животное",
                                        datebirth = DateTime.Now,
                                        id_typeanimal = 1, // Default type
                                        id_client = 1, // Default client
                                        id_gender = 1 // Default gender
                                };

                                context.animal.Add(newAnimal);
                                context.SaveChanges();
                                LoadAnimals();

                                AnimalsListView.ScrollIntoView(newAnimal);
                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show($"Ошибка при добавлении животного: {ex.Message}",
                                              "Ошибка",
                                              MessageBoxButton.OK,
                                              MessageBoxImage.Error);
                        }
                }

                private void EditAnimal_Click(object sender, RoutedEventArgs e)
                {
                        if (!IsAdminOrVet) return;

                        var button = sender as Button;
                        if (button?.Tag is int animalId)
                        {
                                var animal = context.animal.Find(animalId);
                                if (animal != null)
                                {
                                        var editWindow = new EditAnimalWindow(animal, context);
                                        if (editWindow.ShowDialog() == true)
                                        {
                                                try
                                                {
                                                        context.SaveChanges();
                                                        LoadAnimals();
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

                private void DeleteAnimal_Click(object sender, RoutedEventArgs e)
                {
                        if (!IsAdminOrVet) return;

                        if (MessageBox.Show("Вы уверены, что хотите удалить это животное?",
                                          "Подтверждение удаления",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                                var button = sender as Button;
                                if (button?.Tag is int animalId)
                                {
                                        try
                                        {
                                                var animalToDelete = context.animal.Find(animalId);
                                                if (animalToDelete != null)
                                                {
                                                        context.animal.Remove(animalToDelete);
                                                        context.SaveChanges();
                                                        LoadAnimals();
                                                }
                                        }
                                        catch (Exception ex)
                                        {
                                                MessageBox.Show($"Ошибка при удалении животного: {ex.Message}\n\nЖивотное может быть связано с другими записями.",
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
        

