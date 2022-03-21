using System;
using Charly.Data;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Charly.Systems
{
    //Note: this is currently O(n^2) and desperately wants some sort of spatial parition/culling/acceleration data structure
    //this system outputs OverlapEvents
    public class FindOverlaps : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationECBSystem;
        protected override void OnCreate()
        {
            _endSimulationECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        }

        protected override void OnUpdate()
        {
            var query = GetEntityQuery(
                ComponentType.ReadOnly<ColliderData>(), 
                ComponentType.ReadOnly<LocalToWorld>());
            
            var entities = query.ToEntityArrayAsync(Allocator.TempJob, out var entitiesJob);
            
            var collidersFromEntity = GetComponentDataFromEntity<ColliderData>(true);
            var ltwFromEntity = GetComponentDataFromEntity<LocalToWorld>(true);

            var commandBufferConcurrent = _endSimulationECBSystem.CreateCommandBuffer().AsParallelWriter();
            
            //IMPORTANT: in order for this to parallel AND deterministic, no responses can (or writes of any kind) can occur here.
            Dependency = Entities.ForEach((Entity currentEntity, int entityInQueryIndex, in ColliderData collider, in LocalToWorld ltw) =>
                {
                    commandBufferConcurrent.SetBuffer<OverlapEventBuffer>(entityInQueryIndex, currentEntity);
                    
                    foreach (var otherEntity in entities)
                    {
                        if (currentEntity == otherEntity)
                            continue;

                        var collider1 = collider;
                        var collider2 = collidersFromEntity[otherEntity];

                        if (collider1.Type == ColliderType.Inactive || collider2.Type == ColliderType.Inactive)
                            continue;

                        var position1 = ltw.Position.xy;
                        var position2 = ltwFromEntity[otherEntity].Position.xy;

                        if (collider1.Type == ColliderType.Circle && collider2.Type == ColliderType.Circle)
                        {
                            float radiiSum = collider1.Radius + collider2.Radius;
                            float distanceBetween = math.distance(position1, position2);
                            float distanceToSeparation = radiiSum - distanceBetween;

                            if (distanceToSeparation > 0)
                            {
                                float2 toOtherDir = math.normalize(position2 - position1);
                                //todo this approximation could be improved if you take velocity/previous position into account
                                float2 approxContact = position1 + (toOtherDir * distanceBetween / 2);

                                commandBufferConcurrent.AppendToBuffer(
                                    entityInQueryIndex, 
                                    currentEntity,
                                    new OverlapEventBuffer(otherEntity, approxContact, distanceToSeparation));
                            }
                        }
                        else
                        {
                            throw new NotImplementedException(
                                $"Overlap calculation between {collider1.Type} and {collider2.Type} not implemented!");
                        }
                    }
                })
                .WithReadOnly(collidersFromEntity)
                .WithReadOnly(ltwFromEntity)
                .WithDisposeOnCompletion(entities)
                .WithNativeDisableParallelForRestriction(entities)
                .ScheduleParallel(entitiesJob);

            _endSimulationECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
}