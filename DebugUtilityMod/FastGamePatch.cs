using HarmonyLib;
using flanne;
using System.Collections.Generic;
using UnityEngine;

namespace DebugUtilityMod
{
    static class FastGamePatch
    {
        static private float prev_timer = 0;
        static private char lastGamemode = '\0';

        [HarmonyPatch(typeof(GameTimer), "Update")]
        [HarmonyPostfix]
        static void GameTimerUpdate_post(ref GameTimer __instance, bool ____isPlaying)
        {
            // Apply a X-fold multiplier to GameTimer speed
            if (____isPlaying)
            {
                float delta = __instance.timer - prev_timer;
                Traverse.Create(__instance).Property("timer").SetValue(prev_timer + delta * DebugUtilityPlugin.gametimerMult.Value);
                prev_timer = __instance.timer;
            }
        }

        [HarmonyPatch(typeof(GameTimer), "Awake")]
        [HarmonyPrefix]
        static void GameTimerAwake_prefix()
        {
            // Timer reset
            prev_timer = 0;
        }

        [HarmonyPatch(typeof(BossSpawner), "LoadSpawners")]
        [HarmonyPrefix]
        static void BossLoadSpawners_prefix(ref List<BossSpawn> spawners, ref GameObject ___arenaMonsterPrefab)
        {
            if (!IsDone(true))
            {
                // Reduce arena lifetime accordingly
                TimeToLive ttl = (TimeToLive)___arenaMonsterPrefab.GetComponent("TimeToLive");
                float lifetime = (float)Traverse.Create(ttl).Field("lifetime").GetValue();
                Traverse.Create(ttl).Field("lifetime").SetValue(lifetime / DebugUtilityPlugin.gametimerMult.Value);

                foreach (BossSpawn bs in spawners)
                {
                    // Reduce bosses spawn times accordingly
                    bs.timeToSpawn /= DebugUtilityPlugin.gametimerMult.Value;
                }
            }
        }

        [HarmonyPatch(typeof(HordeSpawner), "LoadSpawners")]
        [HarmonyPrefix]
        static void HordeLoadSpawners_prefix(ref List<SpawnSession> spawnSessions)
        {
            if (!IsDone(false))
            {
                foreach (SpawnSession ss in spawnSessions)
                {
                    // Accelerate horde time params accordingly
                    ss.startTime /= DebugUtilityPlugin.gametimerMult.Value;
                    ss.duration /= DebugUtilityPlugin.gametimerMult.Value;
                    ss.spawnCooldown /= DebugUtilityPlugin.gametimerMult.Value;
                }
            }
        }

        // This is useful trust me, avoid reducing twice spawn timers, 
        static private bool IsDone(bool isBoss)
        {
            bool isStandard = SelectedMap.MapData.nameStringID.key == "standard_mode_name";
            if ((isStandard && lastGamemode == 's') || (!isStandard && lastGamemode == 'q'))
            {
                return true;
            }
            if (!isBoss)
            {
                lastGamemode = isStandard ? 's' : 'q';
            }
            return false;
        }


        [HarmonyPatch(typeof(SummonEgg), "Start")]
        [HarmonyPrefix]
        static void SummonnEggStart_prefix(ref float ___secondsToHatch)
        {
            // Accelerate hatch time accordingly
            ___secondsToHatch = ___secondsToHatch / DebugUtilityPlugin.gametimerMult.Value;
        }
    }
}
