using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isBusy { get; private set; }
    [Header("Attack details")]
    public Vector2[] attackMovement;
    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public int facingDir { get; private set; } = 1;
    public bool facingRight { get; private set; } = true;

    [Header("Dash info")]
    [SerializeField] public float dashCooldown = 1f;
    private float dashUsageTimer = 0;
    public float dashSpeed = 15f;
    public float dashDuration = 0.4f;
    public float dashDir { get; private set; }


    [Header("Collision info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.15f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallDistance = 0.15f;
    [SerializeField] private LayerMask whatIsWall;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttack primaryAttack { get; private set; }
    #endregion
    void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttack = new PlayerPrimaryAttack(this, stateMachine, "Attack");

    }
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.currentState.Update();
        CheckDashState();
    }
    
    public IEnumerator BusyFor(float _seconds){
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public void CheckDashState()
    {
        dashUsageTimer -= Time.deltaTime;
        if (IsWallDetected())
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }
    #region Velocity
    public void ZeroVelocity() => rb.velocity = new Vector2(0, 0);
    public void setVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    #region Collision
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundDistance, whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallDistance, whatIsGround);
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallDistance, wallCheck.position.y));
    }
    #endregion

    #region Flip
    public void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion
}
