using InspectShift.ViewModels;
using System.IO;
using System.Windows;

namespace InspectShift.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = DataContext as MainWindowViewModel;
        }

        private void btnPickDirectory_Click(object sender, RoutedEventArgs e)
        {
            //if (tbxDirectory.Text == "" || !Directory.Exists(tbxDirectory.Text))
            //    return;
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbxDirectory.Text = dialog.SelectedPath;
                vm.Files = Directory.GetFiles(tbxDirectory.Text, tbxPattern.Text, SearchOption.TopDirectoryOnly);
            }
        }
    }
}
