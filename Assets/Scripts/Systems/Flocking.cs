using Charly.Data;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Collider2D = UnityEngine.Collider2D;

namespace Systems
{
    public class Flocking : SystemBase
    {
        protected override void OnCreate()
        {
         
        }
        protected override void OnUpdate()
        {
            //todo get all overlaps.....
            //todo avoid closest overlaps...
            //todo otherwise.... go in desired direction

            var query = GetEntityQuery(ComponentType.ReadOnly<Collider2D>(), ComponentType.ReadOnly<LocalToWorld>());
            var entities =  query.ToEntityArrayAsync(Allocator.TempJob, out var entitiesJob);
            var elaspedTime = Time.ElapsedTime;
            
            Dependency = Entities.ForEach((Entity entity, ref FlockingState flockingState, ref Velocity2D velocity2D) =>
            {
                float velMag = math.distance(float2.zero, velocity2D.Linear);
                if (velMag >= flockingState.MaxVelocity)
                {
                    return;

                }
                
                velocity2D.Linear += (flockingState.DesiredDirection * flockingState.MaxAcceleration);
                int compilerDontThrowErrorsCaptureThisVariable = entities.Length;
                //todo find closest asteroid and dodge.
            })
                .WithDisposeOnCompletion(entities)
                .ScheduleParallel(entitiesJob);

        }
    }
}