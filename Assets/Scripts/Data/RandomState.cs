using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    public struct RandomState : IComponentData
    {
        public Random Value;

        public RandomState(uint initialSeed)
        {
            Value = new Random(initialSeed);
        }
        
        public RandomState(int initialSeed) : this((uint) initialSeed) { }
    }
}