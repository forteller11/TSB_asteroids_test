// using System.Collections.Generic;
// using Charly.Data;
// using Unity.Collections;
// using Unity.Entities;
// using UnityEngine;
//
// namespace Charly.Authoring
// {
//     [DisallowMultipleComponent]
//     public class EnemySpawnSettings : MonoBehaviour, IComponentData
//     {
//         [SerializeField] public EnemySpawnData Value;
//     }
//
//     [UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
//     public class EnemySpawnPrefabsDeclare : GameObjectConversionSystem
//     {
//         protected override void OnUpdate()
//         {
//             Entities.ForEach((EnemySpawnSettings authoring) =>
//             {
//                 foreach (var enemy in authoring.Value.Enemies)
//                 {
//                     DeclareReferencedPrefab(enemy.Prefab);
//                     Debug.Log("Prefab Declared: " + enemy.Prefab);
//                 }
//             });
//         }
//     }
//     public class EnemySpawnConversion : GameObjectConversionSystem
//     {
//         protected override void OnUpdate()
//         {
//             Entities.ForEach((EnemySpawnSettings authoring) =>
//             {
//                 var spawnSettings = authoring.Value;
//                 
//                 float totalSpawnRates = 0;
//                 foreach (var enemy in spawnSettings.Enemies)
//                 {
//                     totalSpawnRates += enemy.SpawnRate;
//                 }
//
//                 
//                 
//                 var authoringEntity = GetPrimaryEntity(authoring);
//                 DstEntityManager.AddComponentData(authoringEntity, new SpawnerState(blobReference));
//                 DstEntityManager.AddComponentData(authoringEntity, new RandomState(authoring.GetInstanceID()));
//
//             });
//         }
//     }
// }