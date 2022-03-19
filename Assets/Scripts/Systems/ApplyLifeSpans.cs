using Charly.Data;
using Unity.Entities;

namespace Charly.Systems
{
    public class ApplyLifeSpans : SystemBase
    {
        private BeginSimulationEntityCommandBufferSystem _beginSimulationECBSystem;
        protected override void OnCreate()
        {
            _beginSimulationECBSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;
            var commandBuffer = _beginSimulationECBSystem.CreateCommandBuffer().AsParallelWriter();
            Entities.ForEach((Entity entity, int entityInQueryIndex, ref LifeSpan lifeSpan) =>
            {
                lifeSpan.SecondsCurrent -= dt;

                if (lifeSpan.SecondsCurrent < 0)
                {
                    commandBuffer.DestroyEntity(entityInQueryIndex, entity);
                }
                
            }).ScheduleParallel();
            
            _beginSimulationECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
}