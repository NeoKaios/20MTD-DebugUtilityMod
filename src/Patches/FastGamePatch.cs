using HarmonyLib;
using flanne;
using System.Collections.Generic;
using UnityEngine;

namespace DebugUtilityMod
{
    [HarmonyPatch]
    static class FastGamePatch
    {
        static private float prev_timer = 0;
        static private string lastGamemode = "";
        static Traverse timer = null;

        [HarmonyPatch(typeof(GameTimer), "Start")]
        [HarmonyPostfix]
        static void GameTimerStart_post(ref GameTimer __instance)
        {
            if (!DUMPlugin.PatchEnabled(DUMPlugin.hasFastGame)) return;

            // Backup Traverse to increase performance on update
            timer = Traverse.Create(__instance).Property("timer");
            prev_timer = 0;
        }

        [HarmonyPatch(typeof(GameTimer), "Update")]
        [HarmonyPostfix]
        static void GameTimerUpdate_post(ref GameTimer __instance, bool ____isPlaying)
        {
            // Apply a X-fold multiplier to GameTimer speed
            if (____isPlaying && timer != null)
            {
                float delta = __instance.timer - prev_timer;
                //Traverse.Create(__instance).Property("timer").SetValue(prev_timer + delta * DebugUtilityPlugin.gametimerMult.Value);
                timer.SetValue(prev_timer + delta * DUMPlugin.gametimerMult.Value);

                prev_timer = __instance.timer;
            }
        }

        [HarmonyPatch(typeof(BossSpawner), "LoadSpawners")]
        [HarmonyPrefix]
        static void BossLoadSpawners_prefix(ref List<BossSpawn> spawners, ref GameObject ___arenaMonsterPrefab)
        {
            if (!DUMPlugin.PatchEnabled(DUMPlugin.hasFastGame)) return;

            if (IsDone(true)) return;

            // Reduce arena lifetime accordingly
            TimeToLive ttl = (TimeToLive)___arenaMonsterPrefab.GetComponent("TimeToLive");
            float lifetime = (float)Traverse.Create(ttl).Field("lifetime").GetValue();
            Traverse.Create(ttl).Field("lifetime").SetValue(lifetime / DUMPlugin.gametimerMult.Value);

            foreach (BossSpawn bs in spawners)
            {
                // Reduce bosses spawn times accordingly
                bs.timeToSpawn /= DUMPlugin.gametimerMult.Value;
            }
        }

        [HarmonyPatch(typeof(HordeSpawner), "LoadSpawners")]
        [HarmonyPrefix]
        static void HordeLoadSpawners_prefix(ref List<SpawnSession> spawnSessions)
        {
            if (!DUMPlugin.PatchEnabled(DUMPlugin.hasFastGame)) return;

            if (IsDone(false)) return;

            foreach (SpawnSession ss in spawnSessions)
            {
                // Accelerate horde time params accordingly
                ss.startTime /= DUMPlugin.gametimerMult.Value;
                ss.duration /= DUMPlugin.gametimerMult.Value;
                ss.spawnCooldown /= DUMPlugin.gametimerMult.Value;
            }
        }


        private static List<string> doneGameMode = new List<string>();
        // This is useful trust me, avoid reducing twice spawn timers,
        static private bool IsDone(bool isBoss)
        {
            if (doneGameMode.Contains(SelectedMap.MapData.nameStringID.key)) //Same gamemode
            {
                return true;
            }
            if (isBoss) // beacause bossLoad execute after
            {
                doneGameMode.Add(SelectedMap.MapData.nameStringID.key);
                lastGamemode = SelectedMap.MapData.nameStringID.key;
            }
            return false;
        }

        [HarmonyPatch(typeof(SummonEgg), "Start")]
        [HarmonyPrefix]
        static void SummonnEggStart_prefix(ref float ___secondsToHatch)
        {
            if (!DUMPlugin.PatchEnabled(DUMPlugin.hasFastGame)) return;

            // Accelerate hatch time accordingly
            ___secondsToHatch = ___secondsToHatch / DUMPlugin.gametimerMult.Value;
        }
    }
}
