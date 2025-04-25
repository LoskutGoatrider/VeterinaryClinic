using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity;
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
        /// Логика взаимодействия для Medications.xaml
        /// </summary>
        public partial class Medications : Page
        {
                private VeterinaryСlinicEntities _context;
                private int _currentUserRole;

                public bool IsAdmin => _currentUserRole == 1;

                public Medications(int userRole)
                {
                        InitializeComponent();
                        _context = new VeterinaryСlinicEntities();
                        _currentUserRole = userRole;
                        DataContext = this;
                        LoadMedications();
                }

                private void LoadMedications()
                {
                        try
                        {
                                _context.medicines.Load();
                                MedicationsListView.ItemsSource = _context.medicines.Local;
                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show($"Ошибка при загрузке медикаментов: {ex.Message}",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                }

                private void AddMedicationButton_Click(object sender, RoutedEventArgs e)
                {
                        try
                        {
                                var newMedication = new medicines
                                {
                                        namemedication = "Новый медикамент",
                                        price = 0.00m,  // Явно указываем тип decimal
                                        quantity = 0,
                                        unitmeasurement = "шт.",
                                        description = ""  // Явно инициализируем строку
                                };

                                // Проверка валидации перед сохранением
                                var validationResult = _context.Entry(newMedication).GetValidationResult();
                                if (!validationResult.IsValid)
                                {
                                        ShowValidationErrors(validationResult);
                                        return;
                                }

                                _context.medicines.Add(newMedication);
                                _context.SaveChanges();
                                LoadMedications();
                                MedicationsListView.ScrollIntoView(newMedication);
                        }
                        catch (DbEntityValidationException ex)
                        {
                                ShowEntityValidationErrors(ex);
                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show($"Ошибка при добавлении медикамента: {ex.Message}",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                }

                private void EditMedication_Click(object sender, RoutedEventArgs e)
                {
                        if (!IsAdmin) return;

                        var button = sender as Button;
                        if (button?.Tag is int medicationId)
                        {
                                var medication = _context.medicines.Find(medicationId);
                                if (medication != null)
                                {
                                        var editWindow = new EditMedicationWindow(medication);
                                        if (editWindow.ShowDialog() == true)
                                        {
                                                try
                                                {
                                                        var validationResult = _context.Entry(medication).GetValidationResult();
                                                        if (!validationResult.IsValid)
                                                        {
                                                                ShowValidationErrors(validationResult);
                                                                return;
                                                        }

                                                        _context.SaveChanges();
                                                        LoadMedications();
                                                }
                                                catch (DbEntityValidationException ex)
                                                {
                                                        ShowEntityValidationErrors(ex);
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

                private void ShowValidationErrors(DbEntityValidationResult validationResult)
                {
                        var errorMessage = "Ошибки валидации:\n";
                        foreach (var error in validationResult.ValidationErrors)
                        {
                                errorMessage += $"- {error.PropertyName}: {error.ErrorMessage}\n";
                        }
                        MessageBox.Show(errorMessage, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                private void ShowEntityValidationErrors(DbEntityValidationException ex)
                {
                        var errorMessage = "Ошибки валидации:\n";
                        foreach (var entityErr in ex.EntityValidationErrors)
                        {
                                foreach (var error in entityErr.ValidationErrors)
                                {
                                        errorMessage += $"- {error.PropertyName}: {error.ErrorMessage}\n";
                                }
                        }
                        MessageBox.Show(errorMessage, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                }


                private void DeleteMedication_Click(object sender, RoutedEventArgs e)
                {
                        if (!IsAdmin) return;

                        if (MessageBox.Show("Вы уверены, что хотите удалить этот медикамент?",
                                          "Подтверждение удаления",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                                var button = sender as Button;
                                if (button?.Tag is int medicationId)
                                {
                                        try
                                        {
                                                var medicationToDelete = _context.medicines.Find(medicationId);
                                                if (medicationToDelete != null)
                                                {
                                                        _context.medicines.Remove(medicationToDelete);
                                                        _context.SaveChanges();
                                                        LoadMedications();
                                                }
                                        }
                                        catch (Exception ex)
                                        {
                                                MessageBox.Show($"Ошибка при удалении медикамента: {ex.Message}\n\nМедикамент может быть связан с другими записями.",
                                                              "Ошибка",
                                                              MessageBoxButton.OK,
                                                              MessageBoxImage.Error);
                                        }
                                }
                        }
                }

                private void BackButton_Click(object sender, RoutedEventArgs e)
                {
                        NavigationService.Navigate(new Dashboard(_currentUserRole));
                }
        }
    
}
        

