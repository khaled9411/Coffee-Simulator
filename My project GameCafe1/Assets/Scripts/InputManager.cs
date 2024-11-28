using MFPC.Example;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerInputAction playerInputActions;

    public event Action OnInteract;
    public event Action OnInteractWithPicked;

    [SerializeField] LookField lookField;

    private bool pressed = false;
    private int lookFingerIndex = -1;
    private Vector2 touchDelta;
    private Vector2 previousTouchPos;
    float deadZone = 0.1f;

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputAction();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractWithPicked.performed += InteractWithPicked_performed; ;

        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += HandleFingerDown;
        Touch.onFingerMove += HandleFingerMove;
        Touch.onFingerUp += HandleFingerUp;
    }

    private void InteractWithPicked_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractWithPicked?.Invoke();
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke();
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
        if (Mathf.Abs(touchDelta.x) < deadZone && Mathf.Abs(touchDelta.y) < deadZone)
        {
            return Vector2.zero;
        }

        return touchDelta;
    }

    private void HandleFingerDown(Finger finger)
    {
        Vector2 touchPosition = finger.currentTouch.screenPosition;
        if (IsLookField(touchPosition))
        {
            pressed = true;
            lookFingerIndex = finger.index;
            previousTouchPos = touchPosition;
        }
    }

    private void HandleFingerMove(Finger finger)
    {
        if (finger.index != lookFingerIndex || !pressed) return;

        touchDelta = finger.currentTouch.screenPosition - previousTouchPos;
        previousTouchPos = finger.currentTouch.screenPosition;
    }

    private void HandleFingerUp(Finger finger)
    {
        if (finger.index != lookFingerIndex || !pressed) return;

        pressed = false;
        lookFingerIndex = -1;
        touchDelta = Vector2.zero;
    }

    private bool IsLookField(Vector2 screenPosition)
    {
        // Check UI elements first
        if (EventSystem.current != null && PlayerMovement.Instance.CanMove())
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = screenPosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            return results[0].gameObject.TryGetComponent<LookField>(out _);
        }
        return false;
    }

    private void OnDisable()
    {
        if (EnhancedTouchSupport.enabled)
        {
            Touch.onFingerDown -= HandleFingerDown;
            Touch.onFingerMove -= HandleFingerMove;
            Touch.onFingerUp -= HandleFingerUp;
            EnhancedTouchSupport.Disable();
        }
    }
}
