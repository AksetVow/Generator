using System.Windows;
using System.Windows.Input;

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

        private void Apply()
        {
            Category = _idTextBox.Text;
            IsCancelled = false;

            Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            IsCancelled = true;
            Close();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            Apply();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Apply();
            }
        }
    }
}
