using Unity.Entities;
using Unity.Mathematics;

namespace Charly.Data
{
    public struct PhysicsProperties : IComponentData
    {
        //Note: Mass is not currently used in any calculations
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