using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;

    private PlayerInputAction playerInputAction;
    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Move.Enable();

        playerInputAction.Player.Interact.Enable();
        playerInputAction.Player.InteractAlternate.Enable();
        playerInputAction.Player.Interact.performed += Interact_performed;
        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performed;
    
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalize()
    {
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>(); 



        inputVector = inputVector.normalized;
        return inputVector;
    }
}
