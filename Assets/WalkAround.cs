using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WalkAround : StateMachineBehaviour
{
    
    public float speed = 2.5f;

    private Transform player;

    private Rigidbody2D rb;

    private CameraDrone cd;

    private GirlBoss gb;

    public float attackDistance = 2.5f;
    public float gravity = -9.81f;
    public float gravityScale = 1;
    public float jumpForce = 5;
    private float velocity;

    private Vector2 target;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        cd = animator.GetComponentInChildren<CameraDrone>();
        gb = animator.GetComponent<GirlBoss>();
        target = gb.GetRandomLadder().position;
        gb.LookAtTarget(target);
        target = new Vector2(target.x, rb.position.y);
        Debug.Log("Target's X: " + target.x);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //gb.LookAtPlayer();
        
        
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        /*
        if (Vector3.Distance(target, rb.gameObject.transform.position) < attackDistance)
        {
            //rb.velocity = Vector3.zero;
            animator.SetTrigger("KickAttack");
        }
        */

        if (Math.Abs(target.x - rb.transform.position.x) < 0.1f)
        {
            animator.SetTrigger("EndWalk");
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
