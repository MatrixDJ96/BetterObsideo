using BetterObsideo.Utility;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BetterObsideo.Patches
{
    [HarmonyPatch(typeof(SimplePlayerUse))]
    [HarmonyPatch(nameof(SimplePlayerUse.Interact))]
    class SimplePlayerUseInteractPatch
    {
        static bool Prefix(SimplePlayerUse __instance, InputAction.CallbackContext ctx)
        {
            if (ctx.performed && Physics.Raycast(__instance.mainCamera.transform.position, __instance.mainCamera.transform.forward, out var hitInfo, 2f))
            {
                if (hitInfo.collider.gameObject.GetComponent<SimpleOpenClose>() is SimpleOpenClose simple)
                {
                    DebuggerUtility.WriteMessage($"{simple.GetType().Name}.Interact ({simple.GetInstanceID()})", $"{__instance.GetType().Name}");
                    simple.Interact();
                }
                // TODO: Lights
                // TODO: Doors
                else if (hitInfo.collider.gameObject.GetComponent<Fireplace>() is Fireplace fireplace)
                {
                    DebuggerUtility.WriteMessage($"{fireplace.GetType().Name}.Interact ({fireplace.GetInstanceID()})", $"{__instance.GetType().Name}");
                    fireplace.Interact();
                }
                else if (hitInfo.collider.gameObject.GetComponent<Candle>() is Candle candle)
                {
                    DebuggerUtility.WriteMessage($"{candle.GetType().Name}.Interact ({candle.GetInstanceID()})", $"{__instance.GetType().Name}");
                    candle.Interact();
                }
                else if (hitInfo.collider.gameObject.GetComponent<SwitchLight>() is SwitchLight switchLight)
                {
                    DebuggerUtility.WriteMessage($"{switchLight.GetType().Name}.ToggleLightServerRpc ({switchLight.GetInstanceID()})", $"{__instance.GetType().Name}");
                    switchLight.ToggleLightServerRpc();
                }
                else if (hitInfo.collider.gameObject.GetComponent<Breaker>() is Breaker breaker)
                {
                    DebuggerUtility.WriteMessage($"{breaker.GetType().Name}.ToggleLightServerRpc ({breaker.GetInstanceID()})", $"{__instance.GetType().Name}");
                    breaker.ToggleLightServerRpc();
                }
            }

            return false;
        }
    }
}
