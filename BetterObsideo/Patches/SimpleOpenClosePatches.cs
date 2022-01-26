using BetterObsideo.MonoBehaviours;
using HarmonyLib;

namespace BetterObsideo.Patches
{
    [HarmonyPatch(typeof(SimpleOpenClose))]
    [HarmonyPatch(nameof(SimpleOpenClose.Start))]
    class SimpleOpenCloseStartPatch
    {
        static void Postfix(SimplePlayerUse __instance)
        {
            if (__instance.gameObject.GetComponent<SimpleOpenCloseController>() == null)
            {
                __instance.gameObject.AddComponent<SimpleOpenCloseController>();
            }
        }
    }

    [HarmonyPatch(typeof(SimpleOpenClose))]
    [HarmonyPatch(nameof(SimpleOpenClose.Interact))]
    class SimpleOpenCloseInteractPatch
    {
        static bool Prefix(SimplePlayerUse __instance)
        {
            var controller = __instance.gameObject.GetComponent<SimpleOpenCloseController>();

            if (controller != null)
            {
                // Bypass running variable
                controller.FixedInteractServerRpc();
            }

            return controller ? false : true;
        }
    }

    [HarmonyPatch(typeof(SimpleOpenClose))]
    [HarmonyPatch(nameof(SimpleOpenClose.OperateDoorServerRpc))]
    class SimpleOpenCloseOperateDoorServerRpctPatch
    {
        static bool Prefix(SimplePlayerUse __instance)
        {
            var controller = __instance.gameObject.GetComponent<SimpleOpenCloseController>();

            if (controller != null)
            {
                controller.FixedInteractServerRpc();
            }

            return controller ? false : true;
        }
    }
}
