using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct Launcher : IComponentData
    {
        public float InitialVelocityMagnitude;
        public Entity ProjectileOrigin;
        public Entity ProjectilePrefab;
        public bool ShouldLaunch;
    }
}