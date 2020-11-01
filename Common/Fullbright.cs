using UnityEngine;

namespace PhasmoMelonMod
{
    class Fullbright
    {
        public static void Enable()
        {
                Main.light = Main.boneTransform.gameObject.AddComponent<Light>();
                Main.light.color = Color.white;
                Main.light.type = LightType.Spot;
                Main.light.shadows = LightShadows.None;
                Main.light.range = 99f;
                Main.light.spotAngle = 9999f;
                Main.light.intensity = 0.3f;
        }

        public static void Disable()
        {
            Object.Destroy(Main.light);
        }
    }
}
