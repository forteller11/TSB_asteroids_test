using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    public struct Velocity2D : IComponentData
    {
        public float2 Linear;
        public float Angular;
        
        
        public Velocity2D(float2 linear, float angular)
        {
            Linear = linear;
            Angular = angular;
        }
    }
}