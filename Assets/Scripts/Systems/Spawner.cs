using Charly.Authoring;
using Charly.Data;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Charly.Systems
{
    public class Spawner : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _beginSimulationECBSystem;
        protected override void OnCreate()
        {
            _beginSimulationECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var worldBounds = GetSingleton<WorldBounds>();
            var commandBuffer = _beginSimulationECBSystem.CreateCommandBuffer();

            var queryDesc = new EntityQueryDesc()
            {
                All = new ComponentType[] { typeof(Destroyer) },
                Options = EntityQueryOptions.IncludePrefab
            };

            var query = GetEntityQuery(queryDesc);
            var prefabTest = query.GetSingletonEntity();
            
            Entities.ForEach((ref RandomState random, in SpawnerState spawnState) =>
            {
                bool shouldSpawn = spawnState.SpawnDatasReference.Value.SpawnChancePerSecond > random.Value.NextFloat();

                if (!shouldSpawn)
                    return;

                float typeToSpawn = random.Value.NextFloat();
                for (int i = 0; i < spawnState.SpawnDatasReference.Value.Datas.Length; i++)
                {
                    var spawnRate = spawnState.SpawnDatasReference.Value.Datas[i];

                    if (typeToSpawn >= spawnRate.NormalizedMinRate && typeToSpawn <= spawnRate.NormalizedMaxRate)
                    {
                        if (spawnRate.Prefab == Entity.Null)
                            Debug.LogError("enemy prefab is null");
                        
                        
                        // commandBuffer.Instantiate(prefabTest);
                        commandBuffer.Instantiate(spawnRate.Prefab);
                        var entity = commandBuffer.CreateEntity();
                        Debug.Log("Spawn");
                    }
                }
                
            }).WithStructuralChanges().WithoutBurst().Run();
            
            _beginSimulationECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
}