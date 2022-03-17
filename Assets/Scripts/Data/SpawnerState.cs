using Unity.Entities;

namespace Charly.Data
{

    public struct SpawnDatas
    {
        public float SpawnChancePerSecond;
        // public int MaxObjects;
        public BlobArray<SpawnData> Datas;
    }
    public struct SpawnData
    {
        public Entity Prefab;
        public float NormalizedMinRate;
        public float NormalizedMaxRate;
    }
    
    public struct SpawnerState : IComponentData
    {
        public BlobAssetReference<SpawnDatas> SpawnDatasReference;

        public SpawnerState(BlobAssetReference<SpawnDatas> spawnDatasReference)
        {
            SpawnDatasReference = spawnDatasReference;
        }
    }
}