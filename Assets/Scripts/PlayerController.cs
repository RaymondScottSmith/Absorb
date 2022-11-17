using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    public float baseSpeed = 1;

    [SerializeField]
    private Rigidbody2D rb;
    public bool moving;
    private bool touchingBounceable;
    private PlayerShrink playerShrink;
    public bool readyToLaunch;

    public bool readyToPlay;
    public PhysicsMaterial2D bouncyMaterial;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip squishSound;
    [SerializeField] private AudioClip bounceSound;
    [SerializeField] private AudioClip zapSound;

    [SerializeField] private bool isTutorial;

    [SerializeField] private GameObject pausePanel;

    private bool autoStick;

    private Animator animator;

    private bool grabbing;

    public bool isOverUI;

    private RaycastReflection raycastReflection;

    [SerializeField] private int damageFromHazards = 10;

    private GameObject grabbedSurface;

    public bool shouldTakeDamage = true;

    private Vector3 scaleVsCollider;

    //For use with ParentConstraints
    private ConstraintSource constraintSource;

    [SerializeField] private bool colliderControlSize;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Collider2D>().enabled = true;
        moving = false;
        readyToLaunch = true;
        //readyToPlay = false;
        
        playerShrink = GetComponent<PlayerShrink>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        raycastReflection = GetComponent<RaycastReflection>();
        /*
        if (isTutorial)
        {
            autoStick = true;
        } 
        else if (PlayerPrefs.HasKey("AutoStick"))
        {
            autoStick = PlayerPrefs.GetInt("AutoStick") == 1;
        }
        */
        autoStick = true;

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void HoldPlayerInPlace()
    {
        StartCoroutine(PauseAgainstWall());
    }

    private IEnumerator PauseAgainstWall()
    {
        GetComponent<CircleCollider2D>().enabled = true;
        shouldTakeDamage = false;
        while (moving)
            yield return new WaitForSeconds(0.01f);
        readyToLaunch = false;
        readyToPlay = false;
        rb.velocity = Vector2.zero;
        //rb.constraints = RigidbodyConstraints2D.FreezeAll;

    }

    public void ReleasePlayer()
    {
        shouldTakeDamage = true;
        rb.constraints = RigidbodyConstraints2D.None;
        readyToLaunch = true;
        readyToPlay = true;
        GetComponent<CircleCollider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (pausePanel.activeSelf)
        {
            return;
        }
        if (readyToPlay)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            if (!moving)
            {
                if (grabbedSurface != null && !grabbedSurface.activeInHierarchy)
                {
                    StartCoroutine(Fall(3f));
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
                
            }
                
            raycastReflection.isOverUI = isOverUI;
            if ((pausePanel != null && pausePanel.activeSelf || isOverUI))
            {
                return;
            }

            if (Input.GetMouseButtonDown(0) && !moving)
                readyToLaunch = true;
        
            //When mouse button released
            if (Input.GetMouseButtonUp(0) )
            {
                //Check readyToLaunch to make sure it doesn't automatically launch when we release click after stopping
                if (readyToLaunch && readyToPlay)
                {
                
                    transform.SetParent(null);
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
                    Vector2 test = mouseDir * baseSpeed;
                    //if (Mathf.Atan2(test.y, test.x) * Mathf.Rad2Deg < 179)
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
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        
    }

    public void PlaySquishSound()
    {
        //audioSource.Stop();
        audioSource.PlayOneShot(squishSound);
    }
    
    

    void OnCollisionEnter2D(Collision2D col)
    {
        if (moving)
        {
            if (!col.gameObject.CompareTag("Food") && !col.gameObject.CompareTag("Damaging"))
            {
                if (col.gameObject.CompareTag("NoStick"))
                {
                    audioSource.PlayOneShot(bounceSound);
                }
                else
                {
                    PlaySquishSound();
                }
                //audioSource.Stop();
                
            }
            Vector2 v = rb.velocity;
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
        }
        
        
        //If we hit a bounceable object while holding down mouse and moving
        if (col.gameObject.CompareTag("Bounceable") && grabbing)
        {
            //Debug.Log(col.gameObject.tag);
            grabbing = false;

            transform.up = col.GetContact(0).normal;
            //Calculate reverse of current velocity
            Vector2 reverseVelocity = -rb.velocity;
            //Set current velocity to zero. Stopping the player
            rb.velocity = Vector2.zero;
            moving = false;
            readyToLaunch = true;
            //transform.position = col.GetContact(0).point;
            transform.position = col.GetContact(0).point + col.GetContact(0).normal * GetComponent<CircleCollider2D>().radius * transform.localScale.x;
            grabbedSurface = col.gameObject;
            //transform.SetParent(col.gameObject.transform, true);
            constraintSource.sourceTransform = col.transform;
            constraintSource.weight = 1;
            //GetComponent<ParentConstraint>().AddSource(constraintSource);

            if (colliderControlSize)
            {
                Vector3 colScale = col.transform.lossyScale;
                transform.SetParent(col.transform);
            }
            
            //transform.localScale = transform.localScale / colScale.x;

            //GetComponent<ParentConstraint>().locked = true;
            //GetComponent<ParentConstraint>().constraintActive = true;

        }

        

        if (col.gameObject.CompareTag("Damaging") && shouldTakeDamage)
        {
            //Debug.Log("Should be taking damage");
            TakeDamage(damageFromHazards,zapSound, col);
        }
    }

    private IEnumerator MercyInvuln()
    {
        shouldTakeDamage = false;
        yield return new WaitForSeconds(1f);
        shouldTakeDamage = true;
    }
    public void TakeDamage(int damage, [CanBeNull] AudioClip damageSound, Collision2D coll = null, bool useAnimation = true)
    {
        if (!shouldTakeDamage)
            return;

        StartCoroutine(MercyInvuln());
        if (useAnimation)
            animator.SetTrigger("Shock");
        if (damageSound == null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(zapSound);
        }
        else
        {
            audioSource.Stop();
            audioSource.PlayOneShot(damageSound);
        }
        playerShrink.TakeDamage(damage);
        
        //rb.velocity = Vector2.zero;
        if (coll != null)
        {
            //rb.velocity = -rb.velocity;
        }
        else
        {
            //StartCoroutine(Fall(5f));
            grabbing = true;
            moving = true;
        }
        
    }

    public void ChangeDirection(Vector2 newVelocity)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(newVelocity.normalized * baseSpeed, ForceMode2D.Impulse);
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
            yield return new WaitForSeconds(0.05f);
        }
        
        
        rb.gravityScale = 0f;
        //yield return new WaitForSeconds(0.05f);
    }

}
