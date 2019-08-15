using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.ReloadedII.Interfaces;

namespace reloaded.universal.steamhook
{
    public class SteamHook
    {
        /* Constants */
        public const string SteamAPI32 = "steam_api.dll";
        public const string SteamAPI64 = "steam_api64.dll";

        public const string RestartAppIfNecessaryName = "SteamAPI_RestartAppIfNecessary";
        public const string IsSteamRunningName        = "SteamAPI_IsSteamRunning";

        /* Hook */
        private IHook<RestartAppIfNecessary> _restartAppIfNecessaryHook;  // Newer games
        private IHook<IsSteamRunning> _isSteamRunningHook;                // Older games


        /* Setup */
        public SteamHook(WeakReference<IReloadedHooks> reloadedHooks)
        {
            if (reloadedHooks.TryGetTarget(out var hooks))
            {
                string steamAPI32 = Path.GetFullPath(SteamAPI32);
                string steamAPI64 = Path.GetFullPath(SteamAPI64);

                IntPtr libraryAddress;            
                IntPtr restartAppIfNecessaryPtr = IntPtr.Zero;
                IntPtr isSteamRunningPtr = IntPtr.Zero;

                switch (IntPtr.Size)
                {
                    case 4 when File.Exists(steamAPI32):
                        libraryAddress           = Native.LoadLibraryW(steamAPI32);
                        restartAppIfNecessaryPtr = Native.GetProcAddress(libraryAddress, RestartAppIfNecessaryName);
                        isSteamRunningPtr        = Native.GetProcAddress(libraryAddress, IsSteamRunningName);
                        break;
                    case 8 when File.Exists(steamAPI64):
                        libraryAddress           = Native.LoadLibraryW(steamAPI64);
                        restartAppIfNecessaryPtr = Native.GetProcAddress(libraryAddress, RestartAppIfNecessaryName);
                        isSteamRunningPtr        = Native.GetProcAddress(libraryAddress, IsSteamRunningName);
                        break;
                }

                Program.Logger.WriteLine($"[SteamHook] Hooking SteamAPI_RestartAppIfNecessary (newer games) and SteamAPI_IsSteamRunning (older games).", Program.Logger.ColorGreenLight);
                if (restartAppIfNecessaryPtr != IntPtr.Zero)
                    _restartAppIfNecessaryHook = hooks.CreateHook<RestartAppIfNecessary>(RestartAppIfNecessaryImpl, (long) restartAppIfNecessaryPtr).Activate();
                else
                    Program.Logger.WriteLine($"[SteamHook] SteamAPI_RestartAppIfNecessary not found.", Program.Logger.ColorYellowLight);

                if (isSteamRunningPtr != IntPtr.Zero)
                    _isSteamRunningHook = hooks.CreateHook<IsSteamRunning>(IsSteamRunningImpl, (long) isSteamRunningPtr).Activate();
                else
                    Program.Logger.WriteLine($"[SteamHook] SteamAPI_IsSteamRunning not found.", Program.Logger.ColorYellowLight);
            }
        }

        private bool IsSteamRunningImpl()
        {
            _isSteamRunningHook.OriginalFunction();
            return true;
        }

        private bool RestartAppIfNecessaryImpl(uint appid)
        {
            // Write the Steam AppID to a local file if not dropped by the other method.
            _restartAppIfNecessaryHook.OriginalFunction(appid);
            SteamAppId.Write((int) appid);

            return false;
        }

        /*
            See: https://partner.steamgames.com/doc/api/steam_api#SteamAPI_RestartAppIfNecessary
        */

        [Reloaded.Hooks.Definitions.X64.Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
        [Reloaded.Hooks.Definitions.X86.Function(Reloaded.Hooks.Definitions.X86.CallingConventions.Cdecl)]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool RestartAppIfNecessary(uint appId);

        [Reloaded.Hooks.Definitions.X64.Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
        [Reloaded.Hooks.Definitions.X86.Function(Reloaded.Hooks.Definitions.X86.CallingConventions.Cdecl)]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool IsSteamRunning();
    }
}
