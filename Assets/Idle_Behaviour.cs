using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle_Behaviour : StateMachineBehaviour
{
    public float minIdleTime = 1f;

    public float maxIdleTime = 5f;

    private float timeToIdle;

    private float timeSpentIdling;

    private Rigidbody2D rb;
    
    private Transform player;

    private GirlBoss gb;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gb = animator.GetComponent<GirlBoss>();
        timeSpentIdling = 0;
        timeToIdle = Random.Range(minIdleTime, maxIdleTime);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        if (gb.bossState == GB_State.Stage3)
        {
            animator.SetTrigger("Stage3Setup");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeSpentIdling += Time.deltaTime;
        if (timeSpentIdling >= timeToIdle)
        {
            switch (gb.bossState)
            {
                case GB_State.Stage1:
                    if (player.position.y <= rb.position.y)
                    {
                        animator.SetTrigger("RunToKick");
                    }
                    else
                    {
                        animator.SetTrigger("Walk");
                    }

                    break;
                case GB_State.Stage2:
                    if (player.position.y <= rb.position.y)
                    {
                        animator.SetTrigger("ShootGround");
                    }
                    else
                    {
                        animator.SetTrigger("RunToKick");
                    }
                    break;
                default:
                    break;
            }
            
            
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
