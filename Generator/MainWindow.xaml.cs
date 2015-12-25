using Core;
using Core.Export;
using Core.Import;
using Core.Parser;
using Generator.Command;
using Generator.Utils;
using Generator.Views;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
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
        private ExportCounterSettings _exportSettings = new ExportCounterSettings();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Initialize();
        }

        private void Refresh()
        {
            _articlesDataGrid.Items.Refresh();
        }

        #region Init
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

            _articlesDataGrid.ItemsSource = _workspace.Articles;
            _articlesDataGrid.PreviewKeyDown += PreviewKeyDownHandler;

        }

        private void InitializeMenuItems()
        {
            var regionItems = MenuItemsReader.GetRegionItems();
            var themeItems = MenuItemsReader.GetThemeItems();

            if (regionItems != null)
            {
                foreach (var item in regionItems)
                {
                    var menuItem = ViewHelper.CreateMenuItem(AddCategoryCommand, item);
                    _regionsMenu.Items.Add(menuItem);
                }
            }

            if (themeItems != null)
            {
                foreach (var item in themeItems)
                {
                    var menuItem = ViewHelper.CreateMenuItem(AddCategoryCommand, item);
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
            if (_importCmbbx.Items.Count > 1)
            {
                _importCmbbx.SelectedIndex = 1;
            }
            else if (_importCmbbx.Items.Count > 0)
            {
                _importCmbbx.SelectedIndex = 0;
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
            if (_exportCmbbx.Items.Count > 0)
            {
                _exportCmbbx.SelectedIndex = 0;
            }
        }
        #endregion

        #region Properties
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

        public bool IsAuthorIncluded
        {
            get 
            {
                return _exportSettings.Author;
            }
            set 
            {
                _exportSettings.Author = value;
            }
        }

        public bool IsThemeCategoryIncluded
        {
            get
            {
                return _exportSettings.SubjectCategory;
            }
            set
            {
                _exportSettings.SubjectCategory = value;
            }
        }

        public bool IsSourceCategoryIncluded
        {
            get
            {
                return _exportSettings.Category;
            }
            set
            {
                _exportSettings.Category = value;
            }
        }

        public bool IsPublicDateIncluded
        {
            get
            {
                return _exportSettings.PublicationDate;
            }
            set
            {
                _exportSettings.PublicationDate = value;
            }
        }

        public bool IsSourceIncluded
        {
            get
            {
                return _exportSettings.Source;
            }
            set
            {
                _exportSettings.Source = value;
            }
        }

        public bool IsTitleIncluded
        {
            get
            {
                return _exportSettings.Title;
            }
            set
            {
                _exportSettings.Title = value;
            }
        }

        #endregion 

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
            var regionItems = ViewHelper.MenuItems(_regionsMenu);
            var themeItems = ViewHelper.MenuItems(_themesMenu);

            MenuItemsReader.SaveItems(regionItems, themeItems);
        }

        private void OnItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            Article article = row.Item as Article;
            if (article != null)
            {
                var editArticleWindow = new EditArticleWindow(article.Copy());
                editArticleWindow.ShowDialog();

                if (editArticleWindow.IsSaved)
                {
                    _commandManager.EditArticle(article, editArticleWindow.Article);
                    Refresh();
                }
            }
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

        public ICommand SortAndColorizeCommand
        {
            get 
            {
                return new BaseCommand(Colorize, CanColorize);
            }
        }

        public ICommand UncolorizeCommand
        {
            get
            {
                return new BaseCommand(Uncolorize, CanUncolorize);
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

            Refresh();
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

            _exporter.Export(_workspace, reportFile, userData.UserRequestData, _exportSettings);
        }

        private void DeleteArticles()
        {
            var articles = _articlesDataGrid.SelectedItems.OfType<Article>().ToList();

            _commandManager.Delete(_workspace, articles);
            Refresh();
        }

        private bool CanDeleteArticle()
        {
            return _articlesDataGrid.SelectedItem != null;
        }

        private void DeleteAllArticles()
        {
            _commandManager.DeleteAll(_workspace);
            Refresh();
        }

        private bool CanDeleteAllArticles()
        {
            return _workspace.Articles.Count > 0;
        }

        private void DeleteImage()
        {
            var articles = _articlesDataGrid.SelectedItems.OfType<Article>().ToList();

            _commandManager.DeleteImages(articles);
            Refresh();
        }

        private bool CanDeleteImage()
        {
            var articles = _articlesDataGrid.SelectedItems.OfType<Article>().ToList();

            return ArticleHelper.ContainsImages(articles);
        }

        private void DeleteAllImages()
        {
            _commandManager.DeleteImages(_workspace.Articles);
            Refresh();
        }

        private bool CanDeleteAllImages()
        {
            return ArticleHelper.ContainsImages(_workspace.Articles);
        }

        private bool CanUndo()
        {
            return _commandManager.CanUndo();
        }

        private void Undo()
        {
            _commandManager.Undo();
            Refresh();
        }

        private void Redo()
        {
            _commandManager.Redo();
            Refresh();
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
                var articles = _articlesDataGrid.SelectedItems.OfType<Article>().ToList();
                _commandManager.SetMainId(articles, id);
            }
            Refresh();
        }

        private bool CanConnectWithMain()
        {
            return _articlesDataGrid.SelectedItem != null;
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

                    MenuItem menuItem = ViewHelper.CreateMenuItem(AddCategoryCommand, category);

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

            var articles = _articlesDataGrid.SelectedItems.OfType<Article>().ToList();
            _commandManager.AddCategory(articles, category);


            Refresh();
        }

        private bool CanAddCategory(object obj)
        {
            return _articlesDataGrid.SelectedItem != null;
        }

        private void Colorize()
        {
            var articles = _workspace.Articles.OrderBy(x => x.Title).ThenBy(x => x.Author).ToList();
            _workspace.RemoveAll();
            _workspace.Add(articles);
            ViewHelper.Colorize(_workspace.Articles);

            Refresh();
        }

        private bool CanColorize()
        {
            return _workspace.Articles.Count > 1;
        }

        private void Uncolorize()
        {
            ViewHelper.Uncolorize(_workspace.Articles);
        }

        private bool CanUncolorize()
        {
            return _workspace.Articles.Count > 1;
        }

        #endregion

    }
}
