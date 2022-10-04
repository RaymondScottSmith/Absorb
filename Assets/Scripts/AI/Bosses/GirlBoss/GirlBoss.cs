using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlBoss : MonoBehaviour
{

    [SerializeField] private AudioClip painSound;
    [SerializeField] private AudioClip kickSound;
    private AudioSource myAudio;
    private bool isCollidingDrone;

    private Animator myAnimator;

    private GameObject player;

    private SpriteRenderer mySprite;

    [SerializeField]
    private Collider2D kickCollider;

    public bool isFlipped = false;

    public bool isGrounded;

    [SerializeField] private Transform groundSensor;
    
    
    public Rigidbody2D rb;
    public float gravity = -9.81f;
    public float gravityScale = 1;
    public float jumpForce = 5;
    private float velocity;

    public float xVelocity;

    [SerializeField]
    private List<Transform> ladders;
    // Start is called before the first frame update
    private void Start()
    {
        myAudio = GetComponent<AudioSource>();
        myAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        mySprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        isGrounded = true;
    }

    public Transform GetRandomLadder()
    {
        int num = Random.Range(0, ladders.Count);
        return ladders[num];
    }

    private void FixedUpdate()
    {
        float vPos;
        velocity += (gravity * gravityScale)/10f * Time.deltaTime;
        /*
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Should be jumping");
            velocity = 0.25f;
            vPos = rb.position.y + velocity;
            rb.MovePosition(new Vector3(rb.position.x, vPos));
        }
        */
        vPos = rb.position.y + velocity;
        float xPos;
        if (!isFlipped)
            xPos = rb.position.x + (xVelocity* Time.deltaTime);
        else
        {
            xPos = rb.position.x - (xVelocity* Time.deltaTime);
        }
        //Gravity force
        if (!isGrounded)
        {
            Debug.Log("XVelocity: " + xVelocity);
            Debug.Log("XPos: " + xPos);
            
            rb.MovePosition(new Vector3(xPos, vPos));
            
        }
            
        
    }

    public void JumpBack()
    {
        
        float vPos;
        velocity = 0.3f;
        vPos = rb.position.y + velocity;
        float xPos = rb.position.x - velocity;
        //rb.MovePosition(new Vector3(rb.position.x, vPos));
        rb.MovePosition(new Vector3(rb.position.x, vPos));
        
        xVelocity = 5f;
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundSensor.position, Vector2.down, 0.5f);
        if (hit.collider != null)
        {
            Debug.Log("Hitting Ground");
            isGrounded = true;
            //xVelocity = 0f;
        }
        else
        {
            Debug.Log("Not Hitting Ground");
            isGrounded = false;
        }

        
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.transform.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.transform.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
    
    public void LookAtTarget(Vector2 target)
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > target.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < target.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void PlayKickSound()
    {
        myAudio.PlayOneShot(kickSound);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Drone") && !isCollidingDrone)
        {
            CameraDrone cd = collision.GetComponent<CameraDrone>();
            Debug.Log(cd.isReturning);
            Debug.Log(cd.isRotating);
            if (!cd.isRotating && !cd.isReturning)
            {
                isCollidingDrone = true;
                myAudio.PlayOneShot(painSound);
                myAnimator.SetTrigger("Hurt");
                cd.ReverseVelocity();
                cd.GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isCollidingDrone && collision.gameObject.CompareTag("Drone"))
        {
            isCollidingDrone = false;
        }
    }
}

public enum GB_State
{
    KickPlan,
    CornershotPlan
}