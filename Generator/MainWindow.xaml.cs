using Core;
using Core.Export;
using Core.Import;
using Core.Parser;
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
        }



    }
}
