using HarmonyLib;
using flanne.Player;
using flanne;

using UnityEngine;
namespace DebugUtilityMod
{
    class GunPatch
    {
        [HarmonyPatch(typeof(IdleState), "Enter")]
        [HarmonyPostfix]
        static void CombatStateEnter_postfix(ref IdleState __instance)
        {
            //Flip invincibility bool
            //__instance.ammo.infiniteAmmo.Flip();

            //((Ammo)Traverse.Create(__instance).Property("ammo").GetValue()).infiniteAmmo.Flip();
            Debug.Log("here idle");
        }

        [HarmonyPatch(typeof(Ammo), "Start")]
        [HarmonyPostfix]
        static void dede(ref Ammo __instance)
        {
            //Flip invincibility bool
            //__instance.ammo.infiniteAmmo.Flip();

            //((Ammo)Traverse.Create(__instance).Property("ammo").GetValue()).infiniteAmmo.Flip();
            Debug.Log("here ammo");
            __instance.infiniteAmmo.Flip();

        }
    }
}
