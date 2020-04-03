using System.Runtime;
using BeatSaberMarkupLanguage.Settings;
using IPA;
using IPA.Logging;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace TheTrashMan
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        private Logger Log { get; set; }
        private bool IsInGameCore { get; set; }

        [Init]
        public void Init(Logger log)
        {
            Log = log;
            Log?.Debug("Initialized.");
        }

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
        {
            Log?.Debug($"Scene changed to: {nextScene.name}");
            IsInGameCore = nextScene.name == "GameCore";
            ApplyGCMode();
        }

        private void GCModeChanged(GarbageCollector.Mode mode)
        {
            Log?.Debug($"GC mode changed.");
            ApplyGCMode();
        }

        private void ApplyGCMode()
        {
            var gcMode = IsInGameCore && Settings.instance.DisableInGameCore 
                ? GarbageCollector.Mode.Disabled
                : GarbageCollector.Mode.Enabled;
            var gcLatencyMode = IsInGameCore
                ? Settings.instance.GameCoreMode
                : Settings.instance.MenuMode;

            if (GarbageCollector.GCMode != gcMode) {
                Log?.Debug($"GarbageCollector.GCMode: {GarbageCollector.GCMode} -> {gcMode}");
                GarbageCollector.GCMode = gcMode;
            }
            if (GCSettings.LatencyMode != gcLatencyMode) {
                Log?.Debug($"GCSettings.LatencyMode: {GCSettings.LatencyMode} -> {gcLatencyMode}");
                GCSettings.LatencyMode = gcLatencyMode;
            }
        }
    }
}
