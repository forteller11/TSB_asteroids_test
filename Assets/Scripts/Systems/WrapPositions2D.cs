using Charly.Data;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Charly.Systems
{
    public class WrapPositions2D : SystemBase
    {
        //make WorldBounds part of a singleton post-conversion process for convenience sake
        protected override void OnCreate()
        {
            var entityQuery = EntityManager.CreateEntityQuery(ComponentType.ReadOnly<WorldBounds>());
            using var bounds = entityQuery.ToComponentDataArray<WorldBounds>(Allocator.Temp);
            using var entities = entityQuery.ToEntityArray(Allocator.Temp);
            
            if (bounds.Length > 1)
                Debug.LogWarning($"This system expects one {nameof(WorldBounds)} in the world, check for multiple authoring components");
            
            if (bounds.Length > 0)
                SetSingleton(bounds[0]);

            for (int i = 0; i < bounds.Length; i++)
            {
                EntityManager.RemoveComponent<WorldBounds>(entities[i]);
            }
        }
        
        protected override void OnUpdate()
        {
            if (!TryGetSingleton<WorldBounds>(out var worldBounds))
                return;
            
            //todo look into avoiding branching in favour of modulo operator or other workarounds if this ever becomes a perf problem
            Entities.ForEach((ref Translation translation) =>
            {
                //this assumes that positions will never be more than worldBounds.Value.Size out of the bounds
                if (translation.Value.x > worldBounds.Value.MaxExtents.x)
                    translation.Value.x = worldBounds.Value.MinExtents.x + float.Epsilon;
                else if (translation.Value.x < worldBounds.Value.MinExtents.x)
                    translation.Value.x = worldBounds.Value.MaxExtents.x - float.Epsilon;

                if (translation.Value.y > worldBounds.Value.MaxExtents.y)
                    translation.Value.y -= worldBounds.Value.Size.y;
                else if (translation.Value.y < worldBounds.Value.MinExtents.y)
                    translation.Value.y += worldBounds.Value.Size.y;

            }).ScheduleParallel();
        }
    }
}