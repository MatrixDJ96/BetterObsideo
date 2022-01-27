using BetterObsideo.MonoBehaviours;
using BetterObsideo.Utility;
using HarmonyLib;

namespace BetterObsideo.Patches
{
    [HarmonyPatch(typeof(LobbyInterface))]
    [HarmonyPatch(nameof(LobbyInterface.Start))]
    class LobbyInterfaceStartPatch
    {
        static bool firstBoot = true;

        static void Postfix(LobbyInterface __instance)
        {
            if (SteamIdUtility.IsMatrixDJ96Player())
            {
                if (firstBoot)
                {
                    firstBoot = false;
                    SteamManager.Instance.Tutorial();
                    DebuggerUtility.WriteMessage($"Starting tutorial...");
                }
            }
        }
    }
}
