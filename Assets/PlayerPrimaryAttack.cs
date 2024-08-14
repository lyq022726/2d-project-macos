using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerPrimaryAttack : PlayerState
{
    private int comboCounter;
    private float lastTimeAttack;
    private float comboWindow = 2f;
    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter(){
        base.Enter();
        if(comboCounter > 2 || Time.time >= lastTimeAttack + comboWindow){
            comboCounter = 0;
        }
        player.setVelocity(player.attackMovement[comboCounter].x * player.facingDir, player.attackMovement[comboCounter].y);
        stateTimer = 0.1f;
        player.anim.SetInteger("ComboCounter", comboCounter);
    }
    public override void Exit(){
        base.Exit();
        player.StartCoroutine("BusyFor", 0.15f);
        comboCounter++;
        lastTimeAttack = Time.time;
    }
    public override void Update(){
        base.Update();
        if(stateTimer < 0){
            player.ZeroVelocity();
        }
        if(triggerCalled){
            stateMachine.ChangeState(player.idleState);
        }
    }
    
}
