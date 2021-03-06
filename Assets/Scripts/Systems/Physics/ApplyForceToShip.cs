using Charly.Data;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Charly.Systems
{
    [UpdateAfter(typeof(RefreshInput))]
    public class ApplyForceToShip : SystemBase
    {
        protected override void OnUpdate()
        {
            if (!TryGetSingleton<InputData>(out var controls))
            {
                Debug.LogWarning($"Could not find controls to apply force to ship");
                return;
            }
            
            float dt = Time.DeltaTime;
            Entities.ForEach((ref Velocity2D velocity, in ShipMovement shipMovement, in Rotation rotation, in LocalToWorld ltw) =>
            {
                var forwardDirection3D = ltw.Up;
                if (forwardDirection3D.z > 0.001)
                    Debug.LogWarning($"Forward Direction of ship does not lie flat on the x/y plane, which will lead to bugs.");

                var forwardDirection2D = forwardDirection3D.xy;
                var linearAcceleration = forwardDirection2D * (shipMovement.LinearSensitivity * controls.Movement * dt);
                velocity.Linear += linearAcceleration;
                
                var angularAcceleration = shipMovement.AngularSensitivity * controls.Turn * dt;
                velocity.Angular += angularAcceleration;
            }).Schedule();
        }
    }
}