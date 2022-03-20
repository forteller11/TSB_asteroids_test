using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    public struct InputData : IComponentData
    {
        public float Movement;
        public float Turn;
        public StatefulButton Primary;
        public StatefulButton Secondary;


        public override string ToString()
        {
            return $"{nameof(Movement)}: {Movement}, {nameof(Turn)}: {Turn}, {nameof(Primary)}: {Primary}";
        }
    }

    public struct StatefulButton
    {
        public bool IsDown;
        public bool PressedThisTick;
        public bool ReleasedThisTick;

        public override string ToString()
        {
            return
                $"Stateful Button: {nameof(IsDown)}: {IsDown}, {nameof(PressedThisTick)}: {PressedThisTick}, {nameof(ReleasedThisTick)}: {ReleasedThisTick}";
        }
    }
}