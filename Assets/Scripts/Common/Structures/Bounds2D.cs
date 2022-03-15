using Unity.Mathematics;

namespace Charly.Common.Structures
{
    public struct Bounds2D
    {
        public float2 Center;
        public float2 HalfSize;
        
        public float2 Size => HalfSize + HalfSize;
        public float2 MaxExtents => Center + HalfSize;
        public float2 MinExtents => Center - HalfSize;

        public Bounds2D(float2 center, float2 size)
        {
            Center = center;
            HalfSize = size / 2f;
        }
    }
}