using System;
using System.Collections.Generic;
using UnityEngine;

namespace Charly.Authoring
{
    [CreateAssetMenu(menuName = "Asteroids / AsteroidsSpawnData")]
    public class EnemySpawnData : ScriptableObject
    {
        [SerializeField] public float SpawnChancePerSecond = 0.01f;
        [SerializeField] public SpawnData [] Enemies;
    }

    [Serializable]
    public class SpawnData
    {
        public GameObject Prefab;
        public float SpawnRate = 1;
    }
}