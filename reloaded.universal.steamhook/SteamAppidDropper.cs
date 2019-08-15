using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Indieteur.SAMAPI;

namespace reloaded.universal.steamhook
{
    public static class SteamAppidDropper
    {
        public static void DropSteamId()
        {
            var sam             = new SteamAppsManager();
            var processPath     = Process.GetCurrentProcess().MainModule.FileName;
            foreach (var app in sam.SteamApps)
            {
                if (processPath.Contains(app.InstallDir))
                {
                    Program.Logger.WriteLine($"[SteamHook] Found Steam Library Entry with Id {app.AppID}, containing executable.", Program.Logger.ColorGreenLight);
                    SteamAppId.Write(app.AppID);
                    return;
                }
            }

            Program.Logger.WriteLine("[SteamHook] Not found Application in any Steam library. Oof.", Program.Logger.ColorRedLight);
        }
    }
}
