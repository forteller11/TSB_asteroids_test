using Charly.Data;
using Unity.Entities;
using UnityEngine;

namespace Charly.Authoring
{
    public class LifeSpanAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float LifeSpanInSeconds = 1;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new LifeSpan(LifeSpanInSeconds));
        }
    }
}