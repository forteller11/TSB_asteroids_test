using System;
using Unity.Entities;
using Random = Unity.Mathematics.Random;

namespace Charly.Data
{
    public struct RandomState : IComponentData
    {
        public Random Value;

        public RandomState(uint initialSeed)
        {
            Value = new Random(initialSeed);
        }

        public RandomState(int initialSeed) 
            : this(IntToUintLossless(initialSeed)) { }
        
        //Doesn't lose 1 bit of information like casting to (uint) would, as it would just ignore the signed bit.
        public static uint IntToUintLossless(int integer)
        {
            return (uint) (integer + UInt32.MaxValue / 2);
        }
    }
}