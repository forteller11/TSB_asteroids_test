using Unity.Entities;

namespace Charly.Data
{
    public struct Collider2D : IComponentData
    {
        public ColliderType Type;
        public float Radius;
        // todo public Mask ColliderWith;
    }
    public enum ColliderType
    {
        Inactive=default,
        Circle,
    }
    
}