using BetterObsideo.Utility;
using HarmonyLib;
using Obsideo.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerController = BetterObsideo.MonoBehaviours.PlayerController;

namespace BetterObsideo.Patches
{
    // TODO: Collider!!!

    [HarmonyPatch(typeof(FirstPersonController))]
    [HarmonyPatch(nameof(FirstPersonController.Start))]
    class FirstPersonControllerStartPatch
    {
        static void Postfix(FirstPersonController __instance)
        {
            if (__instance.gameObject.GetComponent<PlayerController>() == null)
            {
                __instance.gameObject.AddComponent<PlayerController>();
            }

            foreach (var light in __instance.gameObject.GetComponentsInChildren<Light>())
            {
                // Disable bugged "face" light
                if (light.name == "Area Light")
                {
                    light.gameObject.SetActive(false);
                }
            }
        }
    }

    [HarmonyPatch(typeof(FirstPersonController))]
    [HarmonyPatch(nameof(FirstPersonController.Update))]
    class FirstPersonControllerUpdatePatch
    {
        static void Postfix(FirstPersonController __instance)
        {
            if (PlayerController.Instance is PlayerController controller && controller.InstanceID == __instance.GetInstanceID())
            {
                PlayerController.Instance.FixMovementAndStamina();
            }
        }
    }

    [HarmonyPatch(typeof(FirstPersonController))]
    [HarmonyPatch(nameof(FirstPersonController.Interact))]
    class FirstPersonControllerInteractPatch
    {
        static bool Prefix(FirstPersonController __instance, InputAction.CallbackContext ctx)
        {
            if (__instance.IsLocalPlayer && ctx.performed && Physics.Raycast(__instance.cameraTransform.position, __instance.cameraTransform.forward, out var hitInfo, 2f))
            {
                if (hitInfo.collider.gameObject.GetComponent<TVController>() is TVController tv)
                {
                    DebuggerUtility.WriteMessage($"{tv.GetType().Name}.Interact ({tv.GetInstanceID()})", $"{__instance.GetType().Name}");
                    tv.Interact();
                }
                else if (hitInfo.collider.gameObject.GetComponent<SinkController>() is SinkController sink)
                {
                    DebuggerUtility.WriteMessage($"{sink.GetType().Name}.Interact ({sink.GetInstanceID()})", $"{__instance.GetType().Name}");
                    sink.Interact();
                }
                else if (hitInfo.collider.gameObject.GetComponent<Oven>() is Oven oven)
                {
                    DebuggerUtility.WriteMessage($"{oven.GetType().Name}.Interact ({oven.GetInstanceID()})", $"{__instance.GetType().Name}");
                    oven.Interact();
                }
                else if (hitInfo.collider.gameObject.GetComponent<Kettle>() is Kettle kettle)
                {
                    DebuggerUtility.WriteMessage($"{kettle.GetType().Name}.Interact ({kettle.GetInstanceID()})", $"{__instance.GetType().Name}");
                    kettle.Interact();
                }
                else if (hitInfo.collider.gameObject.GetComponent<GhostUseRadio>() is GhostUseRadio radio)
                {
                    DebuggerUtility.WriteMessage($"{radio.GetType().Name}.Interact ({radio.GetInstanceID()})", $"{__instance.GetType().Name}");
                    radio.Interact();
                }
                else if (hitInfo.collider.gameObject.GetComponent<EndTheGame>() is EndTheGame end)
                {
                    DebuggerUtility.WriteMessage($"{end.GetType().Name}.Interact ({end.GetInstanceID()})", $"{__instance.GetType().Name}");
                    end.CloseDoorServerRpc();
                }
            }

            return false;
        }
    }
}
