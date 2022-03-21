using Charly.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public class SwapProjectilesOfSelfAndChildren : SystemBase
    {
        private BeginSimulationEntityCommandBufferSystem _beginSimulationECBSystem;

        protected override void OnCreate()
        {
            _beginSimulationECBSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = _beginSimulationECBSystem.CreateCommandBuffer();
            
            //Only works for hierarchies of UP to 1 child deep, no more.
            Entities.ForEach((Entity entity, int entityInQueryIndex, in ShipMovement ship,
                in DynamicBuffer<OverlapEventBuffer> buffer, in DynamicBuffer<Child> children) =>
            {
                foreach (var overlap in buffer)
                {
                    if (!HasComponent<ProjectileSwapData>(overlap.Other)) 
                        continue;

                    var swapData = GetComponent<ProjectileSwapData>(overlap.Other);
                    var launcherWithNewProjectile = new Launcher()
                    {
                        InitialVelocityMagnitude = swapData.VelocityMagnitude,
                        ProjectilePrefab = swapData.Bullet,
                        TargetDirection = new float2(1, 0)
                    };
                    
                    if (HasComponent<Launcher>(entity))
                    {
                        commandBuffer.SetComponent(entity, launcherWithNewProjectile);
                    }
                    
                    foreach (var child in children)
                    {
                        if (!HasComponent<Launcher>(child.Value)) 
                            continue;
                        
                        commandBuffer.SetComponent(child.Value, launcherWithNewProjectile);
                    }
                }

            }).Schedule();

            _beginSimulationECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
}