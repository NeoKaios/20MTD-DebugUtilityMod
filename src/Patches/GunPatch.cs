using System.Security.Cryptography;
using System;
using HarmonyLib;
using flanne;
using BepInEx.Configuration;
using UnityEngine;
using System.Collections.Generic;

namespace DebugUtilityMod
{
    [HarmonyPatch]
    class GunPatch
    {
        static BoolToggle infiniteAmmo = null;

        [HarmonyPatch(typeof(Ammo), "Start")]
        [HarmonyPostfix]
        static void AmmoStart_postfix(ref Ammo __instance)
        {
            if (!DUMPlugin.activateMod.Value) return;
            infiniteAmmo = __instance.infiniteAmmo;
            DUMPlugin.hasGunPatch.SettingChanged += ChangePatch;
            if (DUMPlugin.hasGunPatch.Value) ChangePatch(null, null);
        }

        public static void ChangePatch(object sender, EventArgs e)
        {
            if (infiniteAmmo == null) return;
            bool isInfinite = DUMPlugin.hasGunPatch.Value;
            if (isInfinite == infiniteAmmo.value) return; // No change
            // Change
            if (isInfinite)
                infiniteAmmo.Flip();
            else
                infiniteAmmo.UnFlip();
            NoUnlockPatch.SetProgressionForbidden();
        }
    }
}
