using HarmonyLib;
using flanne.Player;
using flanne;
using UnityEngine;

namespace DebugUtilityMod
{
    class GunPatch
    {

        [HarmonyPatch(typeof(Ammo), "Start")]
        [HarmonyPostfix]
        static void AmmoStart_postfix(ref Ammo __instance)
        {
            __instance.infiniteAmmo.Flip();

        }
    }
}
