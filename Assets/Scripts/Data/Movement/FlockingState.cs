using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct FlockingState : IComponentData
    {
        public float2 DesiredDirection;
        public float DesiredDirectionChange;
        public float MaxVelocity;
        public float DefaultAcceleration;
        public float AvoidanceAcceleration;

        public float MiniumDistanceAway;
    }
}