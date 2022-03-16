using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct Gun : IComponentData
    {
        public float InitialVelocityMagnitude;
        public Entity ProjectileOrigin;
        public Entity BulletPrefab;
    }
}