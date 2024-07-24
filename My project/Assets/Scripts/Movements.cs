using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Movements: MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private bool IsGrounded=false;
	private bool BouncePad = false;
    private bool isMoving;
    private bool isRunning;
    
    
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float rotationFactorPerFrame = 1.0f;

    private Vector2 direction = Vector2.zero;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        
    }
    
    void OnMove(InputValue value)
    {
        Vector2 direction = value.Get<Vector2>();
        if (direction != Vector2.zero)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        this.direction = direction;

    }

    void Update()
    {   
        handleRotation();
        handleAnimation();
        Move(direction.x, direction.y);
        
    }

    private void Move(float x, float z)
    {
        rb.velocity = new Vector3(x * speed, rb.velocity.y, z* speed);
        
    }

    private void OnRun()
    {
        if (Input.GetKeyDown("Left Shift"))
        {
            isRunning = true;
        }
        
    }

    void OnJump()
    {
        if (IsGrounded)
        {
             if (BouncePad)
             {
                 rb.AddForce(0,jumpHeight*1.5F,0,ForceMode.VelocityChange);
                 
             }
                Jump();
                
            
        }

        
    }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
        
    }
    
    void handleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = rb.velocity.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = rb.velocity.z;

        Quaternion currentRotation =transform.rotation;
        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation= Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame*Time.deltaTime);
        }
        
    }
    
    void handleAnimation()
    {
        bool isWalking = animator.GetBool("isWalking");
        //bool isRunning = animator.GetBool("isRunning");

        if (isMoving && !isWalking)
        {
            animator.SetBool("isWalking", true);
        }
        else if (!isMoving && isWalking)
        {
            animator.SetBool("isWalking", false);
        }
    }

   
     void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
      {
         case "JumpPad":
            BouncePad = true;
           break;
      }
     
    }
    
    void OnCollisionExit(Collision collision)
    {
        IsGrounded = false;
         if (collision.gameObject.CompareTag("JumpPad"))
         {
             BouncePad = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (Vector3.Angle(collision.GetContact(0).normal, Vector3.up) < 45f)
        {
            IsGrounded = true;
            //Debug.Log(IsGrounded);
			
            
        }
        else
        {
            IsGrounded = false;
        }
    }
}