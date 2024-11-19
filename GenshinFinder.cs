using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NeoHanega
{
    class GenshinFinder
    {
        public static void FindGenshin(Action<String?> callback)
        {
            Task.Run(() =>
            {
                String gamedataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                gamedataPath += "\\Cognosphere\\HYP\\1_0\\data";
                if (!Directory.Exists(gamedataPath) || !File.Exists(gamedataPath + "\\gamedata.dat"))
                {
                    return;
                }
                String gamedata = File.ReadAllText(gamedataPath + "\\gamedata.dat");

                var matches = Regex.Matches(gamedata, @"(\{(?:[^{}]|(?<open>\{)|(?<-open>\}))*?(?(open)(?!))\}|\[.*?\])");

                foreach (Match match in matches)
                {
                    var obj = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(match.Value);
                    if (obj["gameShortcutName"].ToString().Equals("Genshin Impact"))
                    {
                        callback(obj["persistentInstallPath"].ToString());
                        return;
                    }
                }
            });
           return;
        }
    }
}
