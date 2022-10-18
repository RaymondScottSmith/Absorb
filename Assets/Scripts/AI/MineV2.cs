using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineV2 : MonoBehaviour
{
    private TempAI myAI;

    //private bool isPursuing;
    private bool isInRange;

    private GameObject target;
    private bool isExploding;

    private float currentDistance;

    [SerializeField] private float explosionTimer = 5f;
    private float timeRemaining;

    private AudioSource audioSource;

    private Animator animator;

    [SerializeField] private int explosionDamage;

    [SerializeField] private AudioClip explosionSound;

    private CameraController camController;

    //private bool isFacingRight;

    private PlayerController player;

    [SerializeField] private bool timedExplosion;

    private Rigidbody2D rb;
    

    private GB_Spawner gbSpawner;

    private bool wasPaused = false;
    
    // Start is called before the first frame update
    void Start()
    {
        wasPaused = false;
        rb = GetComponent<Rigidbody2D>();
        myAI = GetComponent<TempAI>();
        ///passiveColor = midBar.color;
        audioSource = GetComponent<AudioSource>();
        
        animator = GetComponent<Animator>();
        camController = FindObjectOfType<CameraController>();
        //isFacingRight = false;

        player = FindObjectOfType<PlayerController>();
        timeRemaining = explosionTimer;
        //StartBeeping();
        myAI.StartMoving(player.transform);
        //isPursuing = true;
        InvokeRepeating("ExplodeFromTimer", explosionTimer, 10f);
    }

    private void ExplodeFromTimer()
    {
        /*
        if (wasBounced)
            return;
            */
        StartCoroutine(Explode());
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isExploding && collision.gameObject.CompareTag("Player"))
        {
            //This is so we don't get hit for damage every frame
            isExploding = false;
            player.TakeDamage(explosionDamage,null);
            
            player.ChangeDirection((transform.position - player.gameObject.transform.position) * 10f);
        }
    }

    public void AssignSpawner(GB_Spawner spawner)
    {
        gbSpawner = spawner;
    }
    

    void Update()
    {
        if (Time.timeScale == 0)
        {
            audioSource.Stop();
            wasPaused = true;
        }
        else if (wasPaused)
        {
            audioSource.Play();
            wasPaused = false;
        }
        timeRemaining -= Time.deltaTime;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {

    }
    

    private void StartBeeping()
    {
        //StartCoroutine(Beep());
    }

    /*
    private IEnumerator Beep()
    {
        /*
        float timeDist = currentDistance / 20f;
        //Debug.Log(timeDist);
        yield return new WaitForSeconds(timeDist);

        if (!isPursuing || isExploding)
            yield break;
        
        yield return new WaitForSeconds(timeRemaining / explosionTimer);
        //timeRemaining -= Time.deltaTime;
        if (wasBounced)
            yield break;
        audioSource.Play();
        StartCoroutine(Beep());
    }
    */
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && !isExploding)
        {
            
            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            isExploding = true;
            //isPursuing = false;
            myAI.StopPursuit();
            if (!isExploding)
                player.TakeDamage(explosionDamage,null);
            
            isExploding = true;
            player.ChangeDirection(col.contacts[0].normal * 10f);
            StartCoroutine(Explode());
        }
    }
    
    public void ChangeDirection(Vector2 newVelocity, float newSpeed)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(newVelocity.normalized * newSpeed, ForceMode2D.Impulse);
    }

    public void ForceExplosion()
    {
        isExploding = true;
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        
        myAI.StopPursuit();
        gbSpawner.RemoveMine(this);
        animator.SetTrigger("Explode");
        audioSource.Stop();
        audioSource.PlayOneShot(explosionSound);
        //camController.ShakeScreen();
        yield return new WaitForSeconds(1.3f);
        Destroy(gameObject);
    }
}
