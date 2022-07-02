using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MTDUI;

namespace DebugUtilityMod
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class DUMPlugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> activateMod;


        public static ConfigEntry<bool> hasXPPatch;
        public static ConfigEntry<float> XPmult;
        public static ConfigEntry<int> maxPlayerLevel;
        public static ConfigEntry<bool> hasFastGame;
        public static ConfigEntry<float> gametimerMult;
        public static ConfigEntry<bool> hasWeakBossesAndElites;
        public static ConfigEntry<bool> hasInvincibility;
        public static ConfigEntry<bool> hasGunPatch;
        public static ConfigEntry<bool> hasInfiniteReroll;

        public void Awake()
        {
            activateMod = Config.Bind("Activation", "D.U.M.", true, "If false, the mod does not load");
            MTDUI.ModOptions.RegisterOptionInModList(activateMod);
            if (!activateMod.Value)
            {
                Logger.LogInfo("<Inactive>");
                return;
            }

            hasXPPatch = Config.Bind("XP Patch", "XP Patch activation", false, "Set to True to activate XP Patch");
            XPmult = Config.Bind("XP Patch", "XP multiplier", 2f, "Amount of multiplication bonus applied to XP pickup, 1 is baseXP");
            maxPlayerLevel = Config.Bind("XP Patch", "Max level reachable", 100, "Level after which the player stop receiving XP");

            hasFastGame = Config.Bind("Fast GameTimer", "FastGame activation", false, "Set to True to activate faster game");
            gametimerMult = Config.Bind("Fast GameTimer", "GameTimer speed", 2f, "Increase the speed of the game; x is baseTime/x, 1 is baseTime");

            hasWeakBossesAndElites = Config.Bind("Enemy", "Weak Bosses and Elite", false, "If active, Bosses and Elite have 100 HP");

            hasInvincibility = Config.Bind("Invincibility", "Invincibility", false, "If active, the player cannot take damage");
            hasGunPatch = Config.Bind("Gun", "Infinite Ammo", false, "If active, infinite ammo");
            hasInfiniteReroll = Config.Bind("Reroll", "Infinite Reroll", false, "If active, every character can reroll indefinitly");


            try
            {
                string mod = "D.U.M.";
                ModOptions.Register(hasXPPatch, location: ConfigEntryLocationType.MainOnly, subMenuName: mod);
                ModOptions.Register(XPmult, new List<float>() { 1f, 2f, 5f, 10f, 100f }, ConfigEntryLocationType.MainOnly, subMenuName: mod);
                ModOptions.Register(maxPlayerLevel, new List<int>() { 0, 10, 100, 1000 }, ConfigEntryLocationType.MainOnly, subMenuName: mod);
                ModOptions.Register(hasFastGame, location: ConfigEntryLocationType.MainOnly, subMenuName: mod);
                ModOptions.Register(gametimerMult, new List<float>() { 0.5f, 2f, 5f, 10f, 20f }, location: ConfigEntryLocationType.MainOnly, subMenuName: mod);
                ModOptions.Register(hasWeakBossesAndElites, location: ConfigEntryLocationType.MainOnly, subMenuName: mod);
                ModOptions.Register(hasInvincibility, subMenuName: mod);
                ModOptions.Register(hasGunPatch, subMenuName: mod);
                ModOptions.Register(hasInfiniteReroll, subMenuName: mod);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

            try
            {
                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            }
            catch
            {
                Logger.LogError($"{PluginInfo.PLUGIN_GUID} failed to patch methods.");
            }
        }
    }
}
