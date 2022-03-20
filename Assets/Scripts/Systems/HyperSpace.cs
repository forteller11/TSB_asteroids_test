using Charly.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Charly.Systems
{
    public class HyperSpace : SystemBase
    {
        protected override void OnUpdate()
        {
            var bounds = GetSingleton<WorldBounds>();
            var input = GetSingleton<InputData>();
            Entities.ForEach((ref Translation translation, ref ShipMovement ship, ref RandomState randomState) =>
            {
                if (!input.Secondary.PressedThisTick)
                    return;
                
                float2 randomPosition = randomState.Value.NextFloat2(bounds.Value.MinExtents, bounds.Value.MaxExtents);
                translation.Value = new float3(randomPosition, 0);
            }).ScheduleParallel();
        }
    }
}