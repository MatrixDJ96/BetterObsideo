using BetterObsideo.Utility;
using HarmonyLib;

namespace BetterObsideo.Patches
{
    [HarmonyPatch(typeof(Doors))]
    [HarmonyPatch(nameof(Doors.Start))]
    class DoorsStartPatch
    {
        static void Postfix(Doors __instance)
        {
            DebuggerUtility.WriteMessage($"{__instance.GetType().Name}.Start ({__instance.GetInstanceID()})", $"TestingPatches");
        }
    }

    [HarmonyPatch(typeof(Lights))]
    [HarmonyPatch(MethodType.Constructor)]
    class LightsStartPatch
    {
        static void Postfix(Lights __instance)
        {
            DebuggerUtility.WriteMessage($"{__instance.GetType().Name}.Constructor ({__instance.GetInstanceID()})", $"TestingPatches");
        }
    }
}
