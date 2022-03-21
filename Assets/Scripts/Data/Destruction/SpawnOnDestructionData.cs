using Common.Structures;
using Unity.Entities;

namespace Charly.Data
{
    [InternalBufferCapacity(3)]
    public struct SpawnOnDestructionData : IBufferElementData
    {
        public Entity Entity;
        public FloatRange LinearVelocity;
        public FloatRange AngularVelocity;

        public SpawnOnDestructionData(Entity entity, FloatRange linearVelocityRange, FloatRange angularVelocityRange)
        {
            Entity = entity;
            LinearVelocity = linearVelocityRange;
            AngularVelocity = angularVelocityRange;
        }
    }
}