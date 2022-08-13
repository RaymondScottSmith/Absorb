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
            else if (!moving)
            {
                //Resets player when releasing mouse button after sticking
                readyToLaunch = true;
            }
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        
        //If we hit a bounceable object while holding down mouse and moving
        if (col.gameObject.CompareTag("Bounceable") && Input.GetMouseButton(0) && moving)
        {
            //Calculate reverse of current velocity
            Vector2 reverseVelocity = -rb.velocity;
            //Set current velocity to zero. Stopping the player
            rb.velocity = Vector2.zero;
            moving = false;
            /*
            //Use raycast to determine exactly where we impacted wall
            RaycastHit2D hit = Physics2D.Raycast(transform.position, reverseVelocity);
            if (hit.collider != null)
            {
                //Move the player back if we bounced a little away from the wall
                transform.position = hit.point;
            }*/

            transform.position = col.GetContact(0).point;
        }
    }

}
