using MelonLoader;
using System;

namespace PhasmoMelonMod
{
    class Debug
    {
        public static void Out(String text)
        {
            if (CheatToggles.enableDebug) { MelonLogger.Log("[*] " + text); }
        }
    }
}
