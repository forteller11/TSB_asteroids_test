using Charly.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Charly.Systems
{
    //so that positions are updated
    [UpdateBefore(typeof(TransformSystemGroup))]
    public class CreateProjectilesFromLaunchers : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationECB;
        
        protected override void OnCreate()
        {
            _endSimulationECB = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = _endSimulationECB.CreateCommandBuffer().AsParallelWriter();
            
            Entities.ForEach((Entity entity, int entityInQueryIndex, in Launcher launcher, in LocalToWorld ltw) =>
            {
                if (!launcher.ShouldLaunch)
                    return;
                
                var newProjectile = commandBuffer.Instantiate(entityInQueryIndex, launcher.ProjectilePrefab);

                commandBuffer.SetComponent(entityInQueryIndex, newProjectile, new Translation {Value = ltw.Position});

                var matrix = float4x4.TRS(ltw.Position, quaternion.identity, 0);
                commandBuffer.SetComponent(entityInQueryIndex, newProjectile, new LocalToWorld(){Value = matrix});

                float2 initialVelocity = new float2(0);
                if (HasComponent<Velocity2D>(entity))
                {
                    initialVelocity = GetComponent<Velocity2D>(entity).Linear;
                }
                initialVelocity += launcher.TargetDirection * launcher.InitialVelocityMagnitude;
                commandBuffer.SetComponent(entityInQueryIndex, newProjectile, new Velocity2D(initialVelocity, 0));
            }).ScheduleParallel();
            
            _endSimulationECB.AddJobHandleForProducer(Dependency);
        }
    }
}