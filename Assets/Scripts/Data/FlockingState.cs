using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct FlockingState : IComponentData
    {
        public float2 DesiredDirection;
        public float MaxVelocity;
        public float MaxAcceleration;

        public float2 detail;
    }
}