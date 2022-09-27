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
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (isActiveOnStart)
        {
            isRunning = true;
        }
    }

    
    public void StartRunning()
    {
        isRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning && !isCowering)
        {
            float targetX = runningFrom.transform.position.x;
            
            if (targetX > transform.position.x)
            {
                spriteRenderer.flipX = true;
                Debug.Log("To the Right of me");
            }
            else if (targetX < transform.position.x)
            {
                Debug.Log("To the Left of me");
                spriteRenderer.flipX = false;
            } 
        }

        if (isCowering)
        {
            
        }
        
    }
}
