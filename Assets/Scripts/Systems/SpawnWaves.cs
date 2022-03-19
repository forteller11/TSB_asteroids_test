using Charly.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Charly.Systems
{
    public class SpawnWaves : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationECBSystem;

        protected override void OnCreate()
        {
            _endSimulationECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            if (!TryGetSingleton<WorldBounds>(out var worldBounds))
            {
                Debug.LogError("Could not get singleton");
                return;
            }
            
            var commandBuffer = _endSimulationECBSystem.CreateCommandBuffer();
            var dt = World.Time.DeltaTime;

            Entities.ForEach((Entity entity, ref RandomState random, ref WaveSpawnerState spawnState) =>
                {
                    if (spawnState.WaveIndex >= spawnState.Waves.Length)
                    {
                        Debug.Log("out of waves");
                        return;
                    }
                    
                    var currentWave = spawnState.Waves[spawnState.WaveIndex];

                    float spawnChanceNormalized = currentWave.SpawnChanceNormalizedPerSecond * dt;
                    bool shouldSpawn = spawnChanceNormalized > random.Value.NextFloat()
                                       && currentWave.EnemiesInPlay < currentWave.MaxEnemiesInPlay;

                    if (!shouldSpawn)
                        return;

                    //switch waves if you spawned all of them and they're all destroyed
                    if (currentWave.EnemyIndex >= currentWave.Enemies.Length &&
                        currentWave.EnemiesInPlay == 0)
                    {
                        if (spawnState.WaveIndex >= spawnState.Waves.Length)
                        {
                            spawnState.Waves[spawnState.WaveIndex] = currentWave;
                            commandBuffer.RemoveComponent<WaveSpawnerState>(entity);
                            Debug.Log($"No Waves Left, after wave {spawnState.WaveIndex}");
                            return;
                        }
                        else
                        {
                            spawnState.WaveIndex++;
                            currentWave = spawnState.Waves[spawnState.WaveIndex];
                            Debug.Log($"Switching to wave: {spawnState.WaveIndex}");
                        }
                    }
                    
                    if (currentWave.EnemyIndex >= currentWave.Enemies.Length)
                        return;

                    //spawn enemy
                    var enemyToSpawn = currentWave.Enemies[currentWave.EnemyIndex];
                    
                    //todo [Bug] this causes errors, usually singleton related??
                    var newEnemy = commandBuffer.Instantiate(enemyToSpawn.Prefab);
                    
                    var randomPosition = random.Value.NextFloat2(worldBounds.Value.MinExtents, worldBounds.Value.MaxExtents);
                    commandBuffer.AddComponent(newEnemy, new Translation() { Value = new float3(randomPosition, 0) });
                    
                    commandBuffer.AddComponent(newEnemy, new Spawned(entity, spawnState.WaveIndex));

                    currentWave.EnemiesInPlay++;
                    currentWave.EnemyIndex++;
                    
                    spawnState.Waves[spawnState.WaveIndex] = currentWave;
                    Debug.Log($"Spawn {enemyToSpawn.Prefab}");
                })
                .Schedule();
            

            _endSimulationECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
}