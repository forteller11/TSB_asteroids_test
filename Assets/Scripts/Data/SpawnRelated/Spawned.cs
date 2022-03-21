using Unity.Entities;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct Spawned : ISystemStateComponentData
    {
        public Entity Spawner;
        public int WaveIndex;

        public Spawned(Entity spawner, int waveIndex)
        {
            Spawner = spawner;
            WaveIndex = waveIndex;
        }
    }
}