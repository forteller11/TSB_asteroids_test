using System;
using Charly.Common.Utils;
using Charly.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

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
                            var bullet = destroyers[overlap.Other];
                            if (MaskUtils.HasAllFlags((int)bullet.Mask,(int)destructible.DestroyedBy))
                            {
                                destructible.BeingDestroyed = true;
                                return;
                            }
                        }
                    }
                })
                .WithoutBurst()
                .WithReadOnly(destroyers)
                .ScheduleParallel();
        }
    }
}