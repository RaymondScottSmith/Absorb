using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Collider2D runnerCollider;

    private Animator doorAnimator;

    private static readonly int OpenDoor1 = Animator.StringToHash("OpenDoor");

    public bool shouldBeOpen;
    private static readonly int Open = Animator.StringToHash("Open");

    private AudioSource audioSource;

    [SerializeField] private AudioClip openDoorSound;

    [SerializeField] private AudioClip closeDoorSound;


    // Start is called before the first frame update
    void Start()
    {
        runnerCollider = GetComponent<Collider2D>();
        doorAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        shouldBeOpen = false;
    }
    

    public void OpenDoor()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(openDoorSound);
        doorAnimator.SetBool(Open, true);
    }

    private void OnTriggerStay2D(Collider2D col)
    {

        if (col.CompareTag("Food"))
        {

            OpenDoor();
        }
            
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            doorAnimator.SetBool(Open, false);
            audioSource.Stop();
            audioSource.PlayOneShot(closeDoorSound);
        }
            
    }
}
