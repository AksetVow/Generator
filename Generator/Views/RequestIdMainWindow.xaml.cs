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
using System.Windows.Shapes;

namespace Generator.Views
{
    /// <summary>
    /// Interaction logic for RequestIdMainWindow.xaml
    /// </summary>
    public partial class RequestIdMainWindow : Window
    {
        public RequestIdMainWindow()
        {
            InitializeComponent();
        }

        private void OnPreviewTextInput(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);
        }


        private static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);

        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            IsCancelled = true;
            Close();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(_idTextBox.Text))
            {
                result = Int32.Parse(_idTextBox.Text);
            }

            IdMain = result;
            IsCancelled = false;

            Close();
        }

        public int IdMain
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
