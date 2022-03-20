using Unity.Entities;
using Unity.Transforms;

namespace Charly.Data
{
    public class TargetEntities : SystemBase
    {
        protected override void OnCreate()
        {
            Entities.ForEach((ref TargetEntity targetEntity) =>
            {
                targetEntity.TimeUntilNextFire = targetEntity.RateOfFire;
            }).Run();
        }

        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;
            //todo rotate towards target
            Entities.ForEach((ref TargetEntity targetEntity, ref Launcher launcher) =>
            {
                if (targetEntity.Target == Entity.Null)
                {
                    launcher.ShouldLaunch = false;
                    return;
                }

                targetEntity.TimeUntilNextFire -= dt;
                if (targetEntity.TimeUntilNextFire <= 0)
                {
                    targetEntity.TimeUntilNextFire += targetEntity.RateOfFire;
                    //shift dir
                    launcher.ShouldLaunch = true;
                }
                else
                {
                    launcher.ShouldLaunch = false;
                }
            }).ScheduleParallel();
        }
    }
}