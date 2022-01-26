using BetterObsideo.MonoBehaviours;

namespace BetterObsideo.Utility
{
    public static class DebuggerUtility
    {
        public static string Prefix { get; set; } = "BetterObsideo";

        public static string GenerateString(string value, string key, bool prefix = true)
        {
            return (prefix ? "[" + Prefix + "] " : "") + (key != null ? key + ": " : string.Empty) + value;
        }

        public static void ShowMessage(string text, string key = null, bool prefix = true)
        {
            DebuggerController.Instance.AddMessage(new DebuggerController.Message() { Text = text, Prefix = prefix }, key);
        }

        public static void WriteMessage(string text, string key = null, bool prefix = false)
        {
            Plugin.Debugger.LogMessage(GenerateString(text, key, prefix));
        }

        public static void ClearMessages()
        {
            DebuggerController.Instance.ClearMessages();
        }
    }
}
