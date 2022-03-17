using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    [InternalBufferCapacity(2)]
    public struct OverlapEventBuffer : IBufferElementData
    {
        public Entity Other;
        public float2 ApproximateContact;
        // public float MassRatio; //Other / self
        public float DistanceToSeperate;
        // public float2 OtherPosition;


        public OverlapEventBuffer(Entity other, float2 approximateContact, float distanceToSeperate)
        {
            Other = other;
            ApproximateContact = approximateContact;
            // MassRatio = massRatio;
            DistanceToSeperate = distanceToSeperate;
            // OtherPosition = otherPosition;
        }
    }
    
}

