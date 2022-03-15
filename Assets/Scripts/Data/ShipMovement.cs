using Unity.Entities;

namespace Charly.Data
{
    [GenerateAuthoringComponent]
    public struct ShipMovement : IComponentData
    {
        public float LinearSensitivity;
        public float AngularSensitivity;
    }
}