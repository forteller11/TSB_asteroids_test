using Unity.Entities;

namespace Charly.Data
{
    [InternalBufferCapacity(3)]
    public struct SpawnOnDestructionData : IBufferElementData
    {
        public Entity Entity;
        public float VelocityMagnitude;

        public SpawnOnDestructionData(Entity entity, float velocityMagnitude)
        {
            Entity = entity;
            VelocityMagnitude = velocityMagnitude;
        }
    }
}