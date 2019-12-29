using BeatSaberMarkupLanguage.Settings;
using IPA;
using System;
using System.Runtime;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace TheTrashMan
{
    public class Plugin : IBeatSaberPlugin
    {
        private bool isInGameCore;
        public void OnApplicationStart()
        {
            GarbageCollector.GCModeChanged += GCModeChanged;
            BSMLSettings.instance.AddSettingsMenu("The Trash Man", "TheTrashMan.Views.settings.bsml", Settings.instance);
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

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode) { }
        public void OnApplicationQuit() { }
        public void OnSceneUnloaded(Scene scene) { }
        public void OnUpdate() { }
        public void OnFixedUpdate() { }
    }
}
