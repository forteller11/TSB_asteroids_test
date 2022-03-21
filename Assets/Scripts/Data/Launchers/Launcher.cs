using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    public struct Launcher : IComponentData
    {
        public float InitialVelocityMagnitude;
        public Entity ProjectilePrefab;
        public float2 TargetDirection;
        public bool ShouldLaunch;
    }
}