using Charly.Data;
using Unity.Entities;

namespace Charly.Systems
{
    public class FindOverlapsAndQueueDestruction : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationECBSystem;

        protected override void OnCreate()
        {
            _endSimulationECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = _endSimulationECBSystem.CreateCommandBuffer().AsParallelWriter();
            var destroyers = GetComponentDataFromEntity<Destroyer>(true);
            
            Entities.ForEach((Entity currentEntity, int entityInQueryIndex, DynamicBuffer<OverlapEventBuffer> overlaps, ref Destructible destructible) =>
                {
                    foreach (var overlap in overlaps)
                    {
                        if (destroyers.HasComponent(overlap.Other))
                        {
                            var destroyer = destroyers[overlap.Other];

                            if (HasComponent<ShieldTag>(currentEntity))
                            {
                                commandBuffer.RemoveComponent<ShieldTag>(entityInQueryIndex, currentEntity);
                                return;
                            }
                            
                            if (destructible.IsDestroyedBy(destroyer.TypeOfObject))
                            {
                                destructible.BeingDestroyed = true;
                            }
                        }
                    }
                })
                .WithReadOnly(destroyers)
                .ScheduleParallel();
        }
    }
}