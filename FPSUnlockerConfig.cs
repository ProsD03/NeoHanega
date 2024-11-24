using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoHanega
{
    class FPSUnlockerConfig
    {
        public String GamePath { get; set; } 
        public bool AutoStart { get; set; }
        public bool AutoClose { get; set; }
        public bool PopupWindow { get; set; }
        public bool Fullscreen { get; set; }
        public bool UseCustomRes {  get; set; }
        public bool IsExclusiveFullscreen { get; set; }
        public bool StartMinimized { get; set; }
        public bool UsePowerSave { get; set; }
        public bool SuspendLoad {  get; set; }
        public bool UseMobileUI { get; set; }
        public int FPSTarget {  get; set; }
        public int CustomResX { get; set; }
        public int CustomResY { get; set; }
        public int MonitorNum { get; set; }
        public int Priority {  get; set; }
        public List<string> DllList { get; set; }

        public FPSUnlockerConfig(string genshinPath, List<string>? migotoDlls)
        {
            GamePath = genshinPath;
            DllList = migotoDlls ?? new List<string>();

            AutoStart = true;
            AutoClose = true;
            PopupWindow = false;
            Fullscreen = true;
            UseCustomRes = false;
            IsExclusiveFullscreen = false;
            StartMinimized = false;
            UsePowerSave = false;
            SuspendLoad = false;
            UseMobileUI = false;
            FPSTarget = 120;
            CustomResX = 1920;
            CustomResY = 1080;
            MonitorNum = 1;
            Priority = 3;
        }
    }
}
