using UnityEngine;

namespace BetterObsideo.Utility
{
    public class ComponentUtility
    {
        public static void WriteComponents(Component __instance)
        {
            WriteComponents(__instance.gameObject);
        }

        public static void WriteComponents(GameObject __instance)
        {
            DebuggerUtility.WriteMessage("");
            DebuggerUtility.WriteMessage(__instance.GetType() + ".Name: " + __instance.name + " ("+__instance.tag +")");
            DebuggerUtility.WriteMessage("");

            foreach (var component in __instance.GetComponentsInParent<Component>())
            {
                DebuggerUtility.WriteMessage(__instance.GetType() + ".GetComponentsInParent.Name: " + component.name + " (" + component.tag + ")");
                DebuggerUtility.WriteMessage(__instance.GetType() + ".GetComponentsInParent.Type: " + component.GetType());
                DebuggerUtility.WriteMessage("");
            }

            foreach (var component in __instance.gameObject.GetComponents<Component>())
            {
                DebuggerUtility.WriteMessage(__instance.GetType() + ".GetComponents.Name: " + component.name + " (" + component.tag + ")");
                DebuggerUtility.WriteMessage(__instance.GetType() + ".GetComponents.Type: " + component.GetType());
                DebuggerUtility.WriteMessage("");
            }

            foreach (var component in __instance.gameObject.GetComponentsInChildren<Component>())
            {
                DebuggerUtility.WriteMessage(__instance.GetType() + ".GetComponentsInChildren.Name: " + component.name + " (" + component.tag + ")");
                DebuggerUtility.WriteMessage(__instance.GetType() + ".GetComponentsInChildren.Type: " + component.GetType());
                DebuggerUtility.WriteMessage("");
            }
        }
    }
}
