using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct Launcher : IComponentData
    {
        public float InitialVelocityMagnitude;
        public Entity ProjectilePrefab;
        public float2 TargetDirection;
        public bool ShouldLaunch;
    }
}