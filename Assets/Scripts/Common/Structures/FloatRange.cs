using System;
using Random = Unity.Mathematics.Random;

namespace Common.Structures
{
    [Serializable]
    public struct FloatRange
    {
        public float Min;
        public float Max;

        public FloatRange(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public readonly float NextRandom(ref Random random)
        {
            return random.NextFloat(Min, Max);
        }
        
    }
}