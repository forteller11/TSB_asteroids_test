using Charly.Data;
using Unity.Entities;

namespace Charly.Systems
{
    [UpdateAfter(typeof(FindOverlapsAndQueueDestruction))]
    public class DestroyQueuedDestructibles : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationECBSystem; 
        protected override void OnCreate()
        {
            _endSimulationECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
        
        protected override void OnUpdate()
        {
            var commandBufferConcurrent = _endSimulationECBSystem.CreateCommandBuffer().AsParallelWriter();
            Entities
                .ForEach((Entity currentEntity, int entityInQueryIndex,ref Destructible destructible) =>
                {
                    if (destructible.BeingDestroyed)
                        commandBufferConcurrent.DestroyEntity(entityInQueryIndex, currentEntity);
                                
                })
                .ScheduleParallel();
                    
            _endSimulationECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
