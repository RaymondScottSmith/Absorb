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
    private PlayerShrink playerShrink;
    public bool readyToLaunch;
    public PhysicsMaterial2D bouncyMaterial;

    private bool grabbing;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        readyToLaunch = true;
        rb = GetComponent<Rigidbody2D>();
        playerShrink = GetComponent<PlayerShrink>();
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
                //Make the player face the direction initially launched
                Vector2 v = rb.velocity;
                float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && moving)
        {
            grabbing = true;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //transform.LookAt(Vector2.Reflect(rb.velocity, col.GetContact(0).normal));
        //transform.Rotate(Vector3.forward, 90);

        if (moving)
        {
            Vector2 v = rb.velocity;
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
        }
        
        
        //If we hit a bounceable object while holding down mouse and moving
        if (col.gameObject.CompareTag("Bounceable") && grabbing)
        {
            grabbing = false;

            transform.up = col.GetContact(0).normal;
            //Calculate reverse of current velocity
            Vector2 reverseVelocity = -rb.velocity;
            //Set current velocity to zero. Stopping the player
            rb.velocity = Vector2.zero;
            moving = false;
            readyToLaunch = true;
            transform.position = col.GetContact(0).point;
        }

        if (col.gameObject.CompareTag("Damaging"))
        {
            playerShrink.TakeDamage(10);
        }
    }

}
