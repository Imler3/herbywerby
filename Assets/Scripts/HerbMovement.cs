using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HerbMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 5f;
    private bool facingRight = true;
    private float crouchTime;
    private bool disabled;
    //private bool isPounceCharged;

    private bool isTimerGoing;
    private Coroutine coUpdateTimer;
    private Coroutine gravTimer;

    //[SerializeField] private TextMeshProUGUI timer_Txt;
    [SerializeField] private float jumpingPower = 10f;
    [SerializeField] private float pounceChargeTime;
    [SerializeField] private float sectionCurrentTime;

    [SerializeField] private GameManager gm;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator anim;

    void Start()
    {
        isTimerGoing = false;
        disabled = false;
        //isPounceCharged = false;
        //timer_Txt.text = "00:00.0";
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded())
        {
            anim.SetBool("isInAir", false);
        }
        else
        {
            anim.SetBool("isInAir", true);
        }
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
            if (!disabled)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            }
        }

        if (Input.GetButtonDown("Jump") && !disabled)
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
        if (!disabled)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
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
        anim.SetBool("isUsingSpace", false);
        if (coUpdateTimer != null)
        {
            StopCoroutine(coUpdateTimer);
        }

        sectionCurrentTime = 0f;
        isTimerGoing = true;
        anim.SetBool("isUsingSpace",true);
        coUpdateTimer = StartCoroutine(UpdateTimer());
    }

    void EndTimer()
    {
        isTimerGoing = false;
        anim.SetBool("isUsingSpace",false);
        if (coUpdateTimer != null)
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
                anim.SetBool("isUsingSpace",false);
            }

            yield return null;
        }
    }

    private IEnumerator SetGravity()
    {
        rb.gravityScale = 1.4f;
        yield return new WaitForSeconds(1.5f);
        rb.gravityScale = 3f;
    }

    public void TimeToDie()
    {
        StartCoroutine(deathOccurrance());
    }

    IEnumerator deathOccurrance()
    {
        if(gm != null)
        {
            anim.SetBool("isDead", true);
            disabled = true;
            yield return new WaitForSeconds(1.5f);

            gm.Respawn(gameObject);
            anim.SetBool("isDead", false);
            yield return new WaitForSeconds(1f);
            GetComponent<HerbHealth>().ResetHealth();
            disabled = false;
            
        }
    }
}
