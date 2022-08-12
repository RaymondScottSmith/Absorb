using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 1;

    private Rigidbody2D rb;
    private bool moving;
    private bool touchingBounceable;
    private bool readyToLaunch;
    public PhysicsMaterial2D bouncyMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moving = false;
        readyToLaunch = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) )
        {
            if (readyToLaunch)
            {
                readyToLaunch = false;
                moving = true;
                var mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
                var mouseDir = mousePos - gameObject.transform.position;
                mouseDir.z = 0.0f;
                mouseDir.Normalize();
                rb.AddForce(mouseDir * baseSpeed, ForceMode2D.Impulse);
            }
            else if (!moving)
            {
                readyToLaunch = true;
            }
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Bounceable") && Input.GetMouseButton(0) && moving)
        {
            Vector2 reverseVelocity = -rb.velocity;
            rb.velocity = Vector2.zero;
            moving = false;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, reverseVelocity);
            if (hit.collider != null)
            {
                transform.position = hit.point;
            }
        }
    }

}
