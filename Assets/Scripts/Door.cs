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


    // Start is called before the first frame update
    void Start()
    {
        runnerCollider = GetComponent<Collider2D>();
        doorAnimator = GetComponent<Animator>();
        shouldBeOpen = false;
    }

    private void Update()
    {
        //doorAnimator.SetBool(Open, shouldBeOpen);
    }

    public void OpenDoor()
    {
        doorAnimator.SetBool(Open, true);
    }

    private void OnTriggerStay2D(Collider2D col)
    {

        if (col.CompareTag("Food"))
        {
            Debug.Log("Should be open from trigger");
            doorAnimator.SetBool(Open, true);
        }
            
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
            doorAnimator.SetBool(Open, false);
    }
}
