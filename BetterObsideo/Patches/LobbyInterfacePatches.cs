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
                if (firstBoot && SteamManager.Instance.gameObject.GetComponent<SteamManagerController>() is SteamManagerController controller)
                {
                    firstBoot = false;
                    SteamManager.Instance.createLobbyTypeDropdown.value = 1;
                    SteamManager.Instance.CreateLobby(4);
                    DebuggerUtility.WriteMessage($"Creating Lobby...");
                }
            }
        }
    }
}
