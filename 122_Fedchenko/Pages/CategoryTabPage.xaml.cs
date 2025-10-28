using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace _122_Fedchenko.Pages
{
    public partial class CategoryTabPage : Page
    {
        public CategoryTabPage()
        {
            InitializeComponent();
            DataGridCategory.ItemsSource = Entities.GetContext().Category.ToList();
            this.IsVisibleChanged += Page_IsVisibleChanged;
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                DataGridCategory.ItemsSource = Entities.GetContext().Category.ToList();
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddCategoryPage(null));
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            var categoryForRemoving = DataGridCategory.SelectedItems.Cast<Category>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить {categoryForRemoving.Count} категорий?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().Category.RemoveRange(categoryForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    DataGridCategory.ItemsSource = Entities.GetContext().Category.ToList();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddCategoryPage((sender as Button).DataContext as Category));
        }
    }
}
