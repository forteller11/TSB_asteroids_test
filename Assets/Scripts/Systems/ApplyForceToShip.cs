using Charly.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Charly.Systems
{
    public class ApplyForceToShip : SystemBase
    {
        protected override void OnUpdate()
        {
            if (!TryGetSingleton<ControlsData>(out var controls))
            {
                Debug.LogWarning($"Could not find controls to apply force to ship");
                return;
            }
            
            float dt = Time.DeltaTime;
            Entities.ForEach((ref Velocity2D velocity, in ShipMovement shipMovement, in Rotation rotation) =>
            {
                
                var forwardDirection3D = math.mul(rotation.Value, new float3(0,1, 0));
                if (forwardDirection3D.z > 0.001)
                    Debug.LogWarning($"Forward Direction of ship does not lie flat on the x/y plane, which will lead to bugs.");

                var forwardDirection2D = forwardDirection3D.xy;
                var linearAcceleration = forwardDirection2D * (shipMovement.LinearSensitivity * controls.Movement * dt);
                velocity.Linear += linearAcceleration;
                
                var angularAcceleration = shipMovement.AngularSensitivity * controls.Turn * dt;
                velocity.Angular += angularAcceleration;
                
            }).ScheduleParallel();
        }
    }
}