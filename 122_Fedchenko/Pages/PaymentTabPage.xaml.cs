using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace _122_Fedchenko.Pages
{
    public partial class PaymentTabPage : Page
    {
        public PaymentTabPage()
        {
            InitializeComponent();
            DataGridPayment.ItemsSource = Entities.GetContext().Payment.ToList();
            this.IsVisibleChanged += Page_IsVisibleChanged;
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                DataGridPayment.ItemsSource = Entities.GetContext().Payment.ToList();
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddPaymentPage(null));
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            var paymentForRemoving = DataGridPayment.SelectedItems.Cast<Payment>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить {paymentForRemoving.Count} платежей?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().Payment.RemoveRange(paymentForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    DataGridPayment.ItemsSource = Entities.GetContext().Payment.ToList();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPaymentPage((sender as Button).DataContext as Payment));
        }
    }
}
