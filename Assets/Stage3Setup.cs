using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3Setup : StateMachineBehaviour
{
    private GirlBoss gb;

    private Vector3 startPos;

    private Rigidbody2D rb;

    public float speed = 3f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gb = animator.GetComponent<GirlBoss>();
        rb = animator.GetComponent<Rigidbody2D>();
        startPos = new Vector3(gb.ladders[2].position.x, gb.transform.position.y,0);
        gb.LookAtTarget(startPos);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, startPos, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        if (Mathf.Abs(startPos.x - rb.position.x) <= 0.5f)
        {
            gb.PlayStage3Intro();
            animator.SetTrigger("Stage3Setup");
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
