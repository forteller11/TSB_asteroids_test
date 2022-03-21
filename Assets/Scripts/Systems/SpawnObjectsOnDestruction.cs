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
            
            Entities.ForEach((Entity currentEntity, int entityInQueryIndex, DynamicBuffer<SpawnOnDestructionData> spawnOnDestructionBuffer, in Destructible destructible, in Translation translation) =>
                {
                    if (!destructible.BeingDestroyed)
                        return;
                    
                    var random = new Random((uint) (time + entityInQueryIndex));

                    float angle = random.NextFloat(0, math.PI * 2);
                    float angleIncrement = (math.PI * 2) / spawnOnDestructionBuffer.Length;
                    float angleMaxJitter = angleIncrement / 2;

                    foreach (var toSpawn in spawnOnDestructionBuffer)
                    {
                        var newlySpawned = commandBufferConcurrent.Instantiate(entityInQueryIndex, toSpawn.Entity);
                        commandBufferConcurrent.AddComponent(entityInQueryIndex, newlySpawned, new Translation { Value = translation.Value });

                        float angleWithJitter = angle + random.NextFloat(-angleMaxJitter, angleMaxJitter);
                        angle += angleIncrement;
                        float2 dir = new float2(math.cos(angleWithJitter), math.sin(angleWithJitter));
                        float velocityMagnitude = toSpawn.LinearVelocity.NextRandom(ref random);
                        float2 initialLinearVelocity = dir * velocityMagnitude;

                        float initialAngularVelocity = toSpawn.AngularVelocity.NextRandom(ref random);
                        if (random.NextBool())
                            initialAngularVelocity *= -1;
                        
                        commandBufferConcurrent.AddComponent(entityInQueryIndex, newlySpawned, new Velocity2D
                        {
                            Linear = initialLinearVelocity , 
                            Angular = initialAngularVelocity
                        });
                    }
                })
                .ScheduleParallel();
            
            _endSimECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
}