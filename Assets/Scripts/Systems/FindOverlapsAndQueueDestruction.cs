using Charly.Data;
using Unity.Entities;

namespace Charly.Systems
{
    public class FindOverlapsAndQueueDestruction : SystemBase
    {
        protected override void OnUpdate()
        {
            var destroyers = GetComponentDataFromEntity<Destroyer>(true);
            Entities.ForEach((Entity currentEntity, int entityInQueryIndex, DynamicBuffer<OverlapEventBuffer> overlaps, ref Destructible destructible) =>
                {
                    foreach (var overlap in overlaps)
                    {
                        if (destroyers.HasComponent(overlap.Other))
                        {
                            var destroyer = destroyers[overlap.Other];
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