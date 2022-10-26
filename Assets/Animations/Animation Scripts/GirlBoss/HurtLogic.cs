using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtLogic : StateMachineBehaviour
{
    private GirlBoss gb;

    private PlayerController pc;

    public AudioClip hurtSound;

    private AudioSource audioSource;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audioSource = animator.GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.PlayOneShot(hurtSound);
        pc = FindObjectOfType<PlayerController>();
        gb = animator.GetComponent<GirlBoss>();
        if (gb.isFacingPlayer)
            gb.SwitchFacingPlayer();
        
        if (gb.bossState != GB_State.Stage3)
            animator.SetBool("IsClimbing", !gb.isGrounded);
        
        if (gb.bossState == GB_State.Stage2)
        {
            if (gb.health == 1)
            {
                gb.ClearMines();
                animator.SetBool("Stage3SetupBool", true);
            }
        }
        gb.health--;
        if (gb.bossState == GB_State.Stage3 && gb.health <= 0)
        {
            animator.SetBool("Dead", true);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    //    
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (gb.health == 0)
        {
            if (gb.bossState == GB_State.Stage1)
                gb.bossState = GB_State.Stage2;
            else if (gb.bossState == GB_State.Stage3)
            {
                FindObjectOfType<CameraController>().FocusOnTarget(animator.gameObject);
                pc.HoldPlayerInPlace();
                gb.bossState = GB_State.None;
                animator.SetBool("Dead", true);
                gb.DeathCutscene();
                return;
            }
            else if (gb.bossState == GB_State.Stage2)
            {
                
                //animator.SetTrigger("Stage3Setup");
            }
                
            gb.health = gb.maxHealth;
        }
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
