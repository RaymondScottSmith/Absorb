using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFromLadder : StateMachineBehaviour
{
    private Rigidbody2D rb;
    private GirlBoss gb;

    private AudioSource audioSource;

    public AudioClip landSound;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gb = animator.GetComponent<GirlBoss>();
        rb = animator.GetComponent<Rigidbody2D>();
        audioSource = animator.GetComponent<AudioSource>();
        gb.isClimbing = false;
        gb.JumpForward();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (gb.isGrounded)
        {
            animator.SetTrigger("Land");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            audioSource.PlayOneShot(landSound);
    }

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
