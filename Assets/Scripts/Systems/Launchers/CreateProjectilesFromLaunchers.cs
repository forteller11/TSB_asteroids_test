using Charly.Data;
using Charly.Systems.CustomEntityCommandBuffers;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Charly.Systems
{
    public class CreateProjectilesFromLaunchers : SystemBase
    {
        private BeforeTransformGroupECBSystem _beforeTransformECB;
        
        protected override void OnCreate()
        {
            _beforeTransformECB = World.GetOrCreateSystem<BeforeTransformGroupECBSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBufferConcurrent = _beforeTransformECB.CreateCommandBuffer().AsParallelWriter();
            
            Entities.ForEach((Entity entity, int entityInQueryIndex, in Launcher launcher, in LocalToWorld ltw) =>
            {
                if (!launcher.ShouldLaunch)
                    return;
                
                var newProjectile = commandBufferConcurrent.Instantiate(entityInQueryIndex, launcher.ProjectilePrefab);

                commandBufferConcurrent.SetComponent(entityInQueryIndex, newProjectile, new Translation {Value = ltw.Position});

                float2 initialVelocity = new float2(0);
                if (HasComponent<Velocity2D>(entity))
                {
                    initialVelocity = GetComponent<Velocity2D>(entity).Linear;
                }
                initialVelocity += launcher.TargetDirection * launcher.InitialVelocityMagnitude;
                commandBufferConcurrent.SetComponent(entityInQueryIndex, newProjectile, new Velocity2D(initialVelocity, 0));
            }).ScheduleParallel();
            
            _beforeTransformECB.AddJobHandleForProducer(Dependency);
        }
    }
}