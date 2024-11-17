using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NeoHanega
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private String? genshinPath;
        private String? migotoPath;
        private bool removeUAC = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void migotoselect_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog migotoFileDialog = new OpenFileDialog();
            migotoFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            migotoFileDialog.Filter = "3D Migoto Executable|3DMigoto Loader.exe|Any Executable|*.exe";
            migotoFileDialog.ShowDialog();
            migotoPath = migotoFileDialog.FileName;
            migotopath_textbox.Text = migotoFileDialog.FileName;
        }

        private void genshinselect_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog genshinFileDialog = new OpenFileDialog();
            genshinFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            genshinFileDialog.Filter = "Genshin Impact Executable|GenshinImpact.exe|Any Executable|*.exe";
            genshinFileDialog.ShowDialog();
            genshinPath = genshinFileDialog.FileName;
            genshinpath_textbox.Text = genshinFileDialog.FileName;
        }

        private void uac_checkbox_Checked(object sender, RoutedEventArgs e)
        {
            String title = "Warning: Irreversable Action";
            String message = "Hanega will make changes to certain Windows Registry keys and configuration files to remove " +
                "the \"Run as Admin\" pop-up (UAC) requirements from the 3DMigoto and Genshin Impact executables. This action " +
                "will only alter settings related to 3DMigoto and Genshin Impact. Nevertheless, this action may impact system " +
                "functionality, and previous settings cannot be restored by Hanega.\n\nWould you like to proceed?";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            
            MessageBoxResult result = MessageBox.Show(message, title, buttons, icon, MessageBoxResult.No);
            if (result != MessageBoxResult.Yes)
            {
                uac_checkbox.IsChecked = false;
            }
            removeUAC = uac_checkbox.IsChecked.HasValue ? uac_checkbox.IsChecked.Value : false;
        }

        private void ok_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}