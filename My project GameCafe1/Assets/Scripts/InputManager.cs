using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerInputAction playerInputActions;
    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputAction();
        playerInputActions.Player.Enable();
    }
   
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
    public bool IsRunning()
    {
        return playerInputActions.Player.Sprint.ReadValue<float>() > 0f;
    }
    public bool IsJumping()
    {
        return playerInputActions.Player.Jump.triggered;
    }
    public Vector2 GetLookVector()
    {
        return playerInputActions.Player.Look.ReadValue<Vector2>();
    }
}
