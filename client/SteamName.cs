using System.Runtime.InteropServices;

static class SteamName
{
    // Steam's flat API. The version suffix depends on the Steamworks SDK the game was built with;
    // v017 covers SDK 1.48+ (2020 onward), which is almost certainly what a current game uses.
    [DllImport("steam_api64", EntryPoint = "SteamAPI_SteamFriends_v017")]
    static extern IntPtr SteamFriends();

    [DllImport("steam_api64", EntryPoint = "SteamAPI_ISteamFriends_GetPersonaName")]
    static extern IntPtr GetPersonaName(IntPtr friends);

    public static string Get()
    {
        try
        {
            IntPtr friends = SteamFriends();
            if (friends != IntPtr.Zero)
            {
                string? name = Marshal.PtrToStringUTF8(GetPersonaName(friends));
                if (!string.IsNullOrWhiteSpace(name))
                    return name;
            }
        }
        catch (Exception) { } // not a Steam build, or a different SDK version
        return Environment.UserName;
    }
}