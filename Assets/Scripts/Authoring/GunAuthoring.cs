using Charly.Data;
using Mono.CompilerServices.SymbolWriter;
using Unity.Assertions;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Charly.Authoring
{
    public class GunAuthoring : MonoBehaviour
    {
        public float InitialVelocityMagnitude = 1;
        public float2 BulletOffset;
        public GameObject GunPrefab;
    }

    public class GunConversion : GameObjectConversionSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((GunAuthoring authoring) =>
            {
                
                Assert.IsNotNull(authoring.GunPrefab);
                
                DeclareReferencedPrefab(authoring.GunPrefab);

                var entity = GetPrimaryEntity(authoring);
                DstEntityManager.AddComponentData(entity, new Gun()
                {
                    BulletPrefab = Entity.Null,
                    FireOffset = authoring.BulletOffset,
                    InitialVelocityMagnitude = authoring.InitialVelocityMagnitude
                });

            });

        }
    }
}