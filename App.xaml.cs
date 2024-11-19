using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace NeoHanega
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public class NeoHanegaConfig
    {
        public String genshinPath { get; set; }
        public String migotoPath { get; set; }

    }


    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string localAppdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string configPath = localAppdata + "\\NeoHanega";
            NeoHanegaConfig config = new NeoHanegaConfig();
            if (Directory.Exists(configPath))
            {
                if (File.Exists(configPath + "\\config.json"))
                {
                    config = JsonSerializer.Deserialize<NeoHanegaConfig>(File.ReadAllText(configPath + "\\config.json"));
                    if (config != null)
                    {
                        if (File.Exists(config.genshinPath) && File.Exists(config.migotoPath))
                        {
                            if (e.Args.Contains("--start"))
                            {
                                ProcessStartInfo migotoStart = new ProcessStartInfo();
                                migotoStart.FileName = config.migotoPath;
                                migotoStart.WorkingDirectory = System.IO.Directory.GetParent(config.migotoPath).FullName;
                                Process.Start(migotoStart);

                                String migotoProcessName = Path.GetFileName(config.migotoPath).Split('.')[0];

                                while (Process.GetProcessesByName(migotoProcessName).Length < 1)
                                {
                                    System.Threading.Thread.Sleep(100);
                                }

                                Process.Start(config.genshinPath);
                                Application.Current.Shutdown();
                            }
                        }
                    }
                }
            }

            String genshinPath = null;
            String migotoPath = null;

            if(config.genshinPath != null)
            {
                genshinPath = config.genshinPath;
            }
            if(config.migotoPath != null)
            {
                migotoPath = config.migotoPath;
            }

            MainWindow mainWindow = new MainWindow(genshinPath, migotoPath);
            mainWindow.Show();  
        }
    }

}
