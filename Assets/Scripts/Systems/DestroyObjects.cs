using Charly.Data;
using Unity.Entities;

namespace Charly.Systems
{
    public class DestroyObjects : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimECBSystem;
        protected override void OnCreate()
        {
            _endSimECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBufferConcurrent = _endSimECBSystem.CreateCommandBuffer().AsParallelWriter();
            var bullets = GetComponentDataFromEntity<Bullet>(true);
            Entities
                .WithAll<DestructibleTag>()
                .ForEach((Entity currentEntity, int entityInQueryIndex, in DynamicBuffer<OverlapEventBuffer> overlaps) =>
                {
                    foreach (var overlap in overlaps)
                    {
                        if (bullets.HasComponent(overlap.Other))
                        {
                            commandBufferConcurrent.DestroyEntity(entityInQueryIndex, currentEntity);
                            return;
                        }
                    }
                })
                .WithoutBurst()
                .WithReadOnly(bullets)
                .ScheduleParallel();
            
            _endSimECBSystem.AddJobHandleForProducer(Dependency);
        }
        
    }
}