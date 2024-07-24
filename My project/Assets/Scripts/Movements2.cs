using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Movements2: MonoBehaviour
{
    private Rigidbody rb;
    private bool IsGrounded=false;
	private bool BouncePad = false;
    
    
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 5f;
    private Vector2 direction = Vector2.zero;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        
    }
    
    void OnMove(InputValue value)
    {
        Vector2 direction = value.Get<Vector2>();
        this.direction = direction;

    }

    void Update()
    {   
      
        Move(direction.x, direction.y);
        
    }

    private void Move(float x, float z)
    {
        rb.velocity = new Vector3(x * speed, rb.velocity.y, z* speed);
        
    }

    void OnDash()
    {
        if (IsGrounded)
        {
            rb.AddForce(rb.velocity.x *1.5f, 0, rb.velocity.z *1.5f,ForceMode.VelocityChange);
            Dash();
                
            
        }
        
    }

    private void Dash()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
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
            
        }
        else
        {
            IsGrounded = false;
        }
    }
}