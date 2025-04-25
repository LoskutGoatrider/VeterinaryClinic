﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VeterinaryClinic.Services
{
        internal class ConfirmationCode
        {
                public string SendEmail(string email)
                {
                        try
                        {


                                MailAddress from = new MailAddress("accelerator697@mail.ru", "VetClinic");
                                MailAddress to = new MailAddress(email);
                                MailMessage message = new MailMessage(from, to);
                                message.Subject = "Код подтверждения";
                                string code = new Random().Next(100000, 999999).ToString();
                                message.Body = $"Вот ваш код подтверждения: {code}, никому его не сообщайте";
                                message.IsBodyHtml = true;
                                SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                                smtp.Credentials = new NetworkCredential("accelerator697@mail.ru", "agtXMVNCBdvTbTQSduuM");
                                smtp.EnableSsl = true;
                                smtp.Send(message);
                                return code;
                        }
                        catch (SmtpException smtpEx)
                        {
                                MessageBox.Show($"SMTP ошибка: {smtpEx.Message}");
                                return null;
                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show($"Ошибка: {ex.Message}");
                                return null;
                        }
                }
        }
}
