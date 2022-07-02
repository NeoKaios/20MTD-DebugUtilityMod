using System;
using HarmonyLib;
using flanne.Core;
using flanne;
using UnityEngine;

namespace DebugUtilityMod
{
    [HarmonyPatch]
    class InvincibilityPatch
    {
        static BoolToggle toggleData;
        static bool currentState = false; // Necessary due to run that starts with HolyShield on

        [HarmonyPatch(typeof(InitState), "Exit")]
        [HarmonyPostfix]
        static void InitStateExit_postfix(GameController ___owner)
        {
            if (!DUMPlugin.activateMod.Value) return;
            currentState = false;
            toggleData = ___owner.playerHealth.isInvincible;
            DUMPlugin.hasInvincibility.SettingChanged += ChangePatch;
            if (DUMPlugin.hasInvincibility.Value) ChangePatch(null, null);
        }

        public static void ChangePatch(object sender, EventArgs e)
        {
            if (toggleData == null) return;
            bool isInvicible = DUMPlugin.hasInvincibility.Value;
            if (isInvicible == currentState) return; // No change
            // Change
            currentState = isInvicible;
            if (isInvicible)
                toggleData.Flip();
            else
                toggleData.UnFlip();
            NoUnlockPatch.SetProgressionForbidden();
        }
    }
}
