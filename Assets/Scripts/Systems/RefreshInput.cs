using Data;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Charly.Systems
{
    /// <summary>
    /// Refreshes ControlsData with input. Responsible for life time of ControlData Singleton.
    /// </summary>
     
    public class RefreshInput : SystemBase
    {
        private MainControls _controlsAsset;

        protected override void OnCreate()
        {
            _controlsAsset = new MainControls();
            
            if (HasSingleton<ControlsData>())
            {
                Debug.LogWarning($"This system expects to be responsible for the creation of the ControlData struct... but it already exists, likely a bug");
            }
            else
            {
                // Remarks: didn't use conversion workflow to create ControlsData as likely we'll ALWAYS want input
                // So forcing every scene to include it explicitly could be bug prone and burdensome.
                EntityManager.CreateEntity(typeof(ControlsData));
            }
        }

        protected override void OnStartRunning()
        {
            _controlsAsset.Enable();
        }

        protected override void OnStopRunning()
        {
            _controlsAsset.Disable();
        }

        protected override void OnUpdate()
        {
            //refresh inputs
            if (TryGetSingleton<ControlsData>(out var controls))
            {
                controls.Movement = _controlsAsset.Game.Movement.ReadValue<Vector2>();
                controls.Turn = _controlsAsset.Game.Turn.ReadValue<float>();
                bool isDown = _controlsAsset.Game.PrimaryAction.IsPressed();
                controls.Primary.RefreshWithPreviousState(isDown);
            }
            Debug.Log(controls.Movement);
            Debug.Log(controls.Turn);
            Debug.Log(controls.Primary);
        }
        
    }
}