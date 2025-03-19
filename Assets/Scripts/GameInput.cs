using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;

    private PlayerInputAction playerInputAction;

    private void Awake()
	{
        // Movement input
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();

        // Interact input
        playerInputAction.Player.Interact.performed += Interact_performed;
    }

	private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
        OnInteractAction?.Invoke(this, EventArgs.Empty); // SAME AS CHECK NOT NULL ON OnInteractAction event
    }

	public Vector2 GetMovementVectorNormalized()
	{
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();

        //Normalize every value regardless of using different key combination
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
