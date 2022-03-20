using Unity.Entities;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct TargetEntity : IComponentData
    {
    public Entity Target;
    public float RateOfFire;
    public float TimeUntilNextFire;
    }
}