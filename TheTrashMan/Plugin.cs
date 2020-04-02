using BeatSaberMarkupLanguage.Settings;
using IPA;
using System.Runtime;
using IPA.Logging;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace TheTrashMan
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        public static Logger Log { get; set; }
        private bool IsInGameCore { get; set; }

        [OnStart]
        public void OnStart()
        {
            GarbageCollector.GCModeChanged += GCModeChanged;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            BSMLSettings.instance.AddSettingsMenu("The Trash Man", "TheTrashMan.Views.settings.bsml", Settings.instance);
        }

        public void OnExit()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            GarbageCollector.GCModeChanged -= GCModeChanged;
        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene) 
            => ApplyGCMode(IsInGameCore = nextScene.name == "GameCore");

        private void GCModeChanged(GarbageCollector.Mode mode) 
            => ApplyGCMode(IsInGameCore);

        private static void ApplyGCMode(bool isInGameCore)
        {
            var gcMode = isInGameCore && Settings.instance.DisableInGameCore 
                ? GarbageCollector.Mode.Disabled
                : GarbageCollector.Mode.Enabled;
            var gcLatencyMode = isInGameCore
                ? Settings.instance.GameCoreMode
                : Settings.instance.MenuMode;

            if (GarbageCollector.GCMode != gcMode) {
                Log.Debug($"GarbageCollector.GCMode: {GarbageCollector.GCMode} -> {gcMode}");
                GarbageCollector.GCMode = gcMode;
            }
            if (GCSettings.LatencyMode != gcLatencyMode) {
                Log.Debug($"GCSettings.LatencyMode: {GCSettings.LatencyMode} -> {gcLatencyMode}");
                GCSettings.LatencyMode = gcLatencyMode;
            }
        }
    }
}
