using Unity.Entities;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct Destructible : IComponentData
    {
        public Mask DestroyedBy;
    }
}