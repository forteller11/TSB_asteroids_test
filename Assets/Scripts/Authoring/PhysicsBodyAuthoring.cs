using Charly.Data;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Charly.Authoring
{
    public class PhysicsBodyAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [Header("Initial Velocities")] 
        public float2 LinearVelocity;
        public float AngularVelocity;

        [Header("Mass")] 
        public float Mass = 1;

        [Header("Drag")] 
        public float LinearDrag = 1;
        public float AngularDrag = 1;
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var props = new PhysicsProperties { LinearDrag = LinearDrag, AngularDrag = AngularDrag };
            props.SetMass(Mass);
            dstManager.AddComponentData(entity, props);

            var velocity = new Velocity2D(LinearVelocity, AngularVelocity);
            dstManager.AddComponentData(entity, velocity);
        }
    }
}