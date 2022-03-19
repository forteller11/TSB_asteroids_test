using Charly.Common.Utils;
using Charly.Data;
using UnityEngine;

namespace Charly.Authoring
{
    public class CircleCollider2DAuthoring : MonoBehaviour
    {
        public float LocalRadius = 1;
        public float WorldRadius => LocalRadius * (transform.localScale.x + transform.localScale.y) / 2;

        private void OnDrawGizmosSelected()
        {
            using (new DebugUtils.ColorFrame(Color.yellow))
            {
                Gizmos.DrawWireSphere(transform.position, WorldRadius);
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
                    Radius = authoring.WorldRadius
                };
                DstEntityManager.AddComponentData(entity, data);

                //NOTE: I'm not convinced EVERY entity with a collider should also necessarily have an overlapEventBuffer
                DstEntityManager.AddBuffer<OverlapEventBuffer>(entity);
            });
        }
    }
}