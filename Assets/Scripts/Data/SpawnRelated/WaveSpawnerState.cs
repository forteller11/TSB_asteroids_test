using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Charly.Data
{
    
    [Serializable]
    //20 bytes
    public struct SpawnEnemyData
    {
        public Entity Prefab;
        public float2 MaxInitialSpeed;
        // [HideInInspector] public float SpawnWeight;
        // public int RemainingCount =>NumberToSpawn;
    }
    
    //could hold 128/20 = 6 types
    [Serializable]
    public struct SpawnWaveData
    {
        public int EnemyIndex;
        public float SpawnChanceNormalizedPerSecond;
        public int MaxEnemiesInPlay;
        public int EnemiesInPlay;
        public FixedList512<SpawnEnemyData> Enemies;
    }
    
    //can hold 5 waves
    [GenerateAuthoringComponent]
    public struct WaveSpawnerState : IComponentData
    {
        public FixedList4096<SpawnWaveData> Waves;
        [HideInInspector] public int WaveIndex;
    }
}