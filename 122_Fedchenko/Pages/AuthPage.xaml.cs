using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _122_Fedchenko.Pages
{
    public partial class AuthPage : Page
    {
        private int failedAttempts = 0;

        public AuthPage()
        {
            InitializeComponent();
        }

        public void CaptchaSwitch()
        {
            if (captcha.Visibility == Visibility.Visible)
            {
                TextBoxLogin.Clear();
                PasswordBox.Clear();
                captcha.Visibility = Visibility.Hidden;
                captchaInput.Visibility = Visibility.Hidden;
                labelCaptcha.Visibility = Visibility.Hidden;
                submitCaptcha.Visibility = Visibility.Hidden;

                labelLogin.Visibility = Visibility.Visible;
                labelPass.Visibility = Visibility.Visible;
                TextBoxLogin.Visibility = Visibility.Visible;
                txtHintLogin.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Visible;
                txtHintPass.Visibility = Visibility.Visible;
                ButtonChangePassword.Visibility = Visibility.Visible;
                ButtonEnter.Visibility = Visibility.Visible;
                ButtonReg.Visibility = Visibility.Visible;
            }
            else
            {
                captcha.Visibility = Visibility.Visible;
                captchaInput.Visibility = Visibility.Visible;
                labelCaptcha.Visibility = Visibility.Visible;
                submitCaptcha.Visibility = Visibility.Visible;

                labelLogin.Visibility = Visibility.Hidden;
                labelPass.Visibility = Visibility.Hidden;
                TextBoxLogin.Visibility = Visibility.Hidden;
                txtHintLogin.Visibility = Visibility.Hidden;
                PasswordBox.Visibility = Visibility.Hidden;
                txtHintPass.Visibility = Visibility.Hidden;
                ButtonChangePassword.Visibility = Visibility.Hidden;
                ButtonEnter.Visibility = Visibility.Hidden;
                ButtonReg.Visibility = Visibility.Hidden;
            }
        }

        public void CaptchaChange()
        {
            string allowchar = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder pwd = new StringBuilder();
            Random r = new Random();
            for (int i = 0; i < 6; i++)
                pwd.Append(allowchar[r.Next(allowchar.Length)]);
            captcha.Text = pwd.ToString();
        }

        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password))
                    .Select(x => x.ToString("X2")));
            }
        }

        private void ButtonEnter_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) ||
                string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            string hashedPassword = GetHash(PasswordBox.Password);
            using (var db = new Entities())
            {
                var user = db.User
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Login == TextBoxLogin.Text && u.Password == hashedPassword);

                if (user == null)
                {
                    failedAttempts++;
                    MessageBox.Show("Неверный логин или пароль!");

                    if (failedAttempts >= 3)
                    {
                        if (captcha.Visibility != Visibility.Visible)
                        {
                            CaptchaSwitch();
                        }
                        CaptchaChange();
                    }

                    return;
                }

                failedAttempts = 0;
                MessageBox.Show($"Добро пожаловать, {user.Login}!");

                switch (user.Role)
                {
                    case "User":
                        NavigationService?.Navigate(new UserPage());
                        break;
                    case "Admin":
                        NavigationService?.Navigate(new AdminPage());
                        break;
                }
            }
        }

        private void submitCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if (captchaInput.Text != captcha.Text)
            {
                MessageBox.Show("Неверно введена капча!", "Ошибка");
                CaptchaChange();
            }
            else
            {
                MessageBox.Show("Капча введена успешно, можно продолжить вход.", "Успех");
                CaptchaSwitch();
                failedAttempts = 0;
            }
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void ButtonReg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegPage());
        }

        private void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ChangePassPage());
        }
    }
}
