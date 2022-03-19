using Charly.Data;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Charly.Systems
{
    [UpdateBefore(typeof(IntegrateVelocity))]
    public class ApplyDrag : SystemBase
    {
        private float _averageDT;
        protected override void OnCreate()
        {
            _averageDT = Time.DeltaTime;
        }

        protected override void OnUpdate()
        {
            _averageDT = math.lerp(_averageDT, Time.DeltaTime, 0.05f);
            //this is done so everything can revolve around a normalized range, instead of involving arbitrary values
            float deltaFromAverageDT = Time.DeltaTime / _averageDT;

            Entities.ForEach((ref Velocity2D velocity, in PhysicsProperties physicsProperties) =>
            {
                //todo deal with mag, not componenets seperately
 
                var draggedLinearVelocity = velocity.Linear * physicsProperties.LinearDrag;
                var draggedLinearVelocityTimeAdjusted = math.lerp(velocity.Linear, draggedLinearVelocity, deltaFromAverageDT);
                velocity.Linear = draggedLinearVelocityTimeAdjusted;
                
                var draggedAngularVelocity = velocity.Angular * physicsProperties.AngularDrag;
                var draggedAngularVelocityTimeAdjusted = math.lerp(velocity.Angular, draggedAngularVelocity, deltaFromAverageDT);
                velocity.Angular = draggedAngularVelocityTimeAdjusted;

            }).ScheduleParallel();
        }
    }
}