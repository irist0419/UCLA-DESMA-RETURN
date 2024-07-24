using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    private PlayerInput playerInput; 
    private CharacterController characterController;
    private Animator animator;
    
    private Vector2 currentmovementInput;
    private Vector3 currentMovement;
    private bool isMovementPressed;
    private float rotationFactorPerFrame = 1.0f;
    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
        playerInput.Player.Move.started += OnMovementInput;
        playerInput.Player.Move.canceled += OnMovementInput;
        playerInput.Player.Move.performed += OnMovementInput;
        
    }

    void OnMovementInput(InputAction.CallbackContext context)
    { currentmovementInput = context.ReadValue<Vector2>();
        currentMovement.x = -currentmovementInput.x *2.8f;
        currentMovement.z = -currentmovementInput.y *2.8f;
        isMovementPressed = currentmovementInput.x != 0 || currentmovementInput.y != 0;
    }

    void handleAnimation()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");

        if (isMovementPressed && !isWalking)
        {
            animator.SetBool("isWalking", true);
        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool("isWalking", false);
        }
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = -currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = -currentMovement.z ;

        Quaternion currentRotation =transform.rotation;
        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation= Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame*Time.deltaTime);
        }
        
    }

    void handleGravity()
    {
        if (characterController.isGrounded)
        {
            float groundedGravity = -0.5f;
            currentMovement.y = groundedGravity;
            
        }
        else
        {
            float gravity = -9.8f;
            currentMovement.y += gravity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleGravity();
        handleRotation();
        handleAnimation();
        characterController.Move(currentMovement*Time.deltaTime);
    }

    void OnEnable()
    {
        playerInput.Player.Enable();
    }
    void OnDisable()
    {
        playerInput.Player.Disable();
    }
}
