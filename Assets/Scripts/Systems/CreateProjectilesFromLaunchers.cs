using Charly.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Charly.Systems
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    public class CreateProjectilesFromLaunchers : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimECBSystem;
        
        protected override void OnCreate()
        {
            _endSimECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = _endSimECBSystem.CreateCommandBuffer().AsParallelWriter();
            
            Entities.ForEach((Entity entity, int entityInQueryIndex, in Launcher launcher, in LocalToWorld ltw) =>
            {
                if (!launcher.ShouldLaunch)
                    return;
                
                var newProjectile = commandBuffer.Instantiate(entityInQueryIndex, launcher.ProjectilePrefab);

                commandBuffer.SetComponent(entityInQueryIndex, newProjectile, new Translation {Value = ltw.Position});

                float2 initialVelocity = new float2(0);
                if (HasComponent<Velocity2D>(entity))
                {
                    initialVelocity = GetComponent<Velocity2D>(entity).Linear;
                }
                initialVelocity += launcher.TargetDirection * launcher.InitialVelocityMagnitude;
                commandBuffer.SetComponent(entityInQueryIndex, newProjectile, new Velocity2D(initialVelocity, 0));
            }).ScheduleParallel();
            
            _endSimECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
}