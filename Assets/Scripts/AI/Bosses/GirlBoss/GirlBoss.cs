using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

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

    public GB_State startingState = GB_State.None;
    

    public bool isClimbing = false;

    public bool isFacingPlayer = false;

    private bool isDead;

    [SerializeField] private PlayableDirector deathCutscene;

    public Collider2D shieldCollider;
    // Start is called before the first frame update
    private void Start()
    {
        bossState = startingState;
        particleSystem = GetComponentInChildren<ParticleSystem>();
        health = maxHealth;
        //bossState = GB_State.Stage3;
        myAudio = GetComponent<AudioSource>();
        myAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        mySprite = GetComponent<SpriteRenderer>();
        mySprite.flipX = true;
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
        spawner.UpdateStage(GB_Spawner.GB_Stage.Stage1);
        isFlipped = false;
        mySprite.flipX = false;
        
    }
    public Transform GetRandomLadder()
    {
        int num = Random.Range(0, ladders.Count);
        return ladders[num];
        //return ladders[0];
    }

    public void PlayStage3Intro()
    {
        PausePlayer();
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
        velocity = 0.35f;
        float vPos = rb.position.y + velocity;
        float xPos;
        if (isFlipped)
            xPos = rb.position.x - velocity*2;
        else
        {
            xPos = rb.position.x + velocity*2;
        }
        rb.MovePosition(new Vector3(xPos, vPos));
        //rb.MovePosition(new Vector3(rb.position.x, vPos));
        xVelocity = -7f;
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

    public void ClearMines()
    {
        spawner.ClearMines();
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
        CameraController camController = FindObjectOfType<CameraController>();
        camController.FocusOnTarget(gameObject);
        camController.StopMusic();
        spawner.UpdateStage(GB_Spawner.GB_Stage.Off);
        deathCutscene.Play();
        isDead = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void Update()
    {
        if (transform.localPosition.y < -0.04f && !isDead && bossState != GB_State.Stage3)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -.03f, 0);
            
        }
        else if (transform.localPosition.y < -2f && !isDead && bossState != GB_State.Stage3)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -1.4f, 0);
            
        }
        
        float distance = bossState == GB_State.Stage3 ? 1.5f : 1f;
        
        RaycastHit2D hit = Physics2D.Raycast(groundSensor.position, Vector2.down, distance);
        if (hit.collider != null)
        {
            if (!isGrounded && (bossState == GB_State.Stage1 || bossState == GB_State.Stage2))
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
            LookAtTarget(player.gameObject.transform.position);
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
        if (!isGrounded && !isClimbing && !isDead)
        {
            //Debug.Log("XVelocity: " + xVelocity);
            //Debug.Log("XPos: " + xPos);
            
            rb.MovePosition(new Vector3(xPos, vPos));
            
        }

        
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        /*
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
        */
        
        //Moving Left
        if (transform.position.x > player.transform.position.x)
        {
            //transform.localScale = flipped;
            //transform.localRotation.eulerAngles = new Vector3(0f, 180f, 0f);
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0,180f,0));
            isFlipped = true;
        }
        else if (transform.position.x < player.transform.position.x) //Moving Right
        {
            //transform.localScale = flipped;
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0f, 0));
            isFlipped = false;
        }
        
    }

    public void FireCrouchedLaser()
    {
        Vector2 facingDirection = Vector2.right;
        if (isFlipped)
            facingDirection = Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(groundSensor.position, facingDirection, 100f,1 << LayerMask.NameToLayer("Obstacle"));
        if (hit.collider != null)
        {
            crouchLine.SetPosition(1,Vector3.zero);
            crouchLine.SetPosition(0, new Vector3(hit.distance-1f,0,0));
        }
    }
    
    public void LookAtTarget(Vector2 target)
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        
        
        //Moving Left
        if (transform.position.x > target.x)
        {
            //transform.localScale = flipped;
            //transform.localRotation.eulerAngles = new Vector3(0f, 180f, 0f);
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f,180f,0f));
            //Debug.Log(transform.rotation);
            isFlipped = true;
        }
        else if (transform.position.x < target.x) //Moving Right
        {
            //transform.localScale = flipped;
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f, 0f, 0f));
            
            isFlipped = false;
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
        if (collision.gameObject.CompareTag("Drone") && !isCollidingDrone && player.GetComponent<PlayerController>().shouldTakeDamage)
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
            ScreenTransition.Instance.LoadDemoEnd();
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