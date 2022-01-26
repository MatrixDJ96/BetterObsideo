using BetterObsideo.MonoBehaviours;
using BetterObsideo.Utility;
using HarmonyLib;
using Obsideo.Ghosts;
using System.Collections;

namespace BetterObsideo.Patches
{
    [HarmonyPatch(typeof(GhostArea))]
    [HarmonyPatch(nameof(GhostArea.Start))]
    class GhostAreaStartPatch
    {
        static void Postfix(GhostArea __instance)
        {
            if (__instance.gameObject.GetComponent<GhostAreaController>() == null)
            {
                __instance.gameObject.AddComponent<GhostAreaController>();
            }
        }
    }

    [HarmonyPatch(typeof(GhostArea))]
    [HarmonyPatch(nameof(GhostArea.FixedUpdate))]
    class GhostAreaFixedUpdatePatch
    {
        static void Prefix(GhostArea __instance)
        {
            // Disable SetInHunt execution flow
            __instance.SetInHunt = __instance.isInHunt;
        }
    }

    [HarmonyPatch(typeof(GhostArea))]
    [HarmonyPatch(nameof(GhostArea.GradePeriodClientRpc))]
    class GhostAreaGradePeriodClientRpcPatch
    {
        static bool Prefix(GhostArea __instance)
        {
            var controller = __instance.gameObject.GetComponent<GhostAreaController>();

            if (controller != null)
            {
                controller.FixedGracePeriodClientRpc();
            }

            return controller ? false : true;
        }
    }

    [HarmonyPatch(typeof(GhostArea))]
    [HarmonyPatch(nameof(GhostArea.Hunt))]
    class GhostAreaHuntPatch
    {
        static bool Prefix(GhostArea __instance, ref IEnumerator __result)
        {
            var controller = __instance.gameObject.GetComponent<GhostAreaController>();

            if (controller != null)
            {
                __result = controller.FixedHunt();
            }

            return controller ? false : true;
        }
    }

    [HarmonyPatch(typeof(GhostArea))]
    [HarmonyPatch(nameof(GhostArea.EndHuntServerRpc))]
    class GhostAreaEndHuntServerRpcPatch
    {
        static bool Prefix(GhostArea __instance)
        {
            var controller = __instance.gameObject.GetComponent<GhostAreaController>();

            if (controller != null)
            {
                controller.HuntTime = 0;
            }

            return controller ? false : true;
        }
    }

    /*[HarmonyPatch(typeof(GhostArea))]
    [HarmonyPatch(nameof(GhostArea.HuntMechanics))]
    class GhostAreaHuntMechanicsPatchs
    {
        static bool Prefix(GhostArea __instance)
        {
            // TODO
            //DebuggerUtility.WriteMessage($"{__instance.GetType().Name}.HuntMechanics ({__instance.GetInstanceID()})");
            return false;
        }
    }

    [HarmonyPatch(typeof(GhostArea))]
    [HarmonyPatch(nameof(GhostArea.ResetCanHunt))]
    class GhostAreaResetCanHuntPatch
    {
        static bool Prefix(GhostArea __instance)
        {
            // TODO
            //DebuggerUtility.WriteMessage($"{__instance.GetType().Name}.ResetCanHunt ({__instance.GetInstanceID()})");
            return false;
        }
    }*/
}
