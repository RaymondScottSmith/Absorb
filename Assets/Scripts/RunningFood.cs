using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class RunningFood : MonoBehaviour
{

    [SerializeField] private bool isActiveOnStart = false;

    [SerializeField] private GameObject runningFrom;

    private bool isRunning;

    private bool isCowering;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private float speed = 1f;

    private bool isRunningRight;

    private Vector3 rayDirection = Vector3.right;

    [SerializeField] private Transform rayLocation;

    private Shrink shrink;
    // Start is called before the first frame update
    void Start()
    {
        shrink = GetComponent<Shrink>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (isActiveOnStart)
        {
            StartRunning();
        }
    }

    public void StopRunning()
    {
        isRunning = false;
    }
    public void StartRunning()
    {
        float targetX = runningFrom.transform.position.x;
        isRunning = true;
        if (targetX > transform.position.x)
        {
            isRunningRight = false;
            rayDirection = Vector3.left;
        }
        else
        {
            isRunningRight = true;
            rayDirection = Vector3.right;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shrink != null && shrink.beingEaten)
        {
            return;
        }
        int layerMask = ~(LayerMask.GetMask("Player") + LayerMask.GetMask("CrewColliders"));
        RaycastHit2D hit = Physics2D.Raycast(rayLocation.position, rayDirection, 2f, layerMask);

        if (hit.collider != null)
        {
            SwitchDirection();
            Debug.Log("Hit: " + hit.collider.name);
        }
            
        
        
        if (isRunning && !isCowering)
        {
            animator.SetBool("Running", true);
            transform.position = transform.position + (rayDirection * speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Running", false);
        }

        if (isCowering)
        {
            Debug.Log("Should be cowering");
        }
        
    }

    private void SwitchDirection()
    {
        if (isRunningRight)
        {
            isRunningRight = false;
            spriteRenderer.flipX = true;
            rayDirection = Vector3.left;
        }
        else
        {
            isRunningRight = true;
            spriteRenderer.flipX = false;
            rayDirection = Vector3.right;
        }
    }
}
