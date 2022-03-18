using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Charly.Data
{

    public struct SpawnWaveData
    {
        public float SpawnChancePerSecond;
        // public int MaxObjects;
        public BlobArray<SpawnData> Datas;
    }
    public struct SpawnData
    {
        public Entity Prefab;
        private float2 MaxInitialSpeed;
        private float Rate;
    }

    [Serializable]
    //20 bytes
    public struct Spawn
    {
        public Entity Prefab;
        public float2 MaxInitialSpeed;
        // [HideInInspector] public float SpawnWeight;
        public int NumberToSpawn;
        // public int RemainingCount =>NumberToSpawn;
    }
    
    //could hold 128/20 = 6 types
    [Serializable]
    public struct SpawnWave
    {
        public float SpawnChanceNormalizedPerSecond;
        public int MaxEnemiesInPlay;
        public int CurrentEnemiesInPlay;
        public FixedList128<Spawn> Enemies;
    }
    
    //can hold 5 waves
    [GenerateAuthoringComponent]
    public struct WaveSpawner : IComponentData
    {
        public FixedList512<SpawnWave> Waves;
        [HideInInspector] public int WaveIndex;
    }
}