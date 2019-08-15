using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace reloaded.universal.steamhook
{
    public static class SteamAppId
    {
        public const string FileName = "steam_appid.txt";

        public static bool Exists()
        {
            return File.Exists(FileName);
        }

        public static void Write(int appId)
        {
            File.WriteAllText(FileName, $"{appId}");
        }
    }
}
