using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Camera cam;
    private Animator anim;
    //[SerializeField] float diagonalDiv = 1.4f;

    private void Start()
    {
        if (anim)
            anim = GetComponent<Animator>();
    }

    Vector2 movement;
    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        //anim.SetFloat("x", movement.x);
        movement.y = Input.GetAxisRaw("Vertical");
        //anim.SetFloat("y", movement.y);
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    private void FixedUpdate()
    {
        //if (Math.Abs(movement.x) > 0.1 && Math.Abs(movement.y) > 0.1)
        //    rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime / diagonalDiv);
        //else
        //    rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);


        Vector2 force = movement.normalized * moveSpeed * Time.deltaTime;
        rb.AddForce(force);
        
    }
}
