using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WalkAround : StateMachineBehaviour
{
    
    public float speed = 2.5f;

    private GameObject player;

    private Rigidbody2D rb;

    private CameraDrone cd;

    private GirlBoss gb;

    public float attackDistance = 2.5f;
    public float gravity = -9.81f;
    public float gravityScale = 1;
    public float jumpForce = 5;
    private float velocity;
    private bool isWalkingRight;

    private GB_Spawner mineSpawner;

    private Vector2 target;
    private AudioSource audioSource;

    public AudioClip walkSound;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        rb = animator.GetComponent<Rigidbody2D>();
        cd = animator.GetComponentInChildren<CameraDrone>();
        gb = animator.GetComponent<GirlBoss>();
        mineSpawner = FindObjectOfType<GB_Spawner>();

        if (gb.bossState != GB_State.None)
        {
            audioSource = animator.GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.clip = walkSound;
            audioSource.Play();
        }
        if (gb.bossState == GB_State.Stage3)
        {
            gb.GetComponent<SpriteRenderer>().flipX = false;
            gb.UpdateCamera();
            Debug.Log("Starting stage 3 walk");
            isWalkingRight = true;
            target = gb.ladders[3].transform.position;
            target = new Vector2(target.x, rb.position.y);
            gb.LookAtTarget(target);
        }
        else
        {
            target = gb.GetRandomLadder().position;
            gb.LookAtTarget(target);
            target = new Vector2(target.x, rb.position.y);
            Debug.Log("Target's X: " + target.x);
        }
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //gb.LookAtPlayer();

        target = new Vector2(target.x, rb.position.y);
        if (rb.position.x < target.x)
        {
            isWalkingRight = true;
        }
        else
        {
            isWalkingRight = false;
        }
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        
        if (Vector3.Distance(player.transform.position, rb.gameObject.transform.position) < attackDistance)
        {
            if (isWalkingRight && player.transform.position.x > rb.position.x)
            {
                Debug.Log("Right, playerX: " + player.transform.position.x + ", BossX: " + rb.position.x);
                animator.SetTrigger("KickAttack");
            }
            else if (!isWalkingRight && player.transform.position.x < rb.position.x)
            {
                Debug.Log("Left, playerX: " + player.transform.position.x + ", BossX: " + rb.position.x);
                animator.SetTrigger("KickAttack");
            }
            //rb.velocity = Vector3.zero;
            
        }
        
        
        if (Math.Abs(target.x - rb.transform.position.x) < 0.1f)
        {
            if (gb.bossState != GB_State.Stage3)
            {
                animator.SetTrigger("EndWalk");
            }
            else
            {
                if (isWalkingRight)
                {
                    target = gb.ladders[0].transform.position;
                    
                }
                else
                {
                    target = gb.ladders[3].transform.position;
                }
                target = new Vector2(target.x, rb.position.y);
                isWalkingRight = !isWalkingRight;
                gb.LookAtTarget(target);
            }

        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audioSource.loop = false;
        audioSource.Stop();
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
