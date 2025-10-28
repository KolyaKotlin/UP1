using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Security.Cryptography;

namespace _122_Fedchenko.Pages
{
    /// <summary>
    /// Страница регистрации пользователей приложения.
    /// </summary>
    public partial class RegPage : Page
    {
        /// <summary>
        /// Инициализирует компоненты страницы регистрации и задаёт значение по умолчанию для выбора роли.
        /// </summary>
        public RegPage()
        {
            InitializeComponent();
            comboBxRole.SelectedIndex = 0;
        }

        /// <summary>
        /// Возвращает SHA1-хэш для указанного пароля.
        /// </summary>
        /// <param name="password">Строка пароля для хэширования.</param>
        /// <returns>Строка, представляющая хэш пароля в шестнадцатеричном формате.</returns>
        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password))
                    .Select(x => x.ToString("X2")));
            }
        }

        /// <summary>
        /// Обрабатывает нажатие на подсказку логина — переводит фокус на поле ввода логина.
        /// </summary>
        private void lblLogHitn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtbxLog.Focus();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки регистрации. Проверяет корректность введённых данных и добавляет пользователя в базу.
        /// </summary>
        /// <param name="sender">Источник события (кнопка регистрации).</param>
        /// <param name="e">Аргументы события нажатия.</param>
        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtbxLog.Text) ||
                string.IsNullOrEmpty(txtbxFIO.Text) ||
                string.IsNullOrEmpty(passBxFrst.Password) ||
                string.IsNullOrEmpty(passBxScnd.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            using (var db = new Entities())
            {
                var user = db.User.AsNoTracking()
                    .FirstOrDefault(u => u.Login == txtbxLog.Text);
                if (user != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!");
                    return;
                }

                if (passBxFrst.Password.Length < 6)
                {
                    MessageBox.Show("Пароль слишком короткий, должно быть минимум 6 символов!");
                    return;
                }

                bool en = true;
                bool number = false;

                foreach (char c in passBxFrst.Password)
                {
                    if (char.IsDigit(c))
                        number = true;
                    else if (!((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')))
                        en = false;
                }

                if (!en)
                {
                    MessageBox.Show("Используйте только английскую раскладку!");
                    return;
                }
                if (!number)
                {
                    MessageBox.Show("Добавьте хотя бы одну цифру!");
                    return;
                }

                if (passBxFrst.Password != passBxScnd.Password)
                {
                    MessageBox.Show("Пароли не совпадают!");
                    return;
                }

                var userObject = new User
                {
                    FIO = txtbxFIO.Text,
                    Login = txtbxLog.Text,
                    Password = GetHash(passBxFrst.Password),
                    Role = comboBxRole.Text,
                    Photo = "/images/default.jpg"
                };

                db.User.Add(userObject);
                db.SaveChanges();

                MessageBox.Show("Пользователь успешно зарегистрирован!");

                txtbxLog.Clear();
                passBxFrst.Clear();
                passBxScnd.Clear();
                txtbxFIO.Clear();
                comboBxRole.SelectedIndex = 0;
            }
        }
    }
}
