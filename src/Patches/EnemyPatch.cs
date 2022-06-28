using HarmonyLib;
using flanne;
using System.Collections.Generic;
using UnityEngine;

namespace DebugUtilityMod
{
    [HarmonyPatch]
    class EnemyPatch
    {

        // Weak elites and bosses
        // TODO: Make this work when adjusted in-game mid-match instead of only on init
        [HarmonyPatch(typeof(BossSpawner), "LoadSpawners")]
        [HarmonyPrefix]
        static void BossLoadSpawners_prefix(ref List<BossSpawn> spawners)
        {
            if (!DebugUtilityPlugin.PatchEnabled(DebugUtilityPlugin.hasWeakBossesAndElites)) return;

            foreach (BossSpawn bs in spawners)
            {
                Health bossHealth = bs.bossPrefab.GetComponent<Health>();
                bossHealth.maxHP = 100;
            }
        }

        [HarmonyPatch(typeof(HordeSpawner), "LoadSpawners")]
        [HarmonyPrefix]
        static void HordeLoadSpawners_prefix(ref List<SpawnSession> spawnSessions)
        {
            if (!DebugUtilityPlugin.PatchEnabled(DebugUtilityPlugin.hasWeakBossesAndElites)) return;

            foreach (SpawnSession spawnSession in spawnSessions)
            {
                if (spawnSession.isElite)
                {
                    spawnSession.HP = 100;
                }
            }
        }
    }
}
