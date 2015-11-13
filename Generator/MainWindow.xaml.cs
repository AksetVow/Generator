using Core;
using Core.Export;
using Core.Import;
using Core.Parser;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public MainWindow()
        {
            InitializeComponent();

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

            _articles.ItemsSource = _workspace.Articles;
        }

        private void OnImportClick(object sender, RoutedEventArgs e)
        {
            if (CurrentImportConfiguration != null)
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Files|" + CurrentImportConfiguration.FileMask;
                openFile.ShowDialog();

                var articles = _importer.Import(openFile.FileNames);
                _workspace.Add(articles);
                _articles.Items.Refresh();
            }
        }

        private void OnExportClick(object sender, RoutedEventArgs e)
        {
            if (CurrentTemplate != null)
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "File|*.htm";
                saveFile.ShowDialog();

                var reportFile = saveFile.FileName;
                _exporter.Export(_workspace, reportFile, new UserRequestData());

            }
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

    }
}
