using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbLadder : StateMachineBehaviour
{
    private float climbHeight;

    [SerializeField] private float minimumHeight = -0.3f;

    [SerializeField] private float maximumHeight = 6.3f;

    [SerializeField] private float climbSpeed = 4f;
    
    private Rigidbody2D rb;
    private GirlBoss gb;

    private Vector2 target;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        gb = animator.GetComponent<GirlBoss>();
        climbHeight = Random.Range(minimumHeight, maximumHeight);
        //Debug.Log("Climb Height: " + climbHeight);
        target = new Vector2(rb.position.x, climbHeight);
        gb.isClimbing = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    
        
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, climbSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        //Debug.Log("RB: " + rb.transform.position.y);
       // Debug.Log("Target: " + target.y);
        if (rb.transform.localPosition.y >= target.y)
        {
            animator.SetTrigger("StopClimbing");
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
