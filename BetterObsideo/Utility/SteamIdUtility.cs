using Steamworks;

namespace BetterObsideo.Utility
{
    public static class SteamIdUtility
    {
        public static SteamId MatrixDJ96 => 76561198032287551u;
        public static SteamId TheSporeFan96 => 76561199129849393u;

        public static SteamId Aurora => 76561198809571719u;
        public static SteamId Damiano => 76561199047571797u;
        public static SteamId Micaela => 76561199038227874u;

        public static bool IsMatrixDJ96Player(SteamId? steamId = null)
        {
            return (steamId ?? SteamClient.SteamId) == MatrixDJ96;
        }
    }
}
