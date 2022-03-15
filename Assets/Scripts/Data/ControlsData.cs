using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    public struct ControlsData : IComponentData
    {
        public float Movement;
        public float Turn;
        public StatefulButton Primary;


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

        public void RefreshWithPreviousState(bool isDownThisTick)
        {
            if (IsDown == isDownThisTick)
                return;

            IsDown = isDownThisTick;
            PressedThisTick = isDownThisTick;
            ReleasedThisTick = !isDownThisTick;
        }

        public override string ToString()
        {
            return
                $"Stateful Button: {nameof(IsDown)}: {IsDown}, {nameof(PressedThisTick)}: {PressedThisTick}, {nameof(ReleasedThisTick)}: {ReleasedThisTick}";
        }
    }
}