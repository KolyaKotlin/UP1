using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace _122_Fedchenko.Pages
{
    public partial class AddPaymentPage : Page
    {
        private Payment _currentPayment = new Payment();

        public AddPaymentPage(Payment selectedPayment)
        {
            InitializeComponent();

            CBCategory.ItemsSource = Entities.GetContext().Category.ToList();
            CBUser.ItemsSource = Entities.GetContext().User.ToList();

            if (selectedPayment != null)
                _currentPayment = selectedPayment;

            DataContext = _currentPayment;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentPayment.Date.ToString()))
                errors.AppendLine("Укажите дату!");
            if (string.IsNullOrWhiteSpace(_currentPayment.Num.ToString()))
                errors.AppendLine("Укажите количество!");
            if (string.IsNullOrWhiteSpace(_currentPayment.Price.ToString()))
                errors.AppendLine("Укажите цену!");
            if (_currentPayment.UserID == 0)
                errors.AppendLine("Укажите клиента!");
            if (_currentPayment.CategoryID == 0)
                errors.AppendLine("Укажите категорию!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentPayment.ID == 0)
                Entities.GetContext().Payment.Add(_currentPayment);

            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonClean_Click(object sender, RoutedEventArgs e)
        {
            TBPaymentName.Text = "";
            TBAmount.Text = "";
            TBCount.Text = "";
            TBDate.Text = "";
            CBUser.SelectedIndex = -1;
            CBCategory.SelectedIndex = -1;
        }
    }
}
