using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace _122_Fedchenko.Pages
{
    public partial class ChangePassPage : Page
    {
        public ChangePassPage()
        {
            InitializeComponent();
        }

        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password))
                    .Select(x => x.ToString("X2")));
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TbLogin.Text) ||
                string.IsNullOrEmpty(CurrentPasswordBox.Password) ||
                string.IsNullOrEmpty(NewPasswordBox.Password) ||
                string.IsNullOrEmpty(ConfirmPasswordBox.Password))
            {
                MessageBox.Show("Все поля обязательны к заполнению!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var db = new Entities())
            {
                string hashedOldPass = GetHash(CurrentPasswordBox.Password);
                var user = db.User.FirstOrDefault(u => u.Login == TbLogin.Text && u.Password == hashedOldPass);

                if (user == null)
                {
                    MessageBox.Show("Неверный логин или текущий пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (NewPasswordBox.Password != ConfirmPasswordBox.Password)
                {
                    MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool hasUpper = NewPasswordBox.Password.Any(char.IsUpper);
                bool hasDigit = NewPasswordBox.Password.Any(char.IsDigit);

                if (!hasUpper || !hasDigit || NewPasswordBox.Password.Length < 6)
                {
                    MessageBox.Show("Пароль должен содержать минимум 6 символов, хотя бы одну заглавную букву и цифру.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                user.Password = GetHash(NewPasswordBox.Password);
                db.SaveChanges();

                MessageBox.Show("Пароль успешно изменён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.Navigate(new AuthPage());
            }
        }
    }
}
