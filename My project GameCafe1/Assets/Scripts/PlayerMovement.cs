using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float jumpPower = 7f;
    [SerializeField] private float gravity = 10f;

    Vector3 moveDirection = Vector3.zero;

    private bool canMove = true;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        HandleMove();
    }
    void HandleMove()
    {
        if (!canMove) { return; }

        moveDirection =  new Vector3(InputManager.Instance.GetMovementVectorNormalized().x, 0, InputManager.Instance.GetMovementVectorNormalized().y);
        float currentSpeed = InputManager.Instance.IsRunning() ? runSpeed : walkSpeed;
        
        characterController.Move(moveDirection * Time.deltaTime * currentSpeed);
    }
}
