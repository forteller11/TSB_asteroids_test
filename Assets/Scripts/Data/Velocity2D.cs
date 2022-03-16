using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct Velocity2D : IComponentData
    {
        public float2 Linear;
        public float Angular;
        public float Mass;
        
        public float InverseMass => math.rcp(Mass); //todo [Perf] remove this, getters with hidden perf costs are gross. 
        
        public Velocity2D(float2 linear, float angular, float mass)
        {
            Linear = linear;
            Angular = angular;
            Mass = mass;
        }
    }
}