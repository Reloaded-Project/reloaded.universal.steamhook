using System;
using System.Diagnostics;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Reloaded.Mod.Interfaces.Internal;

namespace reloaded.universal.steamhook
{
    public class Program : IMod, IExports
    {
        public static ILogger Logger;

        private IModLoader _modLoader;
        private WeakReference<IReloadedHooks> _reloadedHooks;
        private SteamHook _steamHook;

        public void Start(IModLoaderV1 loader)
        {
            _modLoader      = (IModLoader)loader;
            _reloadedHooks  = _modLoader.GetController<IReloadedHooks>();
            Logger          = (ILogger) _modLoader.GetLogger();

            /* Your mod code starts here. */
            Logger.WriteLine($"[SteamHook] Checking for {SteamAppId.FileName}.");
            if (!SteamAppId.Exists())
            {
                Logger.WriteLine($"[SteamHook] Does not exist, dropping {SteamAppId.FileName}.");
                SteamAppidDropper.DropSteamId();
            }
            else
            {
                Logger.WriteLine($"[SteamHook] Exists, not overwriting.");
            }

            _steamHook = new SteamHook(_reloadedHooks);
        }

        /* Mod loader actions. */
        public void Suspend() { }
        public void Resume() { }
        public void Unload() { }

        public bool CanUnload() => false;
        public bool CanSuspend() => false;

        public Action Disposing { get; }
        public Type[] GetTypes() => new Type[0];
    }
}
