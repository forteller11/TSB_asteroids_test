using System;
using Charly.Data;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

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

            var query = GetEntityQuery(ComponentType.ReadOnly<ColliderData>(), ComponentType.ReadOnly<LocalToWorld>(), ComponentType.ReadOnly<Destroyer>());
            var obstacles =  query.ToEntityArrayAsync(Allocator.TempJob, out var entitiesJob);
            var dt = Time.DeltaTime;
            
            Dependency = Entities.ForEach((Entity entity, ref FlockingState flockingState, ref Velocity2D velocity2D, in LocalToWorld ltw, in Destructible destructible) => 
                { 
                    //rotate
                    var rotateBy = float3x3.Euler(new float3(0,0,flockingState.DesiredDirectionChange * dt)); 
                    flockingState.DesiredDirection = math.mul(rotateBy, new float3(flockingState.DesiredDirection, 0)).xy;
                    flockingState.DesiredDirection = math.normalize(flockingState.DesiredDirection);
                    
                    //find closest obstacle that can destroy this one
                float closestDistSq = Single.PositiveInfinity;
                float2 closestPos = new float2(Single.NaN);
                foreach (var obstacle in obstacles)
                {
                    var destroyer = GetComponent<Destroyer>(obstacle);
                    if (!destructible.IsDestroyedBy(destroyer.TypeOfObject))
                    {
                        continue;
                    }
                    
                    var otherPos = GetComponent<LocalToWorld>(obstacle).Position.xy;
                    float distanceSq = math.distancesq(ltw.Position.xy, otherPos);
                    var collider = GetComponent<ColliderData>(obstacle);
                    
                    float approxRadius = collider.GetApproximateRadius();
                    distanceSq -= approxRadius * approxRadius;
                    

                    if (distanceSq < closestDistSq)
                    {
                        closestDistSq = distanceSq;
                        closestPos = otherPos;
                    }
                }

                float closestDist = math.sqrt(closestDistSq);
                float velocityMagnitude = math.length(velocity2D.Linear);
                
                //if close enough to obstacle....
                if (closestDist < flockingState.MiniumDistanceAway)
                {
                    var awayFromClosetPoint = math.normalizesafe(ltw.Position.xy - closestPos);
                    velocity2D.Linear += awayFromClosetPoint * flockingState.AvoidanceAcceleration * dt;

                    //if below max velocity, head away from obstacle always
                    if (velocityMagnitude < flockingState.MaxVelocity)
                    {
                        velocity2D.Linear += awayFromClosetPoint * flockingState.AvoidanceAcceleration * dt;
                    }
                    //if above max velocity, only accelerate away if going towards obstacles, this avoids the flocker speeding up and up as it tries to avoid obstacles
                    else if (math.dot(velocity2D.Linear, awayFromClosetPoint) <= 0)
                    {
                        velocity2D.Linear += awayFromClosetPoint * flockingState.AvoidanceAcceleration * dt;
                    }
                }
                else
                {
                    if (velocityMagnitude < flockingState.MaxVelocity 
                        || math.dot(flockingState.DesiredDirection, velocity2D.Linear.xy) < 0)
                    {
                        velocity2D.Linear += (flockingState.DesiredDirection * flockingState.DefaultAcceleration * dt);
                    }
                }
                })
            .WithNativeDisableParallelForRestriction(obstacles)
            .WithDisposeOnCompletion(obstacles)
            .ScheduleParallel(entitiesJob);

        }
    }
}