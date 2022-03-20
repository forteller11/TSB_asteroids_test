using Charly.Common.Utils;
using Charly.Data;
using Charly.Systems;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [UpdateAfter(typeof(FindOverlapsAndQueueDestruction))]
    [UpdateBefore(typeof(DestroyQueuedDestructibles))]

    public class SpawnObjectsOnDestroy : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimECBSystem;
        protected override void OnCreate()
        {
            _endSimECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBufferConcurrent = _endSimECBSystem.CreateCommandBuffer().AsParallelWriter();
            double time = Time.ElapsedTime;
            Entities.ForEach((Entity currentEntity, int entityInQueryIndex,
                    DynamicBuffer<SpawnOnDestructionData> spawnOnDestructionBuffer, in Destructible destructible,
                    in Translation translation) =>
                {
                    if (!destructible.BeingDestroyed)
                        return;

                    foreach (var toSpawn in spawnOnDestructionBuffer)
                    {
                        var newlySpawned = commandBufferConcurrent.Instantiate(entityInQueryIndex, toSpawn.Entity);
                        commandBufferConcurrent.AddComponent(entityInQueryIndex, newlySpawned, new Translation { Value = translation.Value });

                        //todo [bug] set random direction, use RandomState?
                        var initialVelocity = noise.pnoise(new float2((float)time, (float)time + entityInQueryIndex), new float2(100)) * toSpawn.VelocityMagnitude;
                        commandBufferConcurrent.AddComponent(entityInQueryIndex, newlySpawned, new Velocity2D { Linear = initialVelocity });
                    }
                })
                .ScheduleParallel();
            
            _endSimECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
}