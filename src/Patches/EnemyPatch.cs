using HarmonyLib;
using flanne;
using System.Collections.Generic;
using UnityEngine;

namespace DebugUtilityMod
{
    class EnemyPatch
    {

        // Weak elites and bosses
        [HarmonyPatch(typeof(BossSpawner), "LoadSpawners")]
        [HarmonyPrefix]
        static void BossLoadSpawners_prefix(ref List<BossSpawn> spawners)
        {
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
