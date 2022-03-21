using System.Collections.Generic;
using Charly.Authoring;
using Charly.Data;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Charly.Authoring
{
    [DisallowMultipleComponent]
    public class WaveSpawnerAuthoring : MonoBehaviour
    {
        [SerializeField] public List<SpawnWaveConfig> Waves;
    }

    [ConverterVersion("charly", 1)]
    [UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
    public class WaveDeclare : GameObjectConversionSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((WaveSpawnerAuthoring authoring) =>
            {
                foreach (var wave in authoring.Waves)
                {
                    foreach (var enemy in wave.Enemies)
                    {
                        DeclareReferencedPrefab(enemy.Prefab);
                    }
                }
            });
        }
    }
    
    [ConverterVersion("charly", 1)]
    public class WaveSpawnConversion : GameObjectConversionSystem
    {
        //todo [Refactor] This feels very boiler plate-y and error prone (dealing with valueType lists stored in structs leads to awkward and cumbersome patterns).
        //todo [Refactor] I'm sure there's some auto-conversion stuff I can do in the future to make converting scriptableObjects into FixedLists and/or AssetBlobs less painful.
        protected override void OnUpdate()
        {
            Entities.ForEach((WaveSpawnerAuthoring authoring) =>
            {
                var waveSpawnerDots = new WaveSpawnerState();

                for (var i = 0; i < authoring.Waves.Count; i++)
                {
                    var waveAuthoring = authoring.Waves[i];

                    var enemies = new FixedList512<SpawnEnemyData>();

                    for (int j = 0; j < waveAuthoring.Enemies.Length; j++)
                    {
                        var enemySpawnDataAuthoring = waveAuthoring.Enemies[j];
                        var enemySpawnDataDots = new SpawnEnemyData
                        {
                            Prefab = GetPrimaryEntity(enemySpawnDataAuthoring.Prefab)
                        };
                        
                        DstEntityManager.SetName(enemySpawnDataDots.Prefab, enemySpawnDataAuthoring.Prefab.name);
                        
                        enemies.Add(enemySpawnDataDots);
                    }
                    
                    waveSpawnerDots.Waves.Add(new SpawnWaveData()
                    {
                        MaxEnemiesInPlay = waveAuthoring.MaxObjectsInSimultaneousPlay,
                        SpawnChanceNormalizedPerSecond = waveAuthoring.SpawnChancePerSecondNormalized, 
                        Enemies = enemies
                    });
                }


                var authoringEntity = GetPrimaryEntity(authoring);
                DstEntityManager.AddComponentData(authoringEntity, waveSpawnerDots);
                DstEntityManager.AddComponentData(authoringEntity, new RandomState(authoring.GetInstanceID()));
            });
        }
    }
}