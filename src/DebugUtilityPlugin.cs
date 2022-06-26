using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;

namespace DebugUtilityMod
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class DebugUtilityPlugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> activateMod;


        public static ConfigEntry<float> gametimerMult;
        public static ConfigEntry<float> XPmult;
        public static ConfigEntry<int> maxPlayerLevel;
        public static ConfigEntry<bool> hasXPPatch;
        public static ConfigEntry<bool> hasInvincibility;
        public static ConfigEntry<bool> hasFastGame;
        public static ConfigEntry<bool> hasInfiniteReroll;
        public static ConfigEntry<bool> hasGunPatch;
        public static ConfigEntry<bool> hasWeakBossesAndElites;


        //public static ConfigEntry<bool> hasUnlocks;
        //public static ConfigEntry<bool> hasSoulUnlock;



        public void Awake()
        {
            activateMod = Config.Bind("_General", "Activation", true, "If false, the mod does not load");
            hasXPPatch = Config.Bind("XP Patch", "XP Patch activation", false, "Set to True to activate XP Patch");
            XPmult = Config.Bind("XP Patch", "XP multiplier", 1f, "Amount of multiplication bonus applied to XP pickup, 1 is baseXP");
            maxPlayerLevel = Config.Bind("XP Patch", "Max level reachable", 100, "Level after which the player stop receiving XP");

            hasFastGame = Config.Bind("Fast GameTimer", "FastGame activation", false, "Set to True to activate faster game");
            gametimerMult = Config.Bind("Fast GameTimer", "GameTimer speed", 2f, "Increase the speed of the game; x is baseTime/x, 1 is baseTime");

            hasInvincibility = Config.Bind("Invincibility", "Player Invincibility", false, "If active, the player cannot take damage");

            hasInfiniteReroll = Config.Bind("Reroll", "Infinite Reroll", false, "If active, every character can reroll indefinitly");

            hasGunPatch = Config.Bind("Gun", "Infinite Ammo", false, "If active, infinite ammo");
            hasWeakBossesAndElites = Config.Bind("Enemy", "Weak Bosses and Elite", false, "If active, Bosses and Elite have 100 HP");

            if (!activateMod.Value)
            {
                Logger.LogInfo("<Inactive>");
                return;
            }

            try
            {
                if (hasInvincibility.Value)
                {
                    Harmony.CreateAndPatchAll(typeof(InvincibilityPatch));
                }
                Logger.LogInfo((hasInvincibility.Value ? "<Active>" : "<Inactive>") + " Invincibility");
            }
            catch
            {
                Logger.LogError($"{PluginInfo.PLUGIN_GUID} failed to patch methods (InvincibilityPatch).");
            }

            try
            {
                if (hasGunPatch.Value)
                {
                    Harmony.CreateAndPatchAll(typeof(GunPatch));
                }
                Logger.LogInfo((hasGunPatch.Value ? "<Active>" : "<Inactive>") + " GunPatch");
            }
            catch
            {
                Logger.LogError($"{PluginInfo.PLUGIN_GUID} failed to patch methods (GunPatch).");
            }
            try
            {
                if (hasInfiniteReroll.Value)
                {
                    Harmony.CreateAndPatchAll(typeof(RerollPatch));
                }
                Logger.LogInfo((hasInfiniteReroll.Value ? "<Active>" : "<Inactive>") + " Infinite Reroll");
            }
            catch
            {
                Logger.LogError($"{PluginInfo.PLUGIN_GUID} failed to patch methods (RerollPatch).");
            }
            try
            {
                if (hasXPPatch.Value)
                {
                    Harmony.CreateAndPatchAll(typeof(XPPatch));
                }
                Logger.LogInfo(hasXPPatch.Value ? "<Active> XPPatch     XP = " + XPmult.Value+ "*baseXP  MaxLevel = " + maxPlayerLevel.Value : "<Inactive> FastXP");
            }
            catch
            {
                Logger.LogError($"{PluginInfo.PLUGIN_GUID} failed to patch methods (XPPatch).");
            }

            try
            {
                if (hasFastGame.Value && gametimerMult.Value != 0)
                {
                    Harmony.CreateAndPatchAll(typeof(FastGamePatch));
                }
                Logger.LogInfo(hasFastGame.Value && gametimerMult.Value != 0 ? "<Active> FastGame    duration = baseTime/" + gametimerMult.Value : "<Inactive> FastGame");
            }
            catch
            {
                Logger.LogError($"{PluginInfo.PLUGIN_GUID} failed to patch methods (FastGamePatch).");
            }

            try
            {
                if (hasWeakBossesAndElites.Value)
                {
                    Harmony.CreateAndPatchAll(typeof(EnemyPatch));
                }
                Logger.LogInfo((hasWeakBossesAndElites.Value ? "<Active>" : "<Inactive>") + " Weak Bosses & Elites");
            }
            catch
            {
                Logger.LogError($"{PluginInfo.PLUGIN_GUID} failed to patch methods (EnemyPatch).");
            }

            try
            {
                Harmony.CreateAndPatchAll(typeof(NoUnlockPatch));
                Logger.LogInfo("<Active> NoUnlocks & NoSoulGain");
            }
            catch
            {
                Logger.LogError($"{PluginInfo.PLUGIN_GUID} failed to patch methods (NoUnlockPatch).");
            }
        }
    }
}
