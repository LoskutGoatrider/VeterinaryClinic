using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Windows;
using VeterinaryClinic.Model;

namespace VeterinaryClinic.Services
{
        internal class Validator
        {
                public static ValidationResult Validate(
                        string lastName, string firstName, string middleName,
                        string address, string phoneNumber, string email,
                        string login, string password, string confirmPassword,
                        bool isMaleSelected, bool isFemaleSelected)
                {
                        var result = new ValidationResult();

                        if (!isMaleSelected && !isFemaleSelected)
                        {
                                result.AddError("Пожалуйста, выберите пол.");
                        }

                        if (string.IsNullOrWhiteSpace(lastName))
                        {
                                result.AddError("Фамилия обязательна для заполнения.");
                        }

                        if (string.IsNullOrWhiteSpace(firstName))
                        {
                                result.AddError("Имя обязательно для заполнения.");
                        }

                        if (string.IsNullOrWhiteSpace(address))
                        {
                                result.AddError("Адрес обязателен для заполнения.");
                        }

                        if (string.IsNullOrWhiteSpace(phoneNumber))
                        {
                                result.AddError("Телефон обязателен для заполнения.");
                        }

                        if (string.IsNullOrWhiteSpace(login))
                        {
                                result.AddError("Логин обязателен для заполнения.");
                        }

                        if (string.IsNullOrWhiteSpace(password))
                        {
                                result.AddError("Пароль обязателен для заполнения.");
                        }

                        if (login.Length < 4 || login.Length > 20)
                        {
                                result.AddError("Логин должен быть от 4 до 20 символов.");
                        }

                        if (!Regex.IsMatch(login, @"^[a-zA-Z0-9]+$"))
                        {
                                result.AddError("Логин может содержать только латинские буквы и цифры.");
                        }

                        if (password.Length < 8)
                        {
                                result.AddError("Пароль должен содержать минимум 8 символов.");
                        }

                        if (!Regex.IsMatch(password, @"\d") || !Regex.IsMatch(password, "[a-zA-Z]"))
                        {
                                result.AddError("Пароль должен содержать хотя бы одну букву и одну цифру.");
                        }

                        if (password != confirmPassword)
                        {
                                result.AddError("Пароли не совпадают.");
                        }

                        if (!Regex.IsMatch(phoneNumber, @"^\+?[0-9]{10,15}$"))
                        {
                                result.AddError("Некорректный формат телефона. Допустимы только цифры и знак + в начале.");
                        }

                        if (!string.IsNullOrWhiteSpace(email) && !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                        {
                                result.AddError("Некорректный формат email.");
                        }

                        return result;
                }


                public class ValidationResult
                {
                        public bool IsValid => Errors.Count == 0;
                        public List<string> Errors { get; } = new List<string>();

                        public void AddError(string error)
                        {
                                Errors.Add(error);
                        }
                }
        }
}
