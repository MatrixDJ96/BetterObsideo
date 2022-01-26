using BetterObsideo.MonoBehaviours;
using BetterObsideo.Utility;
using HarmonyLib;
using Steamworks.Data;

namespace BetterObsideo.Patches
{
    [HarmonyPatch(typeof(SteamManager))]
    [HarmonyPatch(nameof(SteamManager.Start))]
    class SteamManagerStartPatch
    {
        static void Postfix(SteamManager __instance)
        {
            if (__instance.gameObject.GetComponent<SteamManagerController>() == null)
            {
                __instance.gameObject.AddComponent<SteamManagerController>();
            }
        }
    }

    [HarmonyPatch(typeof(SteamManager))]
    [HarmonyPatch(nameof(SteamManager.OnLobbyEnteredCallback))]
    class SteamManagerLobbyEnterPatch
    {
        static void Postfix(SteamManager __instance, Lobby lobby)
        {
            if (SteamIdUtility.IsMatrixDJ96Player())
            {
                lobby.InviteFriend(SteamIdUtility.TheSporeFan96);

                //lobby.InviteFriend(SteamIdUtility.Aurora);
                //lobby.InviteFriend(SteamIdUtility.Damiano);
                //lobby.InviteFriend(SteamIdUtility.Micaela);
            }
        }
    }
}
