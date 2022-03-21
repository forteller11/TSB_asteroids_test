// using Charly.Data;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Jobs;
// using Unity.Transforms;
// using UnityEngine;
//
// namespace Charly.Systems
// {
//     [UpdateAfter(typeof(SpawnWaves))]
//     public class CleanUpSpawned : SystemBase
//     {
//         private EndSimulationEntityCommandBufferSystem _beginSimulationECBSystem;
//         private EntityQueryDesc _dyingSpawnsQueryDesc;
//
//         protected override void OnCreate()
//         {
//             _beginSimulationECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
//             
//             _dyingSpawnsQueryDesc = new EntityQueryDesc()
//             {
//                 All = new []{ComponentType.ReadOnly<Spawned>(), },
//                 None = new [] {ComponentType.ReadOnly<Translation>(), }
//             };
//         }
//
//         protected override void OnUpdate()
//         {
//             var commandBuffer = _beginSimulationECBSystem.CreateCommandBuffer();
//
//             //todo [Refactor] think of how to rename the variables below, everything becomes word-salad
//             var dyingSpawnedQuery = GetEntityQuery(_dyingSpawnsQueryDesc);
//             var dyingSpawnedEntities = dyingSpawnedQuery.ToEntityArrayAsync(Allocator.TempJob, out var dyingSpawnsEntitiesJob);
//             
//             var dyingSpawnDataFromEntity = GetComponentDataFromEntity<Spawned>(true);
//             
//             Dependency = Entities.ForEach((ref WaveSpawnerState spawnerState) =>
//             {
//                 foreach (var dyingSpawnEntity in dyingSpawnedEntities)
//                 {
//                     var dyingSpawned = dyingSpawnDataFromEntity[dyingSpawnEntity];
//                     
//                     var parentWave = spawnerState.Waves[dyingSpawned.WaveIndex];
//                     parentWave.EnemiesInPlay--;
//                     spawnerState.Waves[dyingSpawned.WaveIndex] = parentWave;
//
//                     commandBuffer.RemoveComponent<Spawned>(dyingSpawnEntity); //remove system state component so entity is actually destroyed
//                     commandBuffer.DestroyEntity(dyingSpawnEntity); //shouldn't be necessary... just in case a new component is added between the initial destroy and cleanup systems
//                     
//                     Debug.Log($"Clean up enemy in play {dyingSpawnEntity}, enemies in play: {parentWave.EnemiesInPlay}");
//                 }
//             })
//                 .WithDisposeOnCompletion(dyingSpawnedEntities)
//                 .WithReadOnly(dyingSpawnDataFromEntity)
//                 .Schedule(JobHandle.CombineDependencies(Dependency, dyingSpawnsEntitiesJob));
//
//             _beginSimulationECBSystem.AddJobHandleForProducer(Dependency);
//         }
//     }
// }