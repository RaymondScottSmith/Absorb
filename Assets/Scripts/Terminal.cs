using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    protected Collider2D collider2D;
    protected Animator animator;
    protected AudioSource audioSource;
    [SerializeField] protected Triggerable triggerable;

    [SerializeField] protected int keyValue;
    [SerializeField] protected AudioClip acceptSound;
    [SerializeField] protected AudioClip rejectSound;

    protected bool active = true;
    // Start is called before the first frame update
    void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && active)
        {
            if (other.GetComponent<PlayerShrink>().CheckForKey(keyValue))
            {
                audioSource.Stop();
                audioSource.PlayOneShot(acceptSound);
                StartCoroutine(CorrectActivate());
                triggerable.Activate();
                active = false;
            }
            else
            {
                audioSource.Stop();
                audioSource.PlayOneShot(rejectSound);
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
