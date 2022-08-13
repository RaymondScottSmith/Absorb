using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 1;

    [SerializeField]
    private Rigidbody2D rb;
    private bool moving;
    private bool touchingBounceable;
    public bool readyToLaunch;
    public PhysicsMaterial2D bouncyMaterial;

    private bool grabbing;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        readyToLaunch = true;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //When mouse button released
        if (Input.GetMouseButtonUp(0) )
        {
            //Check readyToLaunch to make sure it doesn't automatically launch when we release click after stopping
            if (readyToLaunch)
            {
                grabbing = false;
                readyToLaunch = false;
                moving = true;
                //Get Mouse position
                var mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
                //Calculate direction to mouse from gameobject
                var mouseDir = mousePos - gameObject.transform.position;
                mouseDir.z = 0.0f;
                //Normalize the direction to make sure distance does not affect speed
                mouseDir.Normalize();
                //Start the player moving
                rb.AddForce(mouseDir * baseSpeed, ForceMode2D.Impulse);
            }
            /*
            else if (!moving)
            {
                //Resets player when releasing mouse button after sticking
                readyToLaunch = true;
            }
            */
        }
        if (Input.GetKeyDown(KeyCode.Space) && moving)
        {
            grabbing = true;
            //Debug.Log("Grabbing should be true");
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        
        //If we hit a bounceable object while holding down mouse and moving
        if (col.gameObject.CompareTag("Bounceable") && grabbing)
        {
            grabbing = false;
            //Calculate reverse of current velocity
            Vector2 reverseVelocity = -rb.velocity;
            //Set current velocity to zero. Stopping the player
            rb.velocity = Vector2.zero;
            moving = false;
            readyToLaunch = true;
            transform.position = col.GetContact(0).point;
        }
    }

}
