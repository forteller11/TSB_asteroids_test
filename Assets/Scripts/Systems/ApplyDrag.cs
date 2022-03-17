using Charly.Data;
using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Systems
{
    [UpdateBefore(typeof(IntegrateVelocity))]
    public class ApplyDrag : SystemBase
    {
        protected override void OnUpdate()
        {
            //todo improve authoring:
            //this is asymptotic drag... but is no longer really normalized because of incorporation of deltaTime for frame rate independence
            //the authoring is now fuzzy and risks accidentally creating an inverse drag if the DeltaTime is extreme?
            
            //todo add mass to calculations
            //use componentfromentity so that mass can be optional and defaults to 1
            
            float dt = Time.DeltaTime;
            Entities.ForEach((ref Velocity2D velocity, in PhysicsProperties physicsProperties) =>
            {
                float commonRate = dt * physicsProperties.Mass;
                
                var rateOfDragLinear = math.clamp(commonRate * physicsProperties.LinearDrag, float2.zero, new float2(1));
                velocity.Linear = math.lerp(velocity.Linear, float2.zero, rateOfDragLinear);

                float rateOfDragAngular = math.clamp(commonRate * physicsProperties.AngularDrag, 0, 1);
                velocity.Angular = math.lerp(velocity.Angular, 0, rateOfDragAngular);
            }).ScheduleParallel();
        }
    }
}