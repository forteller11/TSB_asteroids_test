using Charly.Authoring;
using Charly.Data;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Charly.Systems
{
    public class SpawnWaves : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _beginSimulationECBSystem;

        protected override void OnCreate()
        {
            _beginSimulationECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

            // Entities.ForEach((ref SpawnerState spawnState) =>
            // {
            //     foreach (var wave in spawnState.Waves)
            //     {
            //         float sumRate = 0;
            //         foreach (var enemy in wave.Enemies)
            //         {
            //             sumRate += enemy.ra
            //         }
            //     }
            // })
        }

        protected override void OnUpdate()
        {

            var worldBounds = GetSingleton<WorldBounds>();
            var commandBuffer = _beginSimulationECBSystem.CreateCommandBuffer();
            var dt = World.Time.DeltaTime;

            Entities.ForEach((ref RandomState random, in WaveSpawner spawnState) =>
            {
                var currentWave = spawnState.Waves[spawnState.WaveIndex];

                float spawnChanceNormalized = currentWave.SpawnChanceNormalizedPerSecond * dt;
                bool shouldSpawn = spawnChanceNormalized > random.Value.NextFloat()
                                   && currentWave.CurrentEnemiesInPlay < currentWave.MaxEnemiesInPlay;

                if (!shouldSpawn)
                    return;

                for (var i = 0; i < currentWave.Enemies.Length; i++)
                {
                    var enemySpawnData = currentWave.Enemies[i];
                    if (enemySpawnData.NumberToSpawn > 0)
                    {
                        Debug.Log("Spawn");
                        commandBuffer.Instantiate(enemySpawnData.Prefab);
                        return;
                    }

                }
                //todo spawn anywhere in world.......

            }).WithStructuralChanges().WithoutBurst().Run();

            _beginSimulationECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
}