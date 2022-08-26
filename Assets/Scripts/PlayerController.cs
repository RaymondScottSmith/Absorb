using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 1;

    [SerializeField]
    private Rigidbody2D rb;
    private bool moving;
    private bool touchingBounceable;
    private PlayerShrink playerShrink;
    public bool readyToLaunch;

    public bool readyToPlay;
    public PhysicsMaterial2D bouncyMaterial;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bounceSound;
    [SerializeField] private AudioClip zapSound;

    [SerializeField] private bool isTutorial;

    [SerializeField] private GameObject pausePanel;

    private bool autoStick;

    private Animator animator;

    private bool grabbing;

    public bool isOverUI;

    private RaycastReflection raycastReflection;

    // Start is called before the first frame update
    void Awake()
    {
        moving = false;
        readyToLaunch = true;
        readyToPlay = false;
        rb = GetComponent<Rigidbody2D>();
        playerShrink = GetComponent<PlayerShrink>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        raycastReflection = GetComponent<RaycastReflection>();
        if (isTutorial)
        {
            autoStick = false;
        } 
        else if (PlayerPrefs.HasKey("AutoStick"))
        {
            autoStick = PlayerPrefs.GetInt("AutoStick") == 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToPlay)
        {
            raycastReflection.isOverUI = isOverUI;
            if ((pausePanel != null && pausePanel.activeSelf || isOverUI))
            {
                return;
            }
        
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

            if (autoStick && moving)
            {
                grabbing = true;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && moving)
            {
                grabbing = true;
            }
        }
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (moving)
        {
            if (!col.gameObject.CompareTag("Food") && !col.gameObject.CompareTag("Damaging"))
            {
                audioSource.Stop();
                audioSource.PlayOneShot(bounceSound);
            }
            Vector2 v = rb.velocity;
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
        }
        
        
        //If we hit a bounceable object while holding down mouse and moving
        if (col.gameObject.CompareTag("Bounceable") && grabbing)
        {
            Debug.Log(col.gameObject.tag);
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
            TakeDamage(10,zapSound);
        }
    }

    public void TakeDamage(int damage, [CanBeNull] AudioClip damageSound)
    {
        animator.SetTrigger("Shock");
        if (damageSound != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(zapSound);
        }
        playerShrink.TakeDamage(damage);
        StartCoroutine(Fall(5f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ExitPoint"))
        {
            readyToPlay = false;
            rb.velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
            //Debug.Log("Hitting exit");
            TimelineManager.Instance.FoundExit();
        }
    }

    public IEnumerator Fall(float fallSpeed)
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = fallSpeed;
        grabbing = true;
        moving = true;

        while (grabbing)
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        
        rb.gravityScale = 0f;
    }

}
