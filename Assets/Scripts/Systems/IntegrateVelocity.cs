using Charly.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Charly.Systems
{
    [UpdateAfter(typeof(ApplyForceToShip))]
    public class IntegrateVelocity : SystemBase
    {
        protected override void OnUpdate()
        {
            float dt = Time.fixedDeltaTime;
            Entities.ForEach((ref Velocity2D velocity, ref Translation translation, ref Rotation rotation) =>
            {
                float2 forceToAdd = velocity.Linear * dt;
                translation.Value += new float3(forceToAdd, 0);

                var angularAcceleration = quaternion.Euler(0, 0, velocity.Angular * dt);
                rotation.Value = math.mul(rotation.Value , angularAcceleration);
            }).ScheduleParallel();
        }
    }
}