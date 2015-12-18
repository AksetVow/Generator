using Core;
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
    /// Interaction logic for EditArticleWindow.xaml
    /// </summary>
    public partial class EditArticleWindow : Window
    {
        private EditArticleViewModel _editArticleViewModel;

        public EditArticleWindow(Article article)
        {
            InitializeComponent();

            _editArticleViewModel = new EditArticleViewModel(article);
            this.DataContext = _editArticleViewModel;
        }

        public Article Article
        {
            get { return _editArticleViewModel.Article; }
        }
    }
}
