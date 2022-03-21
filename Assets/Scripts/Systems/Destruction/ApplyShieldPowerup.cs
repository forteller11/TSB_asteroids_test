using Charly.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public class ApplyShieldPowerup : SystemBase
    {
        private BeginSimulationEntityCommandBufferSystem _beginSimulationECBSystem;

        protected override void OnCreate()
        {
            _beginSimulationECBSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = _beginSimulationECBSystem.CreateCommandBuffer();

            
            Entities.ForEach((Entity entity, int entityInQueryIndex, in ShipMovement ship, in DynamicBuffer<OverlapEventBuffer> buffer, in DynamicBuffer<Child> children) =>
            {
                foreach (var overlap in buffer)
                {
                    if (HasComponent<ShieldTag>(overlap.Other))
                    {
                        commandBuffer.AddComponent(entity, new ShieldTag());
                    }
                }

            }).Schedule();
            
            _beginSimulationECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
}