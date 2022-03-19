using System;
using System.Collections.Generic;
using UnityEngine;

namespace Charly.Authoring
{
    [CreateAssetMenu(menuName = "Asteroids / AsteroidsSpawnData")]
    public class SpawnWaveConfig : ScriptableObject
    {
        [SerializeField] public int MaxObjectsInSimultaneousPlay = 5;
        [SerializeField] public float SpawnChancePerSecondNormalized = 0.1f;
        [SerializeField] public SpawnDataAuthoring [] Enemies;
    }

    [Serializable]
    public class SpawnDataAuthoring
    {
        public GameObject Prefab;
    }
}