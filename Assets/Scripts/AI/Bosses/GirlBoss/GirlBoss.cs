using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public class GirlBoss : MonoBehaviour
{

    [SerializeField] private AudioClip painSound;
    [SerializeField] private AudioClip kickSound;
    [SerializeField] private AudioClip laserSound;
    [SerializeField] private AudioClip gruntSound;
    [SerializeField]
    private AudioSource myAudio;
    private bool isCollidingDrone;

    private Animator myAnimator;

    private GameObject player;

    private SpriteRenderer mySprite;

    [SerializeField]
    private Collider2D kickCollider;

    public bool isFlipped = false;

    public bool isGrounded;

    public int maxHealth = 3;

    public int health;

    [SerializeField] private Transform groundSensor;

    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private CameraDrone cameraDrone;
    
    
    public Rigidbody2D rb;
    public float gravity = -9.81f;
    public float gravityScale = 1;
    public float jumpForce = 5;
    private float velocity;

    public float xVelocity;

    [SerializeField]
    public List<Transform> ladders;

    [SerializeField]
    private LineRenderer crouchLine;

    [SerializeField] public Transform arenaCenter;

    [SerializeField] private GB_Spawner spawner;
    [SerializeField] private ParticleSystem particleSystem;

    public GB_State bossState;

    public bool isClimbing = false;

    public bool isFacingPlayer = false;

    private bool isDead;

    [SerializeField] private PlayableDirector deathCutscene;
    // Start is called before the first frame update
    private void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        health = maxHealth;
        //bossState = GB_State.Stage3;
        myAudio = GetComponent<AudioSource>();
        myAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        mySprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        isGrounded = true;
        isClimbing = false;
        isFacingPlayer = false;
    }

    public void BreakShield()
    {
        particleSystem.Play();
    }
    public void BeginStage1()
    {
        bossState = GB_State.Stage1;
        //isFlipped = true;
        mySprite.flipX = true;
    }
    public Transform GetRandomLadder()
    {
        int num = Random.Range(0, ladders.Count);
        return ladders[num];
        //return ladders[0];
    }

    public void PlayStage3Intro()
    {
        playableDirector.Play();
        bossState = GB_State.Stage3;
    }

    public void SwitchFacingPlayer()
    {
        isFacingPlayer = !isFacingPlayer;
    }

    public void UpdateCamera()
    {
        cameraDrone.isStage3 = true;
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
    public void JumpForward()
    {
        
        float vPos;
        velocity = 0.3f;
        vPos = rb.position.y + velocity;
        
        float xPos = rb.position.x - velocity;
        //rb.MovePosition(new Vector3(rb.position.x, vPos));
        rb.MovePosition(new Vector3(rb.position.x, vPos));
        if (transform.position.x < arenaCenter.position.x)
        {
            if (isFlipped)
                xVelocity = -5f;
            else
            {
                xVelocity = 5f;
            }
        }
        else
        {
            if (isFlipped)
                xVelocity = 5f;
            else
            {
                xVelocity = -5f;
            }
        }
    }

    public void PausePlayer()
    {
        if (spawner != null)
        {
            spawner.ClearMines();
        }
        player.GetComponent<PlayerController>().HoldPlayerInPlace();
        
    }

    public void StartStage3Mines()
    {
        spawner.UpdateStage(GB_Spawner.GB_Stage.Stage3);
    }
    
    public void StopAllSound()
    {
        myAudio.Stop();
    }

    public void ChangeStage(GB_State newState)
    {
        bossState = newState;
    }

    public void DeathCutscene()
    {
        spawner.UpdateStage(GB_Spawner.GB_Stage.Off);
        deathCutscene.Play();
        isDead = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void Update()
    {
        

        
        if (bossState != GB_State.Stage3 && bossState != GB_State.None)
            mySprite.flipX = true;
            
        if (transform.localPosition.y < -1f && !isDead)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -.03f, 0);
        }
        else if (transform.localPosition.y < -2f)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -1.4f, 0);
        }
        
        RaycastHit2D hit = Physics2D.Raycast(groundSensor.position, Vector2.down, 0.5f);
        if (hit.collider != null)
        {
            if (!isGrounded)
                transform.localPosition = new Vector2(transform.localPosition.x, -.21f);
            isGrounded = true;
            //Adjust to ground level
            
            
        }
        else
        {
            isGrounded = false;
        }
        
        if (isFacingPlayer)
        {
            LookAtPlayer();
        }
        float vPos;
        velocity += (gravity * gravityScale)/10f * Time.deltaTime;
        vPos = rb.position.y + velocity;
        float xPos;
        if (!isFlipped)
            xPos = rb.position.x + (xVelocity* Time.deltaTime);
        else
        {
            xPos = rb.position.x - (xVelocity* Time.deltaTime);
        }
        //Gravity force
        if (!isGrounded && !isClimbing)
        {
            //Debug.Log("XVelocity: " + xVelocity);
            //Debug.Log("XPos: " + xPos);
            
            rb.MovePosition(new Vector3(xPos, vPos));
            
        }

        
    }

    public void LookAtPlayer()
    {
        Debug.Log("Should be facing player");
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

    public void FireCrouchedLaser()
    {
        Vector2 facingDirection = Vector2.left;
        if (isFlipped)
            facingDirection = Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(groundSensor.position, facingDirection, 100f,1 << LayerMask.NameToLayer("Obstacle"));
        if (hit.collider != null)
        {
            Debug.Log("Distance to hit object: " + hit.distance);
            crouchLine.SetPosition(1,Vector3.zero);
            crouchLine.SetPosition(0, new Vector3(hit.distance-1f,0,0));
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

    public void PlayLaserSound()
    {
        myAudio.PlayOneShot(laserSound);
    }

    public void PlayGruntSound()
    {
        myAudio.PlayOneShot(gruntSound);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Drone") && !isCollidingDrone)
        {
            CameraDrone cd = collision.GetComponent<CameraDrone>();
            if (!cd.isRotating)
            {
                if (!cd.isReturning || bossState == GB_State.Stage3)
                {
                    isCollidingDrone = true;
                    //myAudio.PlayOneShot(painSound);
                    myAnimator.SetTrigger("Hurt");
                    cd.ReverseVelocity();
                    cd.GetComponent<Collider2D>().enabled = false;
                }
                
            }
        }
        else if (collision.gameObject.CompareTag("Player") && isDead)
        {
            Debug.Log("Demo Completed!");
        }
    }

    public void PlayParticles()
    {
        particleSystem.Play();
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
    None,
    Stage1,
    Stage2,
    Stage3
}