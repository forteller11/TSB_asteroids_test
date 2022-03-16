using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct Gun : IComponentData
    {
        public float InitialVelocityMagnitude;
        public float2 FireOffset;
        public Entity BulletPrefab;
    }
}