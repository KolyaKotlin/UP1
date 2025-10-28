using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using _122_Fedchenko.Pages; 
using System.Collections.Generic;

namespace _122_Fedchenko.Pages
{
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
            UpdateUsers();
        }

        private void clearFiltersButton_Click_1(object sender, RoutedEventArgs e)
        {
            fioFilterTextBox.Text = "";
            sortComboBox.SelectedIndex = 0;
            onlyAdminCheckBox.IsChecked = false;
        }

        private void fioFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void onlyAdminCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateUsers();
        }

        private void onlyAdminCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateUsers();
        }

        private void UpdateUsers()
        {
            if (!IsInitialized) return;

            try
            {
                List<User> currentUsers = Entities.GetContext().User.ToList();

                if (!string.IsNullOrWhiteSpace(fioFilterTextBox.Text))
                {
                    currentUsers = currentUsers
                        .Where(u => u.FIO.ToLower().Contains(fioFilterTextBox.Text.ToLower()))
                        .ToList();
                }

                if (onlyAdminCheckBox.IsChecked == true)
                {
                    currentUsers = currentUsers
                        .Where(u => u.Role == "Admin")
                        .ToList();
                }

                ListUser.ItemsSource = (sortComboBox.SelectedIndex == 0)
                    ? currentUsers.OrderBy(u => u.FIO).ToList()
                    : currentUsers.OrderByDescending(u => u.FIO).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении пользователей: " + ex.Message);
            }
        }
    }
}
