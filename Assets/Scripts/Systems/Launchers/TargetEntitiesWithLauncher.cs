using Systems;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Charly.Data
{
    [UpdateAfter(typeof(CountDown))]
    public class TargetEntities : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Launcher launcher, ref Rotation rotation, in LocalToParent lwp, in LocalToWorld ltw, in CounterState counter, in TargetEntity targetEntity) =>
            {
                
                if (!counter.FinishedThisFrame ||
                    targetEntity.Target == Entity.Null ||
                    !HasComponent<LocalToWorld>(targetEntity.Target))
                {
                    launcher.ShouldLaunch = false;
                }
                else
                {
                    launcher.ShouldLaunch = true;

                    var targetLTW = GetComponent<LocalToWorld>(targetEntity.Target);
                    var targetPos = targetLTW.Position.xy;
                    var toTarget = targetPos - ltw.Position.xy;
                    launcher.TargetDirection = math.normalizesafe(toTarget);
                }
            }).ScheduleParallel();
        }
    }
}