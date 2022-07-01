using HarmonyLib;
using flanne;
using System.Collections.Generic;
using UnityEngine;

namespace DebugUtilityMod
{
    [HarmonyPatch]
    class EnemyPatch
    {

        // Weak bosses
        [HarmonyPatch(typeof(BossSpawner), "LoadSpawners")]
        [HarmonyPrefix]
        static void BossLoadSpawners_prefix(ref List<BossSpawn> spawners)
        {
            if (!DUMPlugin.activateMod.Value || !DUMPlugin.hasWeakBossesAndElites.Value) return;

            foreach (BossSpawn bs in spawners)
            {
                Health bossHealth = bs.bossPrefab.GetComponent<Health>();
                bossHealth.maxHP = 100;
            }
        }

        // Weak elites
        [HarmonyPatch(typeof(HordeSpawner), "LoadSpawners")]
        [HarmonyPrefix]
        static void HordeLoadSpawners_prefix(ref List<SpawnSession> spawnSessions)
        {
            if (!DUMPlugin.activateMod.Value || !DUMPlugin.hasWeakBossesAndElites.Value) return;

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
