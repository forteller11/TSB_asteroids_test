using Charly.Common.Utils;
using Charly.Data;
using UnityEngine;

namespace Charly.Authoring
{
    public class CircleCollider2DAuthoring : MonoBehaviour
    {
        public float Radius = 1;

        private void OnDrawGizmosSelected()
        {
            using (DebugUtils.CreateGizmoColorFrame(Color.yellow))
            {
                Gizmos.DrawWireSphere(transform.position, Radius);
            }
        }
    }
    public class CircleCollider2DConversion : GameObjectConversionSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((CircleCollider2DAuthoring authoring) =>
            {
                var entity = TryGetPrimaryEntity(authoring);

                var data = new Charly.Data.Collider2D()
                {
                    Type = ColliderType.Circle,
                    Radius = authoring.Radius
                };
                DstEntityManager.AddComponentData(entity, data);

                //NOTE: I'm not convinced EVERY entity with a collider should also necessarily have an overlapEventBuffer
                DstEntityManager.AddBuffer<OverlapEventBuffer>(entity);
            });
        }
    }
}