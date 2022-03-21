using Charly.Common.Structures;
using Unity.Entities;

namespace Charly.Data
{
    public struct WorldBounds : IComponentData
    {
        public Bounds2D Value;

        public WorldBounds(Bounds2D value)
        {
            Value = value;
        }
    }
}