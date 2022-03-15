using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct Velocity2D : IComponentData
    {
        public float2 LinearVelocity;
        public float AngularVelocity;
        public float Mass;
        public float InverseMass => math.rcp(Mass); //todo remove this, getters with hidden perf costs are gross. 
    }
}