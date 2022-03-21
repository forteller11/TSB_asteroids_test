using Charly.Data;
using Unity.Entities;

namespace Charly.Systems
{
    public class ApplyLifeSpans : SystemBase
    {
        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;
            Entities.ForEach((Entity entity, int entityInQueryIndex, ref LifeSpan lifeSpan, ref Destructible destructible) =>
            {
                lifeSpan.SecondsCurrent -= dt;

                if (lifeSpan.SecondsCurrent < 0)
                {
                    destructible.BeingDestroyed = true;
                }
                
            }).ScheduleParallel();
        }
    }
}