// using System;
// using System.Collections.Generic;
// using Charly.Common.Utils;
// using Charly.Data;
// using Unity.Assertions;
// using Unity.Entities;
// using UnityEngine;
//
// namespace Charly.Authoring
// {
//     public class LauncherAuthoring : MonoBehaviour
//     {
//         public float InitialVelocityMagnitude = 1;
//         public GameObject BulletPrefab;
//         // public Driver DrivenBy;
//
//         public enum Driver
//         {
//             None,
//             Input = default,
//             Target
//         }
//
//         private void OnValidate()
//         {
//         }
//     }
//
//     [UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
//     public class GunPrefabDeclare : GameObjectConversionSystem
//     {
//         protected override void OnUpdate()
//         {
//             Entities.ForEach((LauncherAuthoring authoring) =>
//             {
//                 Assert.IsNotNull(authoring.BulletPrefab);
//                 DeclareReferencedPrefab(authoring.BulletPrefab);
//             });
//         }
//     }
//     
//     public class LauncherConversion : GameObjectConversionSystem
//     {
//         protected override void OnUpdate()
//         {
//             Entities.ForEach((LauncherAuthoring authoring) =>
//             {
//                 Assert.IsNotNull(authoring.BulletPrefab);
//                 
//                 var entity = GetPrimaryEntity(authoring);
//                 var gunPrefabEntity = GetPrimaryEntity(authoring.BulletPrefab);
//                 DstEntityManager.AddComponentData(entity, new Launcher
//                 {
//                     ProjectilePrefab = gunPrefabEntity,
//                     InitialVelocityMagnitude = authoring.InitialVelocityMagnitude
//                 });
//
//                 // if (authoring.DrivenBy == LauncherAuthoring.Driver.Input)
//                 // {
//                 //     DstEntityManager.AddComponentData(entity, new InputDrivenTag());
//                 // }
//                 // if ()
//                 // else
//                 // {
//                 //     throw new NotImplementedException();
//                 // }
//
//             });
//
//         }
//     }
// }