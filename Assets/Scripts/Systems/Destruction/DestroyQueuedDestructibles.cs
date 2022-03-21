using Charly.Data;
using Unity.Entities;
using Unity.Transforms;

namespace Charly.Systems
{
    //this only handles cases with transforms of 1 child deep... arbitrary hierarchy trees not supported.
    [UpdateAfter(typeof(FindOverlapsAndQueueDestruction))]
    public class DestroyQueuedDestructibles : SystemBase
    {
        private BeginPresentationEntityCommandBufferSystem _beginPresentationECBSystem; 
        protected override void OnCreate()
        {
            _beginPresentationECBSystem = World.GetOrCreateSystem<BeginPresentationEntityCommandBufferSystem>();
        }
        
        protected override void OnUpdate()
        {
            var commandBufferConcurrent1 = _beginPresentationECBSystem.CreateCommandBuffer().AsParallelWriter();
            Entities.ForEach((Entity currentEntity, int entityInQueryIndex, in Destructible destructible) =>
                {
                    if (destructible.BeingDestroyed)
                        commandBufferConcurrent1.DestroyEntity(entityInQueryIndex, currentEntity);
                                
                })
                .ScheduleParallel();
            
            var commandBufferConcurrent2 = _beginPresentationECBSystem.CreateCommandBuffer().AsParallelWriter();
            Entities.ForEach((Entity currentEntity, int entityInQueryIndex, in Destructible destructible, in DynamicBuffer<Child> children) =>
                {
                    if (destructible.BeingDestroyed)
                    {
                        foreach (var child in children)
                        {
                            commandBufferConcurrent2.DestroyEntity(entityInQueryIndex, child.Value);
                        }
                        commandBufferConcurrent2.DestroyEntity(entityInQueryIndex, currentEntity);

                    }

                })
                .ScheduleParallel();
                    
            _beginPresentationECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
