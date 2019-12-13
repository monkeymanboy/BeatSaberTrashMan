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
        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "MenuViewControllers")
            {
                BSMLSettings.instance.AddSettingsMenu("The Trash Man", "TheTrashMan.Views.settings.bsml", Settings.instance);
                GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
                GCSettings.LatencyMode = Settings.instance.MenuMode;
            }
            if (nextScene.name == "GameCore")
            {
                if (Settings.instance.DisableInGameCore)
                    GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
                GCSettings.LatencyMode = Settings.instance.GameCoreMode;
            }
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode) { }
        public void OnApplicationStart() { }
        public void OnApplicationQuit() { }
        public void OnSceneUnloaded(Scene scene) { }
        public void OnUpdate() { }
        public void OnFixedUpdate() { }
    }
}
