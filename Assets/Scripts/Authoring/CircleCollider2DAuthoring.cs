using Charly.Utils;
using Charly.Data;
using UnityEngine;
using Collider2D = Charly.Data.Collider2D;

namespace Authoring
{
    public class CircleCollider2DAuthoring : MonoBehaviour
    {
        public float Radius = 1;

        private void OnDrawGizmosSelected()
        {
            Debug.Log(Gizmos.color);

            using (DebugUtils.CreateGizmoColorFrame(Color.yellow))
            {
                Debug.Log(Gizmos.color);
                Gizmos.DrawWireSphere(transform.position, Radius);
            }
            Debug.Log(Gizmos.color);
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
            });
        }
    }
}