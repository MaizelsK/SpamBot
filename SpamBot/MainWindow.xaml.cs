using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
using System.ComponentModel.DataAnnotations;

namespace SpamBot
{
    public partial class MainWindow : Window
    {
        private string filePath;
        private bool isFileAttached;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SendBtnClick(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                string fromEmail = FromTextBox.Text;
                string toEmail = ToTextBox.Text;
                string theme = ThemeTextBox.Text;
                string text = MessageTextBox.Text;

                SmtpClient smtpClient = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("smtptesteritstep@gmail.com", "167AEq!!"),
                };

                MailMessage message = new MailMessage()
                {
                    From = new MailAddress("smtptesteritstep@gmail.com", "Kirill"),
                    Subject = theme,
                    Body = text,
                };
                message.To.Add(new MailAddress(toEmail));

                if (isFileAttached)
                    message.Attachments.Add(new Attachment(filePath));

                try
                {
                    smtpClient.SendMailAsync(message);

                    isFileAttached = false;
                    filePath = "";
                    FileNameTextBlock.Text = "";

                    StatusText.Foreground = Brushes.GreenYellow;
                    StatusText.Text = "Сообщение успешно отправлено!";
                }
                catch (SmtpException ex)
                {
                    StatusText.Text = ex.Message;
                }
            }
        }

        #region Validation
        private bool ValidateForm()
        {
            StatusText.Text = "";
            StatusText.Foreground = new SolidColorBrush(Color.FromRgb(240,0,0));

            string fromEmail = FromTextBox.Text;
            string toEmail = ToTextBox.Text;
            string theme = ThemeTextBox.Text;
            string text = MessageTextBox.Text;

            if (!IsValidEmail(fromEmail))
            {
                StatusText.Text = "Некорректный адрес отправителя";
                return false;
            }

            if (!IsValidEmail(toEmail))
            {
                StatusText.Text = "Некорректный адрес получателя";
                return false;
            }

            if (String.IsNullOrWhiteSpace(theme)
                && String.IsNullOrWhiteSpace(text))
            {
                StatusText.Text = "Нужно ввести как минимум тему или сообщение";
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            EmailAddressAttribute attribute = new EmailAddressAttribute();

            return attribute.IsValid(email);
        }
        #endregion

        private void AddFileBtnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                isFileAttached = true;

                FileNameTextBlock.Text = filePath;
            }
        }
    }
}
