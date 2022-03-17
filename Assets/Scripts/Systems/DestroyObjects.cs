using Charly.Common.Utils;
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
            var bullets = GetComponentDataFromEntity<Destroyer>(true);
            Entities
                .ForEach((Entity currentEntity, int entityInQueryIndex, in DynamicBuffer<OverlapEventBuffer> overlaps, in Destructible destructible) =>
                {
                    foreach (var overlap in overlaps)
                    {
                        if (bullets.HasComponent(overlap.Other))
                        {
                            var bullet = bullets[overlap.Other];
                            if (MaskUtils.HasAllFlags((int)bullet.Mask,(int)destructible.DestroyedBy))
                            {
                                commandBufferConcurrent.DestroyEntity(entityInQueryIndex, currentEntity);
                                return;
                            }
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