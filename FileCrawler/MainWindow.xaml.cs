using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace FileCrawler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileHandler fileHandler;

        public MainWindow()
        {
            var _startingPoint = @"D:\FILM";

            InitializeComponent();
            fileHandler = new FileHandler();

            DataContext = fileHandler;

            if (Directory.Exists(_startingPoint))
            {
                fileHandler.SourcePath = _startingPoint;
            }
        }

        private void SourceButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog sourceDialog = new FolderBrowserDialog();
            if (sourceDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK && Directory.Exists(sourceDialog.SelectedPath))
            {
                fileHandler.SourcePath = sourceDialog.SelectedPath;
            }
        }

        private void DestinationButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog destinationDialog = new FolderBrowserDialog();
            if (destinationDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK && Directory.Exists(destinationDialog.SelectedPath))
            {
                fileHandler.DestinationPath = destinationDialog.SelectedPath;
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            var value = System.Windows.MessageBox.Show("Är du säker?", "", System.Windows.MessageBoxButton.YesNo);

            if (value == MessageBoxResult.Yes)
            {
                fileHandler.BeginProcessFiles();
            }
        }
    }
}
