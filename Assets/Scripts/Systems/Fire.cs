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
            //todo [Feature] allow for different bullet types and prefabs per gun
            
            var input = GetSingleton<InputData>();
            
            var commandBuffer = _endSimECBSystem.CreateCommandBuffer().AsParallelWriter();
            
            Entities.ForEach((int entityInQueryIndex, in Rotation rotation, in Gun gun) =>
            {
                if (input.Primary.IsDown)
                {
                    var newBullet = commandBuffer.Instantiate(entityInQueryIndex, gun.BulletPrefab);

                    var ltwBulletOrigin = GetComponent<LocalToWorld>(gun.ProjectileOrigin);
                    commandBuffer.SetComponent(entityInQueryIndex, newBullet, new Translation {Value = ltwBulletOrigin.Position});
                    //todo don't override mass here as it may be set in the prefab... make mass a seperate component in future?
                    
                    var rotatedDir = math.mul(rotation.Value, new float3(0, 1, 0)).xy;
                    commandBuffer.SetComponent(entityInQueryIndex, newBullet, new Velocity2D(rotatedDir * gun.InitialVelocityMagnitude, 0, .1f));
                }
            }).ScheduleParallel();
        }
    }
}