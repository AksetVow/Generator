﻿using Core;
using Generator.Utils;
using Generator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private int _minMarkValue = -100;
        private int _maxMarkValue = 100;
        private int startvalue = 0;
        private Article _article;

        public EditArticleWindow(Article article)
        {
            InitializeComponent();

            _article = article;
            _editArticleViewModel = new EditArticleViewModel(article);
            this.DataContext = _editArticleViewModel;
        }

        public Article Article
        {
            get { return _editArticleViewModel.Article; }
        }

        #region EventHandlers

        private void OnIncreaseButtonClick(object sender, RoutedEventArgs e)
        {
            _editArticleViewModel.Mark = (Convert.ToInt32(_editArticleViewModel.Mark) + 1).ToString();
        }

        private void OnDecreaseButtonClick(object sender, RoutedEventArgs e)
        {
            _editArticleViewModel.Mark = (Convert.ToInt32(_editArticleViewModel.Mark) - 1).ToString();
        }

        private void OnMarkPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                _upBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_upBtn, new object[] { true });
            }


            if (e.Key == Key.Down)
            {
                _downBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_downBtn, new object[] { true });
            }
        }

        private void OnMarkPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_upBtn, new object[] { false });

            if (e.Key == Key.Down)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_downBtn, new object[] { false });
        }

        private void OnMarkTextChanged(object sender, TextChangedEventArgs e)
        {
            int number = 0;
            if ((_markTextBox.Text != string.Empty) && (!int.TryParse(_markTextBox.Text, out number)) )
            {
                _editArticleViewModel.Mark = startvalue.ToString();
            }
            if (number > _maxMarkValue) 
            {
                _editArticleViewModel.Mark = _maxMarkValue.ToString(); 
            }
            if (number < _minMarkValue) 
            {
                _editArticleViewModel.Mark = _minMarkValue.ToString(); 
            }
            _markTextBox.SelectionStart = _markTextBox.Text.Length;

        }

        private void OnPreviewTextInput(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = Helper.IsTextNumeric(e.Text);
        }

        #endregion
    }
}
