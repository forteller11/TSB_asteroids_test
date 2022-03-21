using Charly.Data;
using Unity.Entities;

namespace Systems
{
    public class CountDown : SystemBase
    {
        protected override void OnCreate()
        {
            Entities.ForEach((ref CounterState targetEntity) =>
            {
                targetEntity.CurrentCount = targetEntity.MaxTime;
            }).Run();
        }

        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;
            
            Entities.ForEach((ref CounterState counter) =>
            {
                counter.CurrentCount -= dt;
                if (counter.CurrentCount <= 0)
                {
                    counter.CurrentCount += counter.MaxTime;
                    counter.FinishedThisFrame = true;
                }
                else
                {
                    counter.FinishedThisFrame = false;
                }
            }).ScheduleParallel();
        }
    }
}