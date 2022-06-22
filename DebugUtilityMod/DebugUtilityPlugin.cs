using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;

namespace DebugUtilityMod
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class DebugUtilityPlugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "kaios.mod.debugutility";
        public const string PLUGIN_NAME = "Debug Utility Mod";
        public const string PLUGIN_VERSION = "0.1";


        public static ConfigEntry<float> gametimerMult;
        public static ConfigEntry<float> XPmult;
        public static ConfigEntry<int> stopXPAtLevel;
        public static ConfigEntry<bool> hasXPPatch;
        public static ConfigEntry<bool> hasInvincibility;
        public static ConfigEntry<bool> hasFastGame;

        //public static ConfigEntry<bool> hasUnlocks;
        //public static ConfigEntry<bool> hasSoulUnlock;



        public void Awake()
        {
            hasXPPatch = Config.Bind("XP Patch", "XP Patch activation", false, "Set to True to activate XP Patch");
            XPmult = Config.Bind("XP Patch", "FastXP multiplier", 1f, "Amount of multiplication bonus applied to XP pickup");
            stopXPAtLevel = Config.Bind("XP Patch", "Stop leveling up after X level", 100, "Level after which the player stop receiving XP");

            hasFastGame = Config.Bind("Fast GameTimer", "FastGame activation", false, "Set to True to activate faster game");
            gametimerMult = Config.Bind("Fast GameTimer", "GameTimer multiplier", 2f, "Increase the speed of the game (if 2, standard lasts 20/2 = 10 min)");

            hasInvincibility = Config.Bind("Invincibility", "Player Invincibility", false, "If active, the player cannot take damage");


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
                Logger.LogError($"{PLUGIN_GUID} failed to patch methods (InvincibilityPatch).");
            }

            try
            {
                if (hasXPPatch.Value)
                {
                    Harmony.CreateAndPatchAll(typeof(XPPatch));
                }
                Logger.LogInfo(hasXPPatch.Value ? "<Active> XPPatch     XP = " + XPmult.Value+ "*baseXP  StopLevel = " + stopXPAtLevel.Value : "<Inactive> FastXP");
            }
            catch
            {
                Logger.LogError($"{PLUGIN_GUID} failed to patch methods (FastXPPatch).");
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
                Logger.LogError($"{PLUGIN_GUID} failed to patch methods (FastGamePatch).");
            }

            try
            {
                Harmony.CreateAndPatchAll(typeof(NoUnlockPatch));
                Logger.LogInfo("<Active> NoUnlocks & NoSoulGain");
            }
            catch
            {
                Logger.LogError($"{PLUGIN_GUID} failed to patch methods (NoUnlockPatch).");
            }
        }
    }
}
