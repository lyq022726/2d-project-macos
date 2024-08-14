using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Space)){
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }
        if (yInput < 0)
        {
            player.setVelocity(0, rb.velocity.y);
        }
        else
        {
            player.setVelocity(0, rb.velocity.y * 0.7f);
        }
        if (xInput != 0 && player.facingDir != xInput)
        {
            stateMachine.ChangeState(player.airState);
        }
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
