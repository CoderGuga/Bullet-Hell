using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f,
    dashSpeed,
    dashTimer,
    knockbackSpeed;
    public bool speedPerHp;

    float timer;

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
        timer += Time.deltaTime;
        movement.x = Input.GetAxisRaw("Horizontal");
        //anim.SetFloat("x", movement.x);
        movement.y = Input.GetAxisRaw("Vertical");
        //anim.SetFloat("y", movement.y);
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, -10);

        if (Input.GetButtonDown("Fire2") && timer > dashTimer)
            Dash();
    }

    private void FixedUpdate()
    {
        //if (Math.Abs(movement.x) > 0.1 && Math.Abs(movement.y) > 0.1)
        //    rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime / diagonalDiv);
        //else
        //    rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);


        Vector2 force = movement.normalized * moveSpeed * Time.deltaTime;
        if (force.magnitude > 0.1)
            GetComponent<Weapon>().Destabilize();
        if (speedPerHp)
            force *= 1 + 0.1f * (GetComponent<PlayerHealth>().playerMaxHealth - GetComponent<PlayerHealth>().playerCurrentHealth);
        rb.AddForce(force);

    }




    void Dash()
    {
        timer = 0;
        Vector2 force = movement.normalized * dashSpeed;
        rb.AddForce(force);
        gameObject.layer = LayerMask.NameToLayer("Invincible Player");
        transform.Find("Particles").Find("Dash").GetComponent<ParticleSystem>().Play();

        StartCoroutine(StopDash(0.5f));
    }

    IEnumerator StopDash(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.layer = LayerMask.NameToLayer("Player");
        rb.velocity = Vector2.zero;
    }
    
    public void Knockback()
    {
        timer = 0;
        Vector2 force = (transform.position - transform.Find("Firepoint").position) * knockbackSpeed;
        rb.AddForce(force);
        
        StartCoroutine(StopKnockback(0.1f));
    }

    IEnumerator StopKnockback(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector2.zero;
    }
}
