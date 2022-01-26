using BetterObsideo.MonoBehaviours;
using HarmonyLib;

namespace BetterObsideo.Patches
{
    [HarmonyPatch(typeof(Breaker))]
    [HarmonyPatch(nameof(Breaker.Start))]
    class BreakerStartPatch
    {
        static void Postfix(Breaker __instance)
        {
            if (__instance.gameObject.GetComponent<BreakerController>() == null)
            {
                //__instance.gameObject.AddComponent<BreakerController>();
            }
        }
    }

    [HarmonyPatch(typeof(Breaker))]
    [HarmonyPatch(nameof(Breaker.LightsOnServerRpc))]
    class BreakerLightsOnServerRpcPatch
    {
        static void Prefix(Breaker __instance, bool on)
        {
            // Disable limits of ligths on
            __instance.lightsOn = 0;
        }

        static void Postfix(Breaker __instance, bool on)
        {
            // Disable limits of ligths on
            __instance.lightsOn = 0;
        }
    }
}
