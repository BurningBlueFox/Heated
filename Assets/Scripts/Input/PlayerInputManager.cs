using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace BurningBlueFox.Heated.Input
{
    public class PlayerInputManager : MonoBehaviour
    {
        private PlayerControls controls;
        private IProcessMovement playerMovement;
        private Vector2 currentPlayerMovementValue = new Vector2(0f, 0f);


        private void Awake()
        {
            controls = new PlayerControls();
            playerMovement = GetComponent(typeof(IProcessMovement)) as IProcessMovement;
        }

        private void OnEnable()
        {
            controls.Enable();
            controls.Character.Movement.performed += MovementPerformed;
            controls.Character.Movement.canceled += MovementCanceled;
            controls.Character.Interaction.performed += InteractionPerformed;
        }
        private void Update()
        {
            playerMovement?.ProcessMovement(currentPlayerMovementValue);
        }
        private void OnDisable()
        {
            controls.Character.Movement.performed -= MovementPerformed;
            controls.Character.Movement.canceled -= MovementCanceled;
            controls.Character.Interaction.performed -= InteractionPerformed;
            controls.Disable();
        }

        private void MovementPerformed(InputAction.CallbackContext obj)
        {
            currentPlayerMovementValue = obj.ReadValue<Vector2>();

            //Debug.Log("Movement Performed: \n" + obj.ReadValue<Vector2>()
            //    + obj.control.device.name);
        }
        private void MovementCanceled(InputAction.CallbackContext obj)
        {
            currentPlayerMovementValue.x = 0;
            currentPlayerMovementValue.y = 0;

            //Debug.Log("Movement Canceled: \n" + obj.ReadValue<Vector2>()
            //    + obj.control.device.name);
        }
        private void InteractionPerformed(InputAction.CallbackContext obj)
        {
            //Debug.Log("Interaction: performed");
        }
    }
}