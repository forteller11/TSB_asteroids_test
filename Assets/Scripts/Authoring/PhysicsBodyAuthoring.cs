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
        [Range(0,1)] public float LinearConservation = 1;
        [Range(0,1)] public float AngularConservation = 1;
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var props = new PhysicsProperties { LinearDrag = LinearConservation, AngularDrag = AngularConservation };
            props.SetMass(Mass);
            dstManager.AddComponentData(entity, props);

            var velocity = new Velocity2D(LinearVelocity, AngularVelocity);
            dstManager.AddComponentData(entity, velocity);
        }
    }
}