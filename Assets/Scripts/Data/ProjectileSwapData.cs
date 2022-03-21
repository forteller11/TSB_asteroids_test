using Unity.Entities;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct ProjectileSwapData : IComponentData
    {
        public Entity Bullet;
        public float VelocityMagnitude;
    }
}