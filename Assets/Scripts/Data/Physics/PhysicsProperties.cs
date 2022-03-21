using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    public struct PhysicsProperties : IComponentData
    {
        public float Mass { get; private set; }
        public float InverseMass { get; private set; }
        
        public float LinearDrag;
        public float AngularDrag;

        public void SetMass(float mass)
        {
            Mass = mass;
            InverseMass = math.rcp(mass);
        }
    }
}