using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour

{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private float moveSpeed = 4;
    [SerializeField] private float jumpSpeed = 5;
    public int facingDir = 1;
    public bool facingRight = true;
    private float xInput;
    [Header("Dashing info")]
    [SerializeField] private float dashSpeed = 10;
    [SerializeField] private float dashDuration = 0.5f;
    private float dashTimer;
    private bool isDashing;
    private bool canDash = true;
    [SerializeField] private float dashCD = 2;
    [SerializeField] private float dashCDTimer;

    [Header("Attack Info")]
    [SerializeField] private float comboTimer;
    [SerializeField] private float comboWindow = 0.5f;
    private bool isAttacking;
    private int comboCounter = 0;


    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    void Start()
    {
        Debug.Log("Statr");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        CheckInput();
        TimerController();
        CollsionChecks();

        FilpController();
        AnimatorControllers();
    }

    private void TimerController()
    {
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }
        else
        {
            isDashing = false;
        }
        if (dashCDTimer > 0)
        {
            dashCDTimer -= Time.deltaTime;
        }
        else
        {
            canDash = true;
        }
        comboTimer -= Time.deltaTime;
    }

    private void CollsionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        // if (!(isDashing && !isGrounded))
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
            Jump();
        if (Input.GetKeyDown(KeyCode.LeftShift))
            DashAbility();
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartAttackEvent();
        }
    }

    private void StartAttackEvent()
    {
        if (comboTimer <= 0)
            comboCounter = 0;
        isAttacking = true;
        comboTimer = comboWindow;
    }

    public void AttackOver()
    {
        isAttacking = false;
        comboCounter++;
        if (comboCounter > 2)
        {
            comboCounter = 0;
        }
    }
    private void DashAbility()
    {
        if (canDash && !isAttacking)
        {
            dashTimer = dashDuration;
            dashCDTimer = dashCD;
            isDashing = true;
            canDash = false;
        }
    }

    private void Movement()
    {
        if (isAttacking)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else if (isDashing)
        {
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if (isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }
    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x != 0;
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isDashing", isDashing);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);
    }
    private void Filp()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    private void FilpController()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Filp();
        else if (rb.velocity.x < 0 && facingRight)
            Filp();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}

