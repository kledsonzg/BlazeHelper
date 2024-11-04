namespace BlazeHelper.BlazeConnection
{
    internal static class BlazeAPI
    {
        internal static string BlazeLoginLink { get => $"https://blaze.com/api/auth/password?analyticSessionID={DateTimeOffset.Now.ToUnixTimeMilliseconds()}"; }
        internal static readonly string BLAZE_GET_USER_PERSONAL_INFO = "https://blaze.com/api/users/me";
        internal static readonly string BLAZE_GET_USER_EXPERIENCE = "https://blaze.com/api/users/me/xp";
        internal static readonly string BLAZE_GET_USER_WALLETS = "https://blaze.com/api/wallets";
        internal static readonly string BLAZE_BET_ENTER = "https://blaze.com/api/singleplayer-originals/originals/crash_v2/round/enter";
        internal static readonly string BLAZE_CRASH_GAME = "wss://api-gaming.blaze.com/replication/?EIO=3&transport=websocket";
    }
}