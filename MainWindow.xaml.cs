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
using static NeoHanega.GenshinFinder;

namespace NeoHanega
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private String? genshinPath;
        private String? migotoPath;
        private String? fpsunlockerPath;

        private bool enableMigoto = false;
        private bool enableFpsunlocker = false;
        private bool removeUAC = false;

        public MainWindow(String? genshinPath, String? migotoPath, String? fpsPath)
        {
            InitializeComponent();
            if (genshinPath == null)
            {
                GenshinFinder.FindGenshin(updateGenshinPath);
            } else
            {
                this.genshinPath = genshinPath;

                enableMigoto = migotoPath != null;
                this.migotoPath = migotoPath;

                enableFpsunlocker = fpsPath != null;
                this.fpsunlockerPath = fpsPath;
            }

            genshinpath_textbox.Text = genshinPath;

            migotopath_textbox.Text = migotoPath;
            migotopath_textbox.IsEnabled = enableMigoto;
            migotoenable_checkbox.IsChecked = enableMigoto;
            migotoselect_button.IsEnabled = enableMigoto;

            fpsunlockerpath_textbox.Text = fpsPath;
            fpsunlockerpath_textbox.IsEnabled = enableFpsunlocker;
            fpsunlockerenable_checkbox.IsChecked = enableFpsunlocker;
            fpsunlockerselect_button.IsEnabled = enableFpsunlocker;
            }

        private void updateGenshinPath(String? path)
        {
            if(path != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    genshinPath = path += "\\GenshinImpact.exe";
                    genshinpath_textbox.Text = genshinPath;
                });
            }
        }


        private void migotoselect_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog migotoFileDialog = new OpenFileDialog();
            migotoFileDialog.InitialDirectory = migotoPath != null ? migotoPath : Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            migotoFileDialog.Filter = "3D Migoto Executable|3DMigoto Loader.exe|Any Executable|*.exe";
            migotoFileDialog.ShowDialog();
            if (migotoFileDialog.FileName != "")
            {
                migotoPath = migotoFileDialog.FileName;
                migotopath_textbox.Text = migotoFileDialog.FileName;
            }
        }

        private void migotoenable_checkbox_Checked(object sender, RoutedEventArgs e)
        {
            enableMigoto = true;
            migotoselect_button.IsEnabled = true;
            migotopath_textbox.IsEnabled = true;
        }
        private void migotoenable_checkbox_Unhecked(object sender, RoutedEventArgs e)
        {
            enableMigoto = false;
            migotoselect_button.IsEnabled = false;
            migotopath_textbox.IsEnabled = false;
        }


        private void fpsunlockerselect_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fpsunlockerFileDialog = new OpenFileDialog();
            fpsunlockerFileDialog.InitialDirectory = fpsunlockerPath != null ? fpsunlockerPath : Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fpsunlockerFileDialog.Filter = "FPSUnlocker Executable|unlockfps_nc.exe|Any Executable|*.exe";
            fpsunlockerFileDialog.ShowDialog();
            if (fpsunlockerFileDialog.FileName != "")
            {
                fpsunlockerPath = fpsunlockerFileDialog.FileName;
                fpsunlockerpath_textbox.Text = fpsunlockerFileDialog.FileName;
            }
        }

        private void fpsunlockerenable_checkbox_Checked(object sender, RoutedEventArgs e)
        {
            enableFpsunlocker = true;
            fpsunlockerselect_button.IsEnabled = true;
            fpsunlockerpath_textbox.IsEnabled = true;
        }

        private void fpsunlockerenable_checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            enableFpsunlocker = false;
            fpsunlockerselect_button.IsEnabled = false;
            fpsunlockerpath_textbox.IsEnabled = false;
        }


        private void genshinselect_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog genshinFileDialog = new OpenFileDialog();
            genshinFileDialog.InitialDirectory = genshinPath != null ? genshinPath : Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            genshinFileDialog.Filter = "Genshin Impact Executable|GenshinImpact.exe|Any Executable|*.exe";
            genshinFileDialog.ShowDialog();
            if (genshinFileDialog.FileName != "")
            {
                genshinPath = genshinFileDialog.FileName;
                genshinpath_textbox.Text = genshinFileDialog.FileName;
            }
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
            if(genshinPath == null)
            {
                String title_err = "Error: Missing Path";
                String message_err = "Genshin Impact executable must be selected before continuing.";
                MessageBoxButton buttons_err = MessageBoxButton.OK;
                MessageBoxImage icon_err = MessageBoxImage.Error;

                MessageBox.Show(message_err, title_err, buttons_err, icon_err, MessageBoxResult.OK);
                return;
            }

            if(!enableMigoto && !enableFpsunlocker)
            {
                String title_err = "Error: No Mod Selected";
                String message_err = "At least one mod must be enabled before continuing.";
                MessageBoxButton buttons_err = MessageBoxButton.OK;
                MessageBoxImage icon_err = MessageBoxImage.Error;

                MessageBox.Show(message_err, title_err, buttons_err, icon_err, MessageBoxResult.OK);
                return;
            }

            if (enableMigoto && migotoPath == null)
            {
                String title_err = "Error: Missing Path";
                String message_err = "3DMigoto executable must be selected before continuing.";
                MessageBoxButton buttons_err = MessageBoxButton.OK;
                MessageBoxImage icon_err = MessageBoxImage.Error;

                MessageBox.Show(message_err, title_err, buttons_err, icon_err, MessageBoxResult.OK);
                return;
            }

            if (enableFpsunlocker && fpsunlockerPath == null)
            {
                String title_err = "Error: Missing Path";
                String message_err = "FPSUnlocker executable must be selected before continuing.";
                MessageBoxButton buttons_err = MessageBoxButton.OK;
                MessageBoxImage icon_err = MessageBoxImage.Error;

                MessageBox.Show(message_err, title_err, buttons_err, icon_err, MessageBoxResult.OK);
                return;
            }


            if (removeUAC)
            {
                RegistryKey? key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers", true);
                if (key != null)
                {
                    key.SetValue(genshinPath, "~ RUNASINVOKER", RegistryValueKind.String);
                    if(enableMigoto)
                    {
                        key.SetValue(migotoPath, "~ RUNASINVOKER", RegistryValueKind.String);
                    }
                    if(enableFpsunlocker)
                    {
                        key.SetValue(fpsunlockerPath, "~ RUNASINVOKER", RegistryValueKind.String);
                    }
                }

                if (enableMigoto)
                {
                    String migotoFolder = Directory.GetParent(migotoPath).FullName;
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
            }

            if (enableFpsunlocker)
            {
                List<string> migotoDlls = null;
                if (enableMigoto)
                {
                    migotoDlls = new List<string>();
                    String migotoFolder = Directory.GetParent(migotoPath).FullName;
                    migotoDlls.Add(migotoFolder + "\\d3d11.dll");
                    migotoDlls.Add(migotoFolder + "\\d3dcompiler_46.dll");
                    migotoDlls.Add(migotoFolder + "\\nvapi64.dll");
                }


                String fpsunlockerFolder = Directory.GetParent(fpsunlockerPath).FullName;
                FPSUnlockerConfig fpsUnlockerConfig = new FPSUnlockerConfig(genshinPath, migotoDlls);
                string fpsjson = JsonSerializer.Serialize(fpsUnlockerConfig);
                File.WriteAllText(fpsunlockerFolder + "\\fps_config.json", fpsjson);

            }

            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            string exePath = Environment.ProcessPath ?? "";
            string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Launch NeoHanega.lnk";

            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = exePath; // Path to the .exe
            shortcut.Arguments = "--start"; // Add the arguments
            shortcut.IconLocation = exePath; // Use the same icon as the executable
            shortcut.Save();
            Dictionary<String, String> config = new Dictionary<String, String>();
            config["genshinPath"] = genshinPath;
            config["migotoPath"] = enableMigoto ? migotoPath ?? "" : "";
            config["fpsunlockerPath"] = enableFpsunlocker ? fpsunlockerPath ?? "" : "";

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