using Charly.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Charly.Systems
{
    public class HyperSpace : SystemBase
    {
        protected override void OnUpdate()
        {
            if (!TryGetSingleton<WorldBounds>(out var worldBounds))
            {
                Debug.LogWarning($"No {nameof(WorldBounds)} singleton");
                return;
            }

            if (!TryGetSingleton<InputData>(out var input))
            {
                Debug.LogWarning($"No {nameof(InputData)} singleton");
                return;
            }

            Entities.ForEach((ref Translation translation, ref ShipMovement ship, ref RandomState randomState) =>
            {
                if (!input.Secondary.PressedThisTick)
                    return;
                
                float2 randomPosition = randomState.Value.NextFloat2(worldBounds.Value.MinExtents, worldBounds.Value.MaxExtents);
                translation.Value = new float3(randomPosition, 0);
            }).ScheduleParallel();
        }
    }
}