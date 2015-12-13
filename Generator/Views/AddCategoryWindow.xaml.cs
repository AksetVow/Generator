using System.Windows;

namespace Generator.Views
{
    /// <summary>
    /// Interaction logic for AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        public AddCategoryWindow()
        {
            InitializeComponent();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            IsCancelled = true;
            Close();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            Category = _idTextBox.Text;
            IsCancelled = false;

            Close();
        }

        public string Category
        {
            get;
            private set;
        }

        public bool IsCancelled
        {
            get;
            private set;
        }
    }
}
