using System.Collections.Generic;
using Charly.Data;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Charly.Authoring
{
    [DisallowMultipleComponent]
    public class EnemySpawnSettings : MonoBehaviour, IComponentData
    {
        [SerializeField] public EnemySpawnData Value;
    }

    [UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
    public class EnemySpawnPrefabsDeclare : GameObjectConversionSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((EnemySpawnSettings authoring) =>
            {
                foreach (var enemy in authoring.Value.Enemies)
                {
                    DeclareReferencedPrefab(enemy.Prefab);
                    Debug.Log("Prefab Declared: " + enemy.Prefab);
                }
            });
        }
    }
    public class EnemySpawnConversion : GameObjectConversionSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((EnemySpawnSettings authoring) =>
            {
                var spawnSettings = authoring.Value;
                
                float totalSpawnRates = 0;
                foreach (var enemy in spawnSettings.Enemies)
                {
                    totalSpawnRates += enemy.SpawnRate;
                }

                using var builder = new BlobBuilder(Allocator.Temp);
                ref var root = ref builder.ConstructRoot<SpawnDatas>();
                root.SpawnChancePerSecond = authoring.Value.SpawnChancePerSecond;
                
                var spawnDatasBlob = builder.Allocate(ref root.Datas, spawnSettings.Enemies.Length);
                float probability = 0;
                for (var i = 0; i < spawnSettings.Enemies.Length; i++)
                {
                    var enemy = spawnSettings.Enemies[i];
                    
                    var prefabEntity = GetPrimaryEntity(enemy.Prefab);
                    Debug.Log("Asteroid prefab entity: " + prefabEntity );
                    spawnDatasBlob[i].Prefab = prefabEntity;
                    
                    spawnDatasBlob[i].NormalizedMinRate = probability;
                    probability += enemy.SpawnRate / totalSpawnRates;
                    spawnDatasBlob[i].NormalizedMaxRate = probability;
                }

                var blobReference = builder.CreateBlobAssetReference<SpawnDatas>(Allocator.Persistent);
                
                var authoringEntity = GetPrimaryEntity(authoring);
                DstEntityManager.AddComponentData(authoringEntity, new SpawnerState(blobReference));
                DstEntityManager.AddComponentData(authoringEntity, new RandomState(authoring.GetInstanceID()));

            });
        }
    }
}