using BeatSaberMarkupLanguage.Settings;
using IPA;
using System;
using System.Runtime;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace TheTrashMan
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        private bool isInGameCore;

        [OnStart]
        public void OnStart()
        {
            GarbageCollector.GCModeChanged += GCModeChanged;
            BSMLSettings.instance.AddSettingsMenu("The Trash Man", "TheTrashMan.Views.settings.bsml", Settings.instance);
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        [OnExit]
        public void OnExit()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "MenuViewControllers")
            {
                isInGameCore = false;
                GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
                GCSettings.LatencyMode = Settings.instance.MenuMode;
            }
            if (nextScene.name == "GameCore")
            {
                isInGameCore = true;
                if (Settings.instance.DisableInGameCore)
                    GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
                GCSettings.LatencyMode = Settings.instance.GameCoreMode;
            }
        }

        private void GCModeChanged(GarbageCollector.Mode mode)
        {
            if (Settings.instance.DisableInGameCore && isInGameCore && mode != GarbageCollector.Mode.Disabled)
                GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
        }
    }
}
