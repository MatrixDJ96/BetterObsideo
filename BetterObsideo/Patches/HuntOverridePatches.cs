using HarmonyLib;

namespace BetterObsideo.Patches
{
    [HarmonyPatch(typeof(HuntOverride))]
    [HarmonyPatch(nameof(HuntOverride.Update))]
    class HuntOverrideUpdatePatch
    {
        static bool Prefix(HuntOverride __instance)
        {
            return false;
        }
    }
}
