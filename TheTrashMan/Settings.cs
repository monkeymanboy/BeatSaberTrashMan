using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using BS_Utils.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace TheTrashMan
{
    class Settings : PersistentSingleton<Settings>
    {
        private Config config;

        [UIParams]
        private BSMLParserParams parserParams;

        [UIValue("modes")]
        private List<object> options = (new object[]{ GCLatencyMode.Batch, GCLatencyMode.Interactive, GCLatencyMode.LowLatency, GCLatencyMode.SustainedLowLatency }).ToList();

        [UIValue("disable-in-gamecore")]
        public bool DisableInGameCore
        {
            get => config.GetBool("General", "Disable During Gameplay", false);
            set => config.SetBool("General", "Disable During Gameplay", value);
        }

        private GCLatencyMode gamecoreMode;
        [UIValue("gamecore-mode")]
        public GCLatencyMode GameCoreMode
        {
            get => gamecoreMode;
            set
            {
                gamecoreMode = value;
                config.SetString("Modes", "Gameplay", gamecoreMode.ToString());
            }
        }

        private GCLatencyMode menuMode;
        [UIValue("menu-mode")]
        public GCLatencyMode MenuMode
        {
            get => menuMode;
            set
            {
                menuMode = value;
                config.SetString("Modes", "Menu", menuMode.ToString());
            }
        }
        
        public void Awake()
        {
            config = new Config("The Trash Man");
            if (Enum.TryParse(config.GetString("Modes", "Gameplay", "LowLatency"), out GCLatencyMode gamecoreParsed))
                gamecoreMode = gamecoreParsed;
            else
                gamecoreMode = GCLatencyMode.LowLatency;
            if (Enum.TryParse(config.GetString("Modes", "Menu", "Interactive"), out GCLatencyMode menuParsed))
                menuMode = menuParsed;
            else
                menuMode = GCLatencyMode.Interactive;
        }
        [UIAction("recommended-click")]
        public void RecommendedPreset()
        {
            DisableInGameCore = false;
            GameCoreMode = GCLatencyMode.LowLatency;
            MenuMode = GCLatencyMode.Interactive;
            parserParams.EmitEvent("cancel");
        }
        [UIAction("default-click")]
        public void DefaultPreset()
        {
            DisableInGameCore = false;
            GameCoreMode = GCLatencyMode.Interactive;
            MenuMode = GCLatencyMode.Interactive;
            parserParams.EmitEvent("cancel");
        }

        [UIAction("donate-click")]
        public void Donate()
        {
            System.Diagnostics.Process.Start("https://ko-fi.com/monkeymanboy");
        }
    }
}
