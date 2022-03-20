using Charly.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Charly.Systems
{
    public class Fire : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimECBSystem;
        
        protected override void OnCreate()
        {
            _endSimECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = _endSimECBSystem.CreateCommandBuffer().AsParallelWriter();
            
            Entities.ForEach((Entity entity, int entityInQueryIndex, in Rotation rotation, in Launcher gun) =>
            {
                if (!gun.ShouldLaunch)
                    return;
                
                var newProjectile = commandBuffer.Instantiate(entityInQueryIndex, gun.ProjectilePrefab);

                var ltwBulletOrigin = GetComponent<LocalToWorld>(gun.ProjectileOrigin);
                commandBuffer.SetComponent(entityInQueryIndex, newProjectile, new Translation {Value = ltwBulletOrigin.Position});
                    
                var rotatedDir = math.mul(rotation.Value, new float3(0, 1, 0)).xy;
                commandBuffer.SetComponent(entityInQueryIndex, newProjectile, new Rotation(){Value = rotation.Value});

                float2 initialVelocity = new float2(0);
                if (HasComponent<Velocity2D>(entity))
                {
                    initialVelocity = GetComponent<Velocity2D>(entity).Linear;
                }
                initialVelocity += rotatedDir * gun.InitialVelocityMagnitude;
                commandBuffer.SetComponent(entityInQueryIndex, newProjectile, new Velocity2D(initialVelocity, 0));
            }).ScheduleParallel();
            
            _endSimECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
}