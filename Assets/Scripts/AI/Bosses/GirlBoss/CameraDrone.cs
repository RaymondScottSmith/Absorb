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

    private Animator animator;

    private PlayerController player;

    public bool isRotating;

    public bool isReturning;

    private Rigidbody2D rb;

    private AudioSource audioSource;

    [SerializeField]
    private Rigidbody2D bossRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isRotating = true;
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isRotating)
        {
            
            transform.RotateAround(gBossRotateCenter.transform.position, axis, angle);
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
            else if (Mathf.Abs(distanceToBoss - distance) > 0.1f)
            {
                Debug.Log(distance);
                //transform.position = centerPosition + Vector3.up * distanceToBoss;
            }

            animator.SetBool("FacingRight", transform.position.x < gBossRotateCenter.transform.position.x);
        }

        if (isReturning)
        {
            if (Vector2.Distance(transform.position, gBossRotateCenter.transform.position) > distanceToBoss)
            {
                transform.Translate((gBossRotateCenter.transform.position - transform.position).normalized * returnSpeed);
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
        
    }

    private IEnumerator BounceTimer()
    {
        yield return new WaitForSeconds(timeUntilReturn);
        rb.velocity = Vector3.zero;
        isReturning = true;

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
        
        if (col.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
            player = col.gameObject.GetComponent<PlayerController>();
            player.ChangeDirection((player.gameObject.transform.position - transform.position) * 10f);
            ChangeDirection((transform.position - player.gameObject.transform.position) * 10f, player.baseSpeed/2f);
            //ChangeDirection((transform.position - player.gameObject.transform.position) * 10f, player.baseSpeed);
            isRotating = false;
            StartCoroutine(BounceTimer());
            //isExploding = true;
            //isPursuing = false;
            //wasBounced = true;
            //myAI.StopPursuit();
            //player.TakeDamage(explosionDamage,null);
            //player.ChangeDirection(col.contacts[0].normal * 10f);
            //StartCoroutine(Explode());
        }

        /*
        if (wasBounced == true && !col.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(Explode());
        }
        */

    }
    
    
}
