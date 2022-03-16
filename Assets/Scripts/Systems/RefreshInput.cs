using Charly.Data;
using Unity.Entities;
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
            
            if (HasSingleton<InputData>())
            {
                Debug.LogWarning($"This system expects to be responsible for the creation of the ControlData struct... but it already exists, likely a bug");
            }
            else
            {
                // Remarks: didn't use conversion workflow to create ControlsData as likely we'll ALWAYS want input
                // So forcing every scene to include it explicitly could be bug prone and burdensome.
                EntityManager.CreateEntity(typeof(InputData));
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
            if (TryGetSingleton<InputData>(out var controls))
            {
                controls.Movement = _controlsAsset.Game.Thrust.ReadValue<float>();
                controls.Turn = _controlsAsset.Game.Turn.ReadValue<float>();
                controls.Primary.IsDown = _controlsAsset.Game.PrimaryAction.IsPressed();
                controls.Primary.PressedThisTick = _controlsAsset.Game.PrimaryAction.WasPressedThisFrame();
                controls.Primary.ReleasedThisTick = _controlsAsset.Game.PrimaryAction.WasReleasedThisFrame();
                
                SetSingleton(controls);
            }
        }
    }
}