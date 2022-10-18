using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrone : MonoBehaviour
{

    public GameObject gBossRotateCenter; // position in worldspace of the boss

    public Vector3 axis; //The axis by which it will rotate

    public float angle; //Speed of rotation;

    public float distanceToBoss;

    public float timeUntilReturn;

    public float returnSpeed = 1f;

    public float moveSpeed = 2f;

    public float aimAssistSpeed = 2f;

    private Animator animator;

    private PlayerController player;

    public bool isRotating;

    public bool isReturning;

    private Rigidbody2D rb;

    private AudioSource audioSource;

    [SerializeField] private GirlBoss gb;

    [SerializeField]
    private Rigidbody2D bossRigidbody;

    private bool isMovingLeft;

    public bool isStage3;

    private bool isClockwise;

    public bool inCutscene;
    
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isRotating = true;
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        isMovingLeft = false;
        isStage3 = false;
        isClockwise = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inCutscene)
        {
            return;
        }
        if (!isReturning && isStage3)
        {
            isRotating = false;
            float yPos = gb.arenaCenter.position.y;
            float xPos = 0f;
            Vector3 target = Vector3.zero;
            if (isMovingLeft)
            {
                
                xPos = gb.ladders[0].position.x;
                target = new Vector3(xPos, yPos, 0);
                transform.Translate((target - transform.position)
                    .normalized * moveSpeed * Time.deltaTime);
                if (Vector3.Distance(target, transform.position) < 1f)
                    animator.SetBool("FacingRight", true);
            }
            else
            {
                
                xPos = gb.ladders[3].position.x;
                target = new Vector3(xPos, yPos, 0);
                transform.Translate((target - transform.position)
                    .normalized * moveSpeed * Time.deltaTime);
                if (Vector3.Distance(target, transform.position) < 1f)
                    animator.SetBool("FacingRight", false);
            }
            

            if (Vector3.Distance(target, transform.position) < 0.1f)
            {
                isMovingLeft = !isMovingLeft;
            }


        }
        if (isRotating)
        {
            if (isClockwise)
                transform.RotateAround(gBossRotateCenter.transform.position, axis, angle);
            else
            {
                transform.RotateAround(gBossRotateCenter.transform.position, axis, -angle);
            }
            transform.rotation = Quaternion.Euler(Vector3.zero);
            float radius = 1.5f;
            Vector3 centerPosition = gBossRotateCenter.transform.position;
            
            float distance = Vector3.Distance(transform.position, centerPosition);

            if (distance > radius)
            {
                Vector3 fromOriginToObject = transform.position - centerPosition;
                fromOriginToObject *= radius / distance;
                transform.position = centerPosition + fromOriginToObject;
            }
            //if (isClockwise)
                animator.SetBool("FacingRight", transform.position.x < gBossRotateCenter.transform.position.x);
            /*
            else
            {
                animator.SetBool("FacingRight", transform.position.x > gBossRotateCenter.transform.position.x);
            }
            */
        }

        if (isReturning)
        {
            if (!isStage3)
            {
                if (Vector2.Distance(transform.position, gBossRotateCenter.transform.position) > distanceToBoss)
                {
                    transform.Translate((gBossRotateCenter.transform.position - transform.position).normalized * returnSpeed * Time.deltaTime);
                }
                else if (Vector2.Distance(transform.position, gBossRotateCenter.transform.position) < distanceToBoss)
                {
                    rb.velocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    transform.position = gBossRotateCenter.transform.position + new Vector3(0, distanceToBoss, 0);
                    isReturning = false;
                    isRotating = true;
                    GetComponent<Collider2D>().enabled = true;
                }
            }
            else
            { //Return to top float position
                Vector3 target = new Vector3(rb.position.x, gb.arenaCenter.position.y);
                transform.Translate((target - transform.position)
                    .normalized * returnSpeed * Time.deltaTime);

                if (Vector3.Distance(target, transform.position) <= 0.1f)
                {
                    isReturning = false;
                    GetComponent<Collider2D>().enabled = true;
                }
                
            }
            
        }
        
    }

    private IEnumerator BounceTimer()
    {
        yield return new WaitForSeconds(timeUntilReturn);
        rb.velocity = Vector3.zero;
        isReturning = true;

    }

    public void EndCutscene()
    {
        inCutscene = false;
    }

    public void ChangeDirection(Vector2 newVelocity, float baseSpeed)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector3.zero;
        rb.AddForce(newVelocity.normalized * baseSpeed, ForceMode2D.Impulse);
    }
    public void ReverseVelocity()
    {
        rb.velocity = -rb.velocity;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        
        if (col.gameObject.CompareTag("Player") && col.gameObject.GetComponent<PlayerController>().shouldTakeDamage)
        {
            audioSource.Play();
            player = col.gameObject.GetComponent<PlayerController>();
            player.ChangeDirection((player.gameObject.transform.position - transform.position) * 10f);
            ChangeDirection((transform.position - player.gameObject.transform.position) * 10f, player.baseSpeed/2f);
            if (isStage3)
            {
                Vector2 newVelocity = (gBossRotateCenter.transform.position - transform.position);
                rb.AddForce(newVelocity.normalized * aimAssistSpeed, ForceMode2D.Impulse);
            }
            
            //ChangeDirection((transform.position - player.gameObject.transform.position) * 10f, player.baseSpeed);
            isRotating = false;
            StartCoroutine(BounceTimer());
            isClockwise = !isClockwise;
            
        }

    }
    
    
}
