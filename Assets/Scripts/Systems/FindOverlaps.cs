using System;
using Charly.Data;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Collider2D = Charly.Data.Collider2D;

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
                ComponentType.ReadWrite<OverlapEventBuffer>(),
                ComponentType.ReadOnly<Collider2D>(), 
                ComponentType.ReadOnly<Translation>());
            
            var entities = query.ToEntityArrayAsync(Allocator.TempJob, out var entitiesJob);
            var colliders = query.ToComponentDataArrayAsync<Collider2D>(Allocator.TempJob, out var collidersJob);
            var translations = query.ToComponentDataArrayAsync<Translation>(Allocator.TempJob, out var translationsJob);
            // var buffers = query.ToComponentDataArrayAsync<DynamicBuffer<OverlapEventBuffer>>(Allocator.TempJob, out var overlapEventJob);

            EntityCommandBuffer commandBuffer = _endSimulationECBSystem.CreateCommandBuffer();
            
            var overlapDataDependencies = JobHandle.CombineDependencies(entitiesJob, collidersJob, translationsJob);

            //todo make parralel
            // var commandBufferThreadSafe = commandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            
            Dependency = Job.WithCode(() =>
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    var currentEntity = entities[i];
                    // //my way of "clearing" the buffer... not sure if this will cause structural changes or if the ECB is smart enough to optimize this out
                    // commandBuffer.RemoveComponent<OverlapEventBuffer>(entities[i]);
                    // commandBuffer.AddBuffer<OverlapEventBuffer>(entities[i]);
                    commandBuffer.SetBuffer<OverlapEventBuffer>(entities[i]);
                    for (int j = 0; j < entities.Length; j++)
                    {
                        if (i == j)
                            continue;

                        var collider1 = colliders[i];
                        var collider2 = colliders[j];

                        if (collider1.Type == ColliderType.Inactive || collider2.Type == ColliderType.Inactive)
                            continue;
                        
                        var position1 = translations[i].Value.xy;
                        var position2 = translations[j].Value.xy;

                        if (collider1.Type == ColliderType.Circle 
                            && collider2.Type == ColliderType.Circle)
                        {
                            float radiiSum = collider1.Radius + collider2.Radius;
                            float distanceBetween = math.distance(position1, position2);
                            if (distanceBetween - radiiSum < 0)
                            {
                                float2 toOtherDir = math.normalize(position2 - position1);
                                //todo this approximation could be improved if you take velocity/previous position into account
                                float2 approxContact = position1 + (toOtherDir * distanceBetween / 2);
                                
                                // commandBufferThreadSafe.AppendToBuffer(currentEntity.Index, currentEntity, new OverlapEventBuffer(entities[j], approxContact));
                                commandBuffer.AppendToBuffer(currentEntity, new OverlapEventBuffer(entities[j], approxContact));
                                Debug.Log("OVERLAP occured");

                            }
                        }
                        else
                        {
                            throw new NotImplementedException($"Overlap calculation between {collider1.Type} and {collider2.Type} not implemented!");
                        }
                    }
                }

            })
                .WithName("find_and_record_overlaps_job")
                .WithDisposeOnCompletion(entities)
                .WithDisposeOnCompletion(colliders)
                .WithDisposeOnCompletion(translations)
                .WithDisposeOnCompletion(commandBuffer)
                .Schedule(overlapDataDependencies);
           
            _endSimulationECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
}