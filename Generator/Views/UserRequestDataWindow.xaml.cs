using Core.Export;
using Generator.ViewModels;
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
    /// Interaction logic for UserRequestDataWindow.xaml
    /// </summary>
    public partial class UserRequestDataWindow : Window
    {
        private UserRequestDataViewModel _viewModel;

        public UserRequestDataWindow()
        {
            InitializeComponent();
            _viewModel = (UserRequestDataViewModel)base.DataContext;
        }

        public UserRequestData UserRequestData 
        {
            get { return _viewModel.UserRequestData; }
        }

        private void OnContinueButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnWindowKeyDownHandler(object sender, KeyEventArgs e)
        {
            Close();
        }

        
    }
}
