using System;
using Charly.Common.Structures;
using Charly.Common.Utils;
using Charly.Data;
using Unity.Mathematics;
using UnityEngine;

namespace Charly.Authoring
{
    [ExecuteInEditMode]
    public class CameraToBoundsAuthoring : MonoBehaviour
    {
        [SerializeField] [HideInInspector] public float2 BoundsSize = new float2(0,0);
        [SerializeField] float2 _worldPadding = new float2(1,1);

        [SerializeField] Camera _attachedCamera;
        private void Awake()
        {
            if (_attachedCamera == null)
                _attachedCamera = GetComponent<Camera>();
        }

        private void OnValidate()
        {
            if (!_attachedCamera.orthographic)
            {
                _attachedCamera.orthographic = true;
                Debug.LogWarning($"{nameof(CameraToBoundsAuthoring)} expects an orthographic camera to work properly, so {_attachedCamera} has been changed to orthographic.");
            }
        }

        public void Update()
        {
            float2 cameraWorldBounds = GetCameraWorldBounds();
            BoundsSize = cameraWorldBounds + _worldPadding;
        }

        private void OnDrawGizmos()
        {
            var cameraWorldBounds = GetCameraWorldBounds();

            using (new DebugUtils.ColorFrame(new Color(0.88f, 0.69f, 0.36f, 0.67f)))
            {
                Gizmos.DrawWireCube(transform.position, new float3(cameraWorldBounds, 0));
            }
            using (new DebugUtils.ColorFrame(new Color(1f, 0.65f, 0.29f)))
            {
                Gizmos.DrawWireCube(transform.position, new float3(BoundsSize, 0));
            }
        }

        float2 GetCameraWorldBounds()
        {
            var orthographicSize = _attachedCamera.orthographicSize;
            //orthographic size determines y extents, x extents vary with the camera's aspect ratio
            float2 cameraWorldBounds = new float2(orthographicSize * _attachedCamera.aspect , orthographicSize) * 2;
            return cameraWorldBounds;
        }
    }
    
    public class CameraToBoundsConversion : GameObjectConversionSystem
    {
        protected override void OnUpdate()
        {
            int authoringMonobehaviorsInScene = 0;
            //NOTE: this will be converted to singleton data at runtime
            Entities.ForEach((CameraToBoundsAuthoring authoring) =>
            {
                float3 position3D = authoring.transform.position;
                var bounds = new Bounds2D(position3D.xy, authoring.BoundsSize);
                
                var primaryEntity = GetPrimaryEntity(authoring);
                DstEntityManager.AddComponentData(primaryEntity, new WorldBounds(bounds));

                authoringMonobehaviorsInScene++;
            });

            if (authoringMonobehaviorsInScene > 1)
                Debug.LogWarning($"There is more than 1 {nameof(CameraToBoundsAuthoring)} in the scene when only 1 is expected.");
        }
    }
}