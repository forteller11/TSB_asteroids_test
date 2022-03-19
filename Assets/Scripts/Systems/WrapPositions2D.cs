using Charly.Data;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Collider2D = Charly.Data.Collider2D;

namespace Charly.Systems
{
    [UpdateAfter(typeof(IntegrateVelocity))]
    public class WrapPositions2D : SystemBase
    {
        protected override void OnUpdate()
        {
            if (!TryGetSingleton<WorldBounds>(out var worldBounds))
            {
                Debug.LogWarning($"No WorldBounds singleton");
                return;
            }

            //todo [Perf] look into avoiding branching in favour of modulo operator or other workarounds if this ever becomes a perf problem
            Entities.ForEach((Entity entity, ref Translation translation) =>
            {
                float2 halfSize = float2.zero;
                if (HasComponent<Collider2D>(entity))
                {
                    var collider = GetComponent<Collider2D>(entity);
                    if (collider.Type == ColliderType.Circle)
                    {
                        halfSize = collider.Radius;
                    }
                }

                float2 minPos = translation.Value.xy - halfSize;
                float2 maxPos = translation.Value.xy + halfSize;
                
                if (minPos.x > worldBounds.Value.MaxExtents.x)
                    translation.Value.x = worldBounds.Value.MinExtents.x + float.Epsilon;
                else if (maxPos.x < worldBounds.Value.MinExtents.x)
                    translation.Value.x = worldBounds.Value.MaxExtents.x - float.Epsilon;

                if (minPos.y > worldBounds.Value.MaxExtents.y)
                    translation.Value.y = worldBounds.Value.MinExtents.y + float.Epsilon;
                else if (maxPos.y < worldBounds.Value.MinExtents.y)
                    translation.Value.y = worldBounds.Value.MaxExtents.y - float.Epsilon;

            }).ScheduleParallel();
        }
    }
}