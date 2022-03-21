using Charly.Data;
using Unity.Assertions;
using Unity.Entities;
using UnityEngine;

namespace Charly.Authoring
{
    public class LauncherAuthoring : MonoBehaviour
    {
        public float InitialVelocityMagnitude = 1;
        public GameObject BulletPrefab;
        public Driver DrivenBy;

        //Driver: Target
        [Header("Settings if Driver: Target")]
        public float RateOfFire;
        public GameObject Target;

        public enum Driver
        {
            Input = default,
            Target, 
            None = 1000000000
        }
    }

    [UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
    public class LauncherPrefabDeclare : GameObjectConversionSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((LauncherAuthoring authoring) =>
            {
                Assert.IsNotNull(authoring.BulletPrefab);
                DeclareReferencedPrefab(authoring.BulletPrefab);
            });
        }
    }
    
    public class LauncherConversion : GameObjectConversionSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((LauncherAuthoring authoring) =>
            {
                Assert.IsNotNull(authoring.BulletPrefab);
                
                var entity = GetPrimaryEntity(authoring);
                var gunPrefabEntity = GetPrimaryEntity(authoring.BulletPrefab);
                DstEntityManager.AddComponentData(entity, new Launcher
                {
                    ProjectilePrefab = gunPrefabEntity,
                    InitialVelocityMagnitude = authoring.InitialVelocityMagnitude
                });

                switch (authoring.DrivenBy)
                {
                    case LauncherAuthoring.Driver.Input:
                    {
                        DstEntityManager.AddComponentData(entity, new InputDrivenTag());
                        break;
                    }
                    case LauncherAuthoring.Driver.Target:
                    {
                        var targetEntity = GetPrimaryEntity(authoring.Target);
                        if (targetEntity == Entity.Null)
                            Debug.LogError($"Primary Entity of {authoring.Target} is null at {nameof(LauncherAuthoring)}");
                        
                        DstEntityManager.AddComponentData(entity, new TargetEntity(){Target = targetEntity});
                        DstEntityManager.AddComponentData(entity, new CounterState(){MaxTime = authoring.RateOfFire, CurrentCount = authoring.RateOfFire});
                        break;
                    }
                    default:
                        break;
                }
            });

        }
    }
}