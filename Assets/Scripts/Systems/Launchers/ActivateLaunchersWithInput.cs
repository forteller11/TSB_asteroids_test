using Charly.Data;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Charly.Systems
{
    public class ActivateLaunchersWithInput : SystemBase
    {
        protected override void OnUpdate()
        {
            if (!TryGetSingleton<InputData>(out var input))
            {
                Debug.LogError("No input singleton");
                return;
            }

            Entities.WithAll<InputDrivenTag>().ForEach((ref Launcher launchers, in LocalToWorld ltw) =>
            {
                launchers.ShouldLaunch = input.Primary.PressedThisTick;
                launchers.TargetDirection = ltw.Up.xy;
            }).ScheduleParallel();
        }
    }
}