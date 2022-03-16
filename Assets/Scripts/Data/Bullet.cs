using Unity.Entities;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct Bullet : IComponentData
    {
        public EntityTypeMask Ignore;
    }
}