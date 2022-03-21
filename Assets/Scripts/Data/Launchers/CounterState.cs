using Unity.Entities;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct CounterState : IComponentData
    {
        public float CurrentCount;
        public float MaxTime;
        public bool FinishedThisFrame;

        public CounterState(float maxTime) : this()
        {
            MaxTime = maxTime;
        }
    }
}