using UnityEngine;
using MelonLoader;

namespace PhasmoMelonMod
{
    class Fullbright
    {
        public static void Enable()
        {
            if (Main.boneTransform != null && Main.initializedScene > 1 && isAlreadyOn == false)
            {
                isAlreadyOn = true;
                Main.light = Main.boneTransform.GetComponent<Light>();
                Main.light = Main.boneTransform.gameObject.AddComponent<Light>();
                Main.light.color = Color.white;
                Main.light.type = LightType.Spot;
                Main.light.shadows = LightShadows.None;
                Main.light.range = 99f;
                Main.light.spotAngle = 9999f;
                Main.light.intensity = 0.3f;
            }
            else if (isAlreadyOn == false)
            {
                CheatToggles.enableFullbright = !CheatToggles.enableFullbright;
                MelonLogger.Log("[+] No boneTransform! Fullbright: Toggled " + (CheatToggles.enableFullbright ? "On" : "Off"));
            }
        }

        public static void Disable()
        {
            if (Main.boneTransform.gameObject.GetComponent<Light>())
            {
                Object.Destroy(Main.light);
                isAlreadyOn = false;
            }
        }

        private static bool isAlreadyOn = false;
    }
}
