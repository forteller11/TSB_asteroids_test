using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    [InternalBufferCapacity(2)]
    public struct OverlapEventBuffer : IBufferElementData
    {
        public Entity Other;
        public float2 ApproximateContact;

        public OverlapEventBuffer(Entity other, float2 approximateContact)
        {
            Other = other;
            ApproximateContact = approximateContact;
        }
    }
    
}

