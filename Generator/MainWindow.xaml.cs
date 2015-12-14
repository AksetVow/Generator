using Core;
using Core.Export;
using Core.Import;
using Core.Parser;
using Generator.Command;
using Generator.Utils;
using Generator.Views;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Workspace _workspace;
        private IList<Template> _templates;
        private IList<ImportConfiguration> _importConfigurations;
        private Importer _importer;
        private Exporter _exporter;
        private Core.Command.ICommandManager _commandManager = new Core.Command.CommandManager();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Initialize();
        }

        private void Initialize()
        {
            _importConfigurations = ImportConfigParser.ParseImportSettings("imports.ini");
            _templates = ExportConfigParser.ParseExportSettings("exports.ini");

            _workspace = new Workspace();
            _importer = new Importer();
            _exporter = new Exporter();

            InitializeImportMenu();
            InitializeExportMenu();
            InitializeMenuItems();

            _articles.ItemsSource = _workspace.Articles;
            _articles.PreviewKeyDown += PreviewKeyDownHandler;

        }

        private void InitializeMenuItems()
        {
            var regionItems = MenuItemsReader.GetRegionItems();
            var themeItems = MenuItemsReader.GetThemeItems();

            if (regionItems != null)
            {
                foreach (var item in regionItems)
                {
                    var menuItem = ViewFactory.CreateMenuItem(AddCategoryCommand, item);
                    _regionsMenu.Items.Add(menuItem);
                }
            }

            if (themeItems != null)
            {
                foreach (var item in themeItems)
                {
                    var menuItem = ViewFactory.CreateMenuItem(AddCategoryCommand, item);
                    _themesMenu.Items.Add(menuItem);
                }
            }
        }

        private void InitializeImportMenu()
        {
            ComboBoxItem importItem;

            foreach (var configuration in _importConfigurations)
            {
                importItem = new ComboBoxItem();
                importItem.Content = configuration.Name;
                importItem.Tag = configuration;
                _importCmbbx.Items.Add(importItem);
            }
        }

        private void InitializeExportMenu()
        {
            ComboBoxItem exportItem;

            foreach (var template in _templates)
            {
                exportItem = new ComboBoxItem();
                exportItem.Content = template.Name;
                exportItem.Tag = template;
                _exportCmbbx.Items.Add(exportItem);
            }
        }

        private ImportConfiguration CurrentImportConfiguration
        {
            get
            {
                if (_importCmbbx.SelectedItem == null)
                    return null;
                else
                    return (_importCmbbx.SelectedItem as ComboBoxItem).Tag as ImportConfiguration;
            }
        }

        private Template CurrentTemplate
        {
            get
            {
                if (_exportCmbbx.SelectedItem == null)
                    return null;
                else
                    return (_exportCmbbx.SelectedItem as ComboBoxItem).Tag as Template;
            }

        }

        private bool ContainsImages(IList<Article> articles)
        {
            foreach (var article in articles)
            {
                if (article.Images.Count > 0)
                    return true;
            }
            return false;
        }

        #region EventHandlers

        private void PreviewKeyDownHandler(object sender, KeyEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid != null)
            {
                if (Key.Delete == e.Key)
                {
                    if (CanDeleteArticle())
                    {
                        DeleteArticles();
                        e.Handled = true;
                    }
                }
            }
        }

        private void OnDataGridLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }


        private void OnImportCmbbxSelected(object sender, SelectionChangedEventArgs e)
        {
            if (CurrentImportConfiguration != null)
            {
                _importer.ImportConfiguration = CurrentImportConfiguration;
            }
        }

        private void OnExportCmbbxSelected(object sender, SelectionChangedEventArgs e)
        {
            if (CurrentTemplate != null)
            {
                _exporter.Template = CurrentTemplate;
            }
        }

        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var regionItems = ViewFactory.MenuItems(_regionsMenu);
            var themeItems = ViewFactory.MenuItems(_themesMenu);

            MenuItemsReader.SaveItems(regionItems, themeItems);
        }

        #endregion

        #region Commands

        public ICommand ImportCommand
        {
            get
            {
                return new BaseCommand(Import, CanImport);
            }
        }

        public ICommand ExportCommand
        {
            get
            {
                return new BaseCommand(Export, CanExport);
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new BaseCommand(DeleteArticles, CanDeleteArticle);
            }
        }

        public ICommand DeleteAllCommand
        {
            get
            {
                return new BaseCommand(DeleteAllArticles, CanDeleteAllArticles);
            }
        }

        public ICommand DeleteImageCommand
        {
            get
            {
                return new BaseCommand(DeleteImage, CanDeleteImage);
            }
        }

        public ICommand DeleteAllImagesCommand
        {
            get
            {
                return new BaseCommand(DeleteAllImages, CanDeleteAllImages);
            }
        }

        public ICommand UndoCommand
        {
            get
            {
                return new BaseCommand(Undo, CanUndo);
            }
        }

        public ICommand RedoCommand
        {
            get
            {
                return new BaseCommand(Redo, CandRedo);
            }
        }

        public ICommand ConnectWithSourceCommand
        {
            get
            {
                return new BaseCommand(ConnectWithMain, CanConnectWithMain);
            }
        }

        public ICommand AddCategoryCommand
        {
            get
            {
                return new BaseCommandWithParameter(AddCategory, CanAddCategory);
            }
        }

        #endregion

        #region CommandHandlers

        private bool CanImport()
        {
            return CurrentImportConfiguration != null;
        }

        private void Import()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Files|" + CurrentImportConfiguration.FileMask;
            openFile.Multiselect = true;
            openFile.ShowDialog();
            _commandManager.Import(_importer, _workspace, openFile.FileNames);

            _articles.Items.Refresh();
        }

        private bool CanExport()
        {
            return CurrentTemplate != null && _workspace.Articles.Count > 0;
        }

        private void Export()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "File|*.htm";
            saveFile.ShowDialog();

            var reportFile = saveFile.FileName;

            var userData = new UserRequestDataWindow();
            userData.ShowDialog();

            _exporter.Export(_workspace, reportFile, userData.UserRequestData);
        }

        private void DeleteArticles()
        {
            var articles = _articles.SelectedItems.OfType<Article>().ToList();

            _commandManager.Delete(_workspace, articles);
            _articles.Items.Refresh();
        }

        private bool CanDeleteArticle()
        {
            return _articles.SelectedItem != null;
        }

        private void DeleteAllArticles()
        {
            _commandManager.DeleteAll(_workspace);
            _articles.Items.Refresh();
        }

        private bool CanDeleteAllArticles()
        {
            return _workspace.Articles.Count > 0;
        }

        private void DeleteImage()
        {
            var articles = _articles.SelectedItems.OfType<Article>().ToList();

            _commandManager.DeleteImages(articles);
            _articles.Items.Refresh();
        }

        private bool CanDeleteImage()
        {
            var articles = _articles.SelectedItems.OfType<Article>().ToList();

            return ContainsImages(articles);
        }

        private void DeleteAllImages()
        {
            _commandManager.DeleteImages(_workspace.Articles);
            _articles.Items.Refresh();
        }

        private bool CanDeleteAllImages()
        {
            return ContainsImages(_workspace.Articles);
        }

        private bool CanUndo()
        {
            return _commandManager.CanUndo();
        }

        private void Undo()
        {
            _commandManager.Undo();
            _articles.Items.Refresh();
        }

        private void Redo()
        {
            _commandManager.Redo();
            _articles.Items.Refresh();
        }

        private bool CandRedo()
        {
            return _commandManager.CanRedo();
        }

        private void ConnectWithMain()
        {
            var requestIdWindow = new RequestIdMainWindow();
            requestIdWindow.ShowDialog();

            if (!requestIdWindow.IsCancelled)
            {
                int id = requestIdWindow.IdMain;
                var articles = _articles.SelectedItems.OfType<Article>().ToList();
                _commandManager.SetMainId(articles, id);
            }
            _articles.Items.Refresh();
        }

        private bool CanConnectWithMain()
        {
            return _articles.SelectedItem != null;
        }

        private void AddCategory(object obj)
        {
            string category = "";
            if (obj.Equals("##Region") || obj.Equals("##Themes"))
            {

                var addCategoryWindow = new AddCategoryWindow();
                addCategoryWindow.ShowDialog();

                if (!addCategoryWindow.IsCancelled)
                {
                    category = addCategoryWindow.Category;

                    MenuItem menuItem = ViewFactory.CreateMenuItem(AddCategoryCommand, category);

                    if (obj.Equals("##Region"))
                    {
                        _regionsMenu.Items.Add(menuItem);
                    }
                    else
                    {
                        _themesMenu.Items.Add(menuItem);
                    }

                }

            }
            else
            {
                category = obj.ToString();
            }

            var articles = _articles.SelectedItems.OfType<Article>().ToList();
            _commandManager.AddCategory(articles, category);


            _articles.Items.Refresh();
        }

        private bool CanAddCategory(object obj)
        {
            return _articles.SelectedItem != null;
        }

        #endregion


    }
}
