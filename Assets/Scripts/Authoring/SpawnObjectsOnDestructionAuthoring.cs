using System.Collections.Generic;
using Charly.Data;
using Common.Structures;
using Unity.Entities;
using UnityEngine;

namespace Charly.Authoring
{
    public class SpawnObjectsOnDestructionAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public List<GameObject> ToSpawns;
        public FloatRange InitialLinearVelocity = new FloatRange(.1f,1f);
        public FloatRange InitialAngularVelocity = new FloatRange(.01f,.1f);
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddBuffer<SpawnOnDestructionData>(entity);
            var spawnOnDestructionBuffer = dstManager.GetBuffer<SpawnOnDestructionData>(entity);
            foreach (var toSpawn in ToSpawns)
            {
                spawnOnDestructionBuffer.Add(new SpawnOnDestructionData(
                    conversionSystem.GetPrimaryEntity(toSpawn), 
                    InitialLinearVelocity, 
                    InitialAngularVelocity));
            }
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            foreach (var toSpawn in ToSpawns)
                referencedPrefabs.Add(toSpawn);
        }
        
    }
}