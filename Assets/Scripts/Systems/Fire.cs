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
        
        private EntityQueryDesc _bulletQueryDesc;
        protected override void OnCreate()
        {
            _bulletQueryDesc = new EntityQueryDesc
            {
                All = new[] { ComponentType.ReadWrite<Bullet>(), ComponentType.ReadOnly<Prefab>() },
                Options = EntityQueryOptions.IncludePrefab
            };
            
            _endSimECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var bulletQuery = GetEntityQuery(_bulletQueryDesc);
            //todo [Feature] allow for different bullet types and prefabs per gun
            //todo has to be a prefab
            var bulletPrefab = bulletQuery.GetSingletonEntity();
            if (Entity.Null == bulletPrefab)
            {
                Debug.LogError($"Cannot fire as system cannot find bullet prefab");
                return;
            }
            
            var input = GetSingleton<InputData>();
            
            var commandBuffer = _endSimECBSystem.CreateCommandBuffer().AsParallelWriter();
            
            Entities.ForEach((int entityInQueryIndex, in Translation translation, in Rotation rotation, in Gun gun) =>
            {
                if (input.Primary.IsDown)
                {
                    var newBullet = commandBuffer.Instantiate(entityInQueryIndex,  bulletPrefab);

                    var rotatedFireOffset = math.mul(rotation.Value, new float3(gun.FireOffset, 0));
                    var rotatedDir = math.mul(rotation.Value, new float3(0, 1, 0)).xy;
                    
                    commandBuffer.SetComponent(entityInQueryIndex, newBullet, new Translation(){Value = translation.Value + rotatedFireOffset});
                    //todo don't override mass here as it may be set in the prefab... make mass a seperate component in future?
                    commandBuffer.SetComponent(entityInQueryIndex, newBullet, new Velocity2D(rotatedDir * gun.InitialVelocityMagnitude, 0, .1f));
                }

            }).ScheduleParallel();
        }
    }
}