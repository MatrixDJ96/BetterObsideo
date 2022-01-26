using BepInEx;
using BepInEx.Logging;
using BetterObsideo.Utility;
using BetterObsideo.WinApi;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BetterObsideo
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public partial class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public static ManualLogSource Debugger { get => Instance.Logger; }

        public Harmony Harmony { get; } = new Harmony(PluginInfo.PLUGIN_NAME);

        public bool ShowSettings { get; set; } = false;
        public int CurrentQuality { get; set; } = 1;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;

            // Harmony Patch
            Harmony.PatchAll();

            SceneManager.sceneLoaded += OnSceneLoaded;

            var window = UnsafeNativeMethods.GetActiveWindow();
            var console = UnsafeNativeMethods.GetConsoleWindow();

            DebuggerUtility.WriteMessage($"Set Foreground Window: {window} ({UnsafeNativeMethods.SetForegroundWindow(window)})");
            DebuggerUtility.WriteMessage($"Set Foreground Console: {console} ({UnsafeNativeMethods.SetForegroundWindow(console)})");

            // C:\Program Files (x86)\Steam\steam.exe steam://rungameid/1708460
        }

        void OnGUI()
        {
            if (ShowSettings)
            {
                GUILayout.BeginVertical();

                string[] names = QualitySettings.names;

                for (int i = 0; i < names.Length; i++)
                {
                    if (GUILayout.Button(names[i]))
                    {
                        CurrentQuality = i;
                        QualitySettings.SetQualityLevel(i, true);
                    }
                }

                GUILayout.EndVertical();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                ShowSettings = !ShowSettings;
            }

            if (Input.GetKeyDown(KeyCode.PageDown))
            {
                WindowPlacement result = default;
                var window = UnsafeNativeMethods.GetActiveWindow();

                if (UnsafeNativeMethods.GetWindowPlacement(window, ref result))
                {
                    if (result.ShowCmd != ShowWindowCommands.Minimize)
                    {
                        result.ShowCmd = result.ShowCmd = ShowWindowCommands.Minimize;
                        UnsafeNativeMethods.SetWindowPlacement(window, ref result);
                    }
                }
            }
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            QualitySettings.SetQualityLevel(CurrentQuality, true);
            DebuggerUtility.WriteMessage($"Set Quality Level to {QualitySettings.names[CurrentQuality]}");
        }
    }
}
