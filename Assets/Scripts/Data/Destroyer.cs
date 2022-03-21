using Unity.Entities;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct Destroyer : IComponentData
    {
        public Mask TypeOfObject;

        public Destroyer(Mask destroy)
        {
            TypeOfObject = destroy;
        }
    }
}