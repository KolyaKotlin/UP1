using _122_Fedchenko.Pages;
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

namespace _122_Fedchenko
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new AuthPage());
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) =>{DateTimeNow.Text = DateTime.Now.ToString();
     };
            timer.Start();

           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
            else
            {
                MessageBox.Show("Назад перехода нет", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(MessageBox.Show("Вы цверены,что хотите закрыть окно?","Message",MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.No)
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = (ThemeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(selected)) return;

            Application.Current.Resources.MergedDictionaries.Clear();

            string themeFile = selected == "Темная" ? "Dictionary.xaml" : "JoyfulTheme.xaml";

            var uri = new Uri(themeFile, UriKind.Relative);
            ResourceDictionary themeDict = Application.LoadComponent(uri) as ResourceDictionary;

            Application.Current.Resources.MergedDictionaries.Add(themeDict);
        }

    }
}
