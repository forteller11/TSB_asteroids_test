using Unity.Entities;

namespace Charly.Data
{
    public struct LifeSpan : IComponentData
    {
        public float SecondsTotal;
        public float SecondsCurrent;

        public LifeSpan(float secondsTotal)
        {
            SecondsTotal = secondsTotal;
            SecondsCurrent = secondsTotal;
        }

        public float LifeLeftNormalized() => SecondsCurrent / SecondsTotal;
    }
}