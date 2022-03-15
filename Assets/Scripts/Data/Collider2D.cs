using Unity.Entities;

namespace Charly.Data
{
    public struct Collider2D : IComponentData
    {
        public ColliderType Type;
        public float Radius;
    }
    public enum ColliderType
    {
        None=default,
        Circle,
    }
    
}