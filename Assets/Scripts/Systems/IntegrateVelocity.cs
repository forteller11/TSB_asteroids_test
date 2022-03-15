using Charly.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Charly.Systems
{
    public class IntegrateVelocity : SystemBase
    {
        protected override void OnUpdate()
        {
            float dt = Time.fixedDeltaTime;
            Entities.ForEach((ref Velocity2D velocity, ref Translation translation, ref Rotation rotation) =>
            {
                float2 forceToAdd = velocity.LinearVelocity * dt;
                translation.Value += new float3(forceToAdd, 0);

                var angularAcceleration = quaternion.Euler(0, 0, velocity.AngularVelocity * dt);
                rotation.Value = math.mul(rotation.Value , angularAcceleration);
            }).ScheduleParallel();
        }
    }
}