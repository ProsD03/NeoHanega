using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Text.Json;
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
            String message = "NeoHanega will make changes to certain Windows Registry keys and configuration files to remove " +
                "the \"Run as Admin\" pop-up (UAC) requirements from the 3DMigoto and Genshin Impact executables. This action " +
                "will only alter settings related to 3DMigoto and Genshin Impact. Nevertheless, this action may impact system " +
                "functionality, and previous settings cannot be restored by NeoHanega.\n\nWould you like to proceed?";
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
            if(genshinPath == null || migotoPath == null)
            {
                String title_err = "Error: Missing Paths";
                String message_err = "Both the 3DMigoto and Genshin Impact executables must be selected before continuing.";
                MessageBoxButton buttons_err = MessageBoxButton.OK;
                MessageBoxImage icon_err = MessageBoxImage.Error;

                MessageBox.Show(message_err, title_err, buttons_err, icon_err, MessageBoxResult.OK);
                return;
            }

            if (removeUAC)
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers", true);
                if (key != null)
                {
                    key.SetValue(genshinPath, "~ RUNASINVOKER", RegistryValueKind.String);
                    key.SetValue(migotoPath, "~ RUNASINVOKER", RegistryValueKind.String);
                }
                String migotoFolder = System.IO.Directory.GetParent(migotoPath).FullName;
                String migotoConfig = migotoFolder + "\\d3dx.ini";
                if (!File.Exists(migotoConfig))
                {
                    String title_war = "Warning: Non-standard Installation";
                    String message_war = "The d3dx.ini file could not be found in the 3DMigoto Folder. 3DMigoto installation might not be standard, " +
                        "and UAC bypass could have failed.";
                    MessageBoxButton buttons_war = MessageBoxButton.OK;
                    MessageBoxImage icon_war = MessageBoxImage.Warning;

                    MessageBox.Show(message_war, title_war, buttons_war, icon_war, MessageBoxResult.OK);
                    return;
                }

                string text = File.ReadAllText(migotoConfig);
                text = text.Replace(";require_admin", "require_admin");
                text = text.Replace("require_admin = true", "require_admin = false");
                File.WriteAllText(migotoConfig, text);
            }

            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            using (StreamWriter writer = new StreamWriter(deskDir + "\\Launch NeoHanega.url"))
            {
                string app = System.Reflection.Assembly.GetExecutingAssembly().Location;
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=file:///" + app + " --start");
                writer.WriteLine("IconIndex=0");
                string link_icon = app.Replace('\\', '/');
                writer.WriteLine("IconFile=" + link_icon);
            }

            Dictionary<String, String> config = new Dictionary<String, String>();
            config["genshinPath"] = genshinPath;
            config["migotoPath"] = migotoPath;

            string localAppdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string configPath = localAppdata + "\\NeoHanega";
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }

            string json = JsonSerializer.Serialize(config);
            File.WriteAllText(configPath + "\\config.json", json);

            String title = "Info: Installation Complete";
            String message = "Hanega installation is now complete. To run the game, simply use the \"Launch Hanega\" shortcut that has " +
            "been created on your desktop. This windows will not appear when Hanega is launched via the shortcut. To " +
            "show this window again, please launch the Hanega.exe executable.\n\nHappy gaming!";
            MessageBoxButton buttons = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;

            MessageBoxResult result = MessageBox.Show(message, title, buttons, icon, MessageBoxResult.OK);
            Application.Current.Shutdown();
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}