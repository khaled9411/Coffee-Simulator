using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    CharacterController characterController;

    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float jumpHeight = 7f;
    [SerializeField] private float gravityValue = -10f;
    private Transform cameraTransform;
    private bool groundedPlayer;
    private Vector3 playerVelocity;

    private Vector3 moveDirection = Vector3.zero;

    private bool canMove = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cameraTransform = Camera.main.transform;
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

        groundedPlayer = characterController.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // get the input direciton 
        moveDirection =  new Vector3(InputManager.Instance.GetMovementVectorNormalized().x, 0, InputManager.Instance.GetMovementVectorNormalized().y);
        // this to move based on the camera direction 
        moveDirection = cameraTransform.forward *  moveDirection.z + cameraTransform.right * moveDirection.x;
        moveDirection.y = 0f;
        // check if run
        float currentSpeed = InputManager.Instance.IsRunning() ? runSpeed : walkSpeed;
        
        characterController.Move(moveDirection * Time.deltaTime * currentSpeed);

       

        if (InputManager.Instance.IsJumping() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    public void SetCanMove(bool can){canMove = can;}
    public bool CanMove() {return canMove;}

    public bool IsWalking()
    {
        return moveDirection != Vector3.zero;
    }
}
