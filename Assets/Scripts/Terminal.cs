using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    //protected Collider2D collider2D;
    protected Animator animator;
    protected AudioSource audioSource;
    [SerializeField] protected Triggerable triggerable;

    [SerializeField] protected List<int> keyValue;
    [SerializeField] protected AudioClip acceptSound;
    [SerializeField] protected AudioClip rejectSound;
    [SerializeField] protected string[] intitialMessages;
    [SerializeField] protected string[] afterMessages;

    [SerializeField] private bool deleteKey = false;

    protected bool active = true;
    // Start is called before the first frame update
    void Awake()
    {
        //collider2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && active)
        {
            foreach (int key in keyValue)
            {
                if (!other.GetComponent<PlayerShrink>().CheckForKey(key))
                    return;
            }
            audioSource.Stop();
            audioSource.PlayOneShot(acceptSound);
            if (deleteKey)
            {
                foreach (int key in keyValue)
                {
                    other.GetComponent<PlayerShrink>().DeleteKey(key);
                }
            }
            StartCoroutine(CorrectActivate());
            triggerable.Activate();
            active = false;
            
            TalkScript.Instance.ClearQueue();
            foreach (string message in afterMessages)
            {
                TalkScript.Instance.QueueLine(message);
            }
            TalkScript.Instance.DisplayMessages();
            
        }
    }

    private bool ContainsAllKeys(PlayerShrink player)
    {
        foreach (int key in keyValue)
        {
            if (!player.CheckForKey(key))
                return false;
        }

        return true;
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && active)
        {

            if (ContainsAllKeys(other.GetComponent<PlayerShrink>()))
            {
                return;
            }
            else
            {
                if (intitialMessages.Any())
                {
                    TalkScript.Instance.ClearQueue();
                    TalkScript.Instance.ClearText();
                    
                    foreach (string message in intitialMessages)
                    {
                        TalkScript.Instance.QueueLine(message);
                    }
                    TalkScript.Instance.DisplayMessages();
                    
                }
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
