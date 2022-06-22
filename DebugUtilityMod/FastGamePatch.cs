using HarmonyLib;
using flanne;
using System.Collections.Generic;
using UnityEngine;

namespace DebugUtilityMod
{
    static class FastGamePatch
    {
        static private float prev_timer = 0;

        [HarmonyPatch(typeof(GameTimer), "Update")]
        [HarmonyPostfix]
        static void Update_post(ref GameTimer __instance, bool ____isPlaying)
        {
            // Apply a "mult"-fold to GameTimer speed
            if (____isPlaying)
            {
                float delta = __instance.timer - prev_timer;
                Traverse.Create(__instance).Property("timer").SetValue(prev_timer + delta * DebugUtilityPlugin.gametimerMult.Value);
                prev_timer = __instance.timer;
            }
        }

        [HarmonyPatch(typeof(BossSpawner), "LoadSpawners")]
        [HarmonyPrefix]
        static void Start_prefix(ref List<BossSpawn> spawners, ref GameObject ___arenaMonsterPrefab)
        {
            TimeToLive ttl = (TimeToLive)___arenaMonsterPrefab.GetComponent("TimeToLive");
            float lifetime = (float)Traverse.Create(ttl).Field("lifetime").GetValue();
            Traverse.Create(ttl).Field("lifetime").SetValue(lifetime / DebugUtilityPlugin.gametimerMult.Value);
            foreach (BossSpawn bs in spawners)
            {
                bs.timeToSpawn /= DebugUtilityPlugin.gametimerMult.Value;
            }
        }
        [HarmonyPatch(typeof(HordeSpawner), "LoadSpawners")]
        [HarmonyPrefix]
        static void hordeLoadSpawners_prefix(ref List<SpawnSession> spawnSessions)
        {
            foreach (SpawnSession ss in spawnSessions)
            {
                ss.startTime /= DebugUtilityPlugin.gametimerMult.Value;
                ss.duration /= DebugUtilityPlugin.gametimerMult.Value;
                ss.spawnCooldown /= DebugUtilityPlugin.gametimerMult.Value;
            }
        }
    }
}
