using System.Collections.Generic;
using Charly.Data;
using Unity.Assertions;
using Unity.Entities;
using UnityEngine;

namespace Charly.Authoring
{
    public class GunAuthoring : MonoBehaviour
    {
        public float InitialVelocityMagnitude = 1;
        public GameObject BulletOrigin;
        public GameObject GunPrefab;
    }

    [UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
    public class GunPrefabDeclare : GameObjectConversionSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((GunAuthoring authoring) =>
            {
                Assert.IsNotNull(authoring.GunPrefab);
                DeclareReferencedPrefab(authoring.GunPrefab);
            });
        }
    }
    
    public class GunConversion : GameObjectConversionSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((GunAuthoring authoring) =>
            {

                var bulletOrigin = authoring.BulletOrigin == null 
                    ? authoring.gameObject 
                    : authoring.BulletOrigin;
                Assert.IsNotNull(authoring.GunPrefab);
                
                var entity = GetPrimaryEntity(authoring);
                var gunPrefabEntity = GetPrimaryEntity(authoring.GunPrefab);
                var bulletOriginEntity = GetPrimaryEntity(bulletOrigin);
                DstEntityManager.AddComponentData(entity, new Gun
                {
                    BulletPrefab = gunPrefabEntity,
                    ProjectileOrigin = bulletOriginEntity,
                    InitialVelocityMagnitude = authoring.InitialVelocityMagnitude
                });

            });

        }
    }
}