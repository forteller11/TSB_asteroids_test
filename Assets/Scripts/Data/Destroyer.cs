using Unity.Entities;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct Destroyer : IComponentData
    {
        public Mask Mask;

        public Destroyer(Mask destroy)
        {
            Mask = destroy;
        }
    }
}