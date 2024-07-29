using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    private PlayerInput playerInput; 
    private CharacterController characterController;
    private Animator animator;
    
    private int isWalkingHash;
    private int isRunningHash;
    private int isJumpingHash;
    private float groundedGravity = -0.5f;
    private float  gravity = -9.8f; 
    private Vector2 currentmovementInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;
    private Vector3 cameraRelativeMovement;
    private bool isMovementPressed;
    private bool isRunPressed;
    
    private bool isJumpPressed =false;
    private bool isJumping = false;
    private bool isJumpAnimating = false;
    private float initialJumpVelocity;
    private float maxJumpHeight = 4.0f;
    private float maxJumpTime = 1.0f;
    private float runMultiplier = 5.0f;
    private float bouncePadMultiplier = 1.2f;
    
    private bool BouncePad = false;

    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");

        playerInput.Player.Move.started += OnMovementInput;
        playerInput.Player.Move.canceled += OnMovementInput;
        playerInput.Player.Move.performed += OnMovementInput;

        playerInput.Player.Dash.started += OnDash;
        playerInput.Player.Dash.canceled += OnDash;
        playerInput.Player.Jump.started += OnJump;
        playerInput.Player.Jump.canceled += OnJump;

        setUpJumpVariables();
    }

    void setUpJumpVariables()
    {
        
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void handleJump()
    {
        
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            if (BouncePad)
            {
                currentMovement.y = initialJumpVelocity * bouncePadMultiplier;
                currentRunMovement.y = initialJumpVelocity * bouncePadMultiplier;
            }
            else
            {
                currentMovement.y = initialJumpVelocity * 0.5f;
                currentRunMovement.y = initialJumpVelocity * 0.5f;
            }
            
        }else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
        
    }
    void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void OnMovementInput(InputAction.CallbackContext context)
    { 
        currentmovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentmovementInput.x *3.5f;
        currentMovement.z = currentmovementInput.y *3.5f;
        currentRunMovement.x = currentmovementInput.x * 3.5f * runMultiplier;
        currentRunMovement.z = currentmovementInput.y * 3.5f * runMultiplier;
        isMovementPressed = currentmovementInput.x != 0 || currentmovementInput.y != 0;
    }

    void OnDash(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
        
    }

  

    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
        
    }

    void handleRotation()
    {
        float rotationFactorPerFrame;
        Vector3 positionToLookAt;
        positionToLookAt.x = -cameraRelativeMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = -cameraRelativeMovement.z ;

        Quaternion currentRotation =transform.rotation;
        if (isMovementPressed)
        {
            rotationFactorPerFrame = 1.0f;
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation= Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame*Time.deltaTime);
        }
    }

    void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0.00f;
        float fallMultiplier = 2.0f;
        if (characterController.isGrounded)
        {
            if (isJumpAnimating)
            {
                 animator.SetBool(isJumpingHash, false);
                 isJumpAnimating = false;
            }
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
            
        }else if (isFalling)
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleRotation();
        handleAnimation();
        if (isRunPressed)
        {
            cameraRelativeMovement = ConvertToCameraSpace(currentRunMovement);
            characterController.Move(cameraRelativeMovement*Time.deltaTime);
        }
        else
        {
            cameraRelativeMovement = ConvertToCameraSpace(currentMovement);
            characterController.Move(cameraRelativeMovement * Time.deltaTime);
        }
        handleGravity();
        handleJump();
    }

    Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        float currentYValue = vectorToRotate.y;
        
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 cameraForwardZProduct = vectorToRotate.z * cameraForward;
        Vector3 cameraForwardXProduct = vectorToRotate.x * cameraRight;

        Vector3 vectorRotateToCameraSpace = cameraForwardZProduct + cameraForwardXProduct;
        vectorRotateToCameraSpace.y = currentYValue;
        return vectorRotateToCameraSpace;
    }

    void OnEnable()
    {
        playerInput.Player.Enable();
    }
    void OnDisable()
    {
        playerInput.Player.Disable();
    }
    
    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "JumpPad":
                BouncePad = true;
                Debug.Log(BouncePad);
                break;
        }
     
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("JumpPad"))
        {
            BouncePad = false;
            Debug.Log(BouncePad);
        }
    }

    
}
