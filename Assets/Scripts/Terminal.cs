using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    protected Collider2D collider2D;
    protected Animator animator;
    [SerializeField] protected Triggerable triggerable;

    [SerializeField] protected int keyValue;

    protected bool active = true;
    // Start is called before the first frame update
    void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && active)
        {
            if (other.GetComponent<PlayerShrink>().CheckForKey(keyValue))
            {
                StartCoroutine(CorrectActivate());
                triggerable.Activate();
                active = false;
            }
            else
            {
                animator.SetTrigger("Incorrect");
            }
        }
    }

    protected virtual IEnumerator CorrectActivate()
    {
        animator.SetTrigger("Correct");
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Activated", true);
    }
}
