using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Movement movement;
    [SerializeField] MouseLook mouseLook;
    
    public PlayerControls controls;
    public PlayerControls.GroundMovementActions groundMovement;

    Vector2 horizontalInput;
    Vector2 mouseInput;
    
    private void Awake ()
    {
        controls = new PlayerControls();
        groundMovement = controls.GroundMovement;

        groundMovement.Move.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();

        groundMovement.Jump.performed += _ => movement.OnJumpPressed();

        groundMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        groundMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDestroy()
    {
        controls.Disable();
    }

    private void Update()
    {
        movement.ReceiveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);
    }
}
