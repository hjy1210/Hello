using ModuleLocateRectangle.ViewModels;
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

namespace ModuleLocateRectangle.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class ViewA : UserControl
    {
        ViewAViewModel vm;
        public ViewA()
        {
            InitializeComponent();
            vm = DataContext as ViewAViewModel;
        }
        private void btnPickDirectory_Click(object sender, RoutedEventArgs e)
        {
            //if (tbxDirectory.Text == "" || !Directory.Exists(tbxDirectory.Text))
            //    return;
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbxDirectory.Text = dialog.SelectedPath;
                vm.Files = System.IO.Directory.GetFiles(tbxDirectory.Text, tbxPattern.Text, System.IO.SearchOption.TopDirectoryOnly);
            }
        }

    }
}
