using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HerbMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 7f;
    private bool facingRight = true;
    private float crouchTime;
    //private bool isPounceCharged;

    private bool isTimerGoing;
    private Coroutine coUpdateTimer;
    private Coroutine gravTimer;

    //[SerializeField] private TextMeshProUGUI timer_Txt;
    [SerializeField] private float jumpingPower = 6f;
    [SerializeField] private float pounceChargeTime;
    [SerializeField] private float sectionCurrentTime;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator anim;

    void Start()
    {
        isTimerGoing = false;
        //isPounceCharged = false;
        //timer_Txt.text = "00:00.0";
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("MoveHorizontal", horizontal);
        if(horizontal != 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {            
            //rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y == 0f)
        {
            //Debug.Log("Time: "+timer_Txt.text);
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonDown("Jump"))
        {
            BeginTimer();
            
        }
        else if (Input.GetButtonUp("Jump"))
        {
            EndTimer();
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if ((facingRight && horizontal < 0f) || (!facingRight && horizontal > 0f))
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void BeginTimer()
    {
        isTimerGoing = false;
        if (coUpdateTimer != null)
        {
            StopCoroutine(coUpdateTimer);
        }

        sectionCurrentTime = 0f;
        isTimerGoing = true;
        coUpdateTimer = StartCoroutine(UpdateTimer());
    }

    void EndTimer()
    {
        isTimerGoing = false;
        if(coUpdateTimer != null)
        {
            StopCoroutine(coUpdateTimer);
        }
        //isPounceCharged = false;
        sectionCurrentTime = 0f;
    }

    private IEnumerator UpdateTimer()
    {
        while (isTimerGoing && IsGrounded())
        {
            sectionCurrentTime += Time.deltaTime;
            if(sectionCurrentTime >= pounceChargeTime) 
            {
                //isPounceCharged = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                gravTimer = StartCoroutine(SetGravity());
                sectionCurrentTime = 0f;
            }

            yield return null;
        }
    }

    private IEnumerator SetGravity()
    {
        rb.gravityScale = 1.75f;
        yield return new WaitForSeconds(1.5f);
        rb.gravityScale = 3.0f;
    }
}
