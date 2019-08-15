using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace reloaded.universal.steamhook
{
    public static class Native
    {
        /// <summary>
        /// LoadLibrary
        ///     Loads the specified module into the address space of the calling process.
        ///     The specified module may cause other modules to be loaded.
        /// </summary>
        /// <param name="lpFileName">
        ///     The name of the module. This can be either a library module (a.dll file) 
        ///     or an executable module (.exe file). If the string specifies a full path, 
        ///     the function searches only that path for the module. If the string specifies 
        ///     a relative path or a module name without a path, the function uses the 
        ///     standard search strategy.
        /// </param>
        /// <returns>Handle to the module.</returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibraryW([MarshalAs(UnmanagedType.LPWStr)]string lpFileName);

        /// <summary>
        /// GetProcAddress
        ///     Retrieves the address of an exported function or variable from the specified 
        ///     dynamic-link library (DLL).
        /// </summary>
        /// <param name="hModule">The handle to the module to get function/variable.</param>
        /// <param name="procName">The name of the function or variable for which the address is to be obtained for.</param>
        /// <returns>Address of exported function or variable.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
    }
}
