using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperFood : MonoBehaviour
{
    public bool allowedToShoot;
    [SerializeField] private float fireRate;

    [SerializeField] private float lookRange;

    [SerializeField] private CircleCollider2D circleCollider2D;
    [SerializeField] private Transform firingPoint;

    private Animator animator;

    private PlayerController player;
    
    private CrewShrink crewShrink;
    private bool alreadyFired;

    
    private LineRenderer laserSight;
    private Quaternion restingPosition;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        restingPosition = transform.rotation;
        alreadyFired = false;
        crewShrink = GetComponent<CrewShrink>();
        laserSight = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        circleCollider2D.radius = lookRange;
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("AimAndShoot", 0f, fireRate);
    }
    
    public void StartLooking()
    {
        allowedToShoot = true;
    }

    public void StopLooking()
    {
        allowedToShoot = false;
    }
    
    private void AimAndShoot()
    {
        

    }

    private void FixedUpdate()
    {
        bool facingRight;
        if (!allowedToShoot || crewShrink.beingEaten)
        {
            return;
        }

        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);

        if (hit.collider.CompareTag("Player") && !alreadyFired)
        {
            laserSight.enabled = true;
            if (player.transform.position.x < transform.position.x)
            {
                facingRight = false;
                transform.rotation = Quaternion.Euler(0, 180, transform.rotation.z);
            }
            else
            {
                facingRight = true;
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z);
            }

            StartCoroutine(ShootSniper());

            Quaternion oldTransform = transform.rotation;

            int yRot;
            if (facingRight)
            {
                //spriteRenderer.flipX = false;
                yRot = 0;
                transform.right = player.transform.position - transform.position;
            }
            else
            {
                //spriteRenderer.flipX = true;
                yRot = 180;
                transform.right = -(player.transform.position - transform.position);
            }

            transform.rotation = Quaternion.RotateTowards(oldTransform, transform.rotation, 180);

            if (facingRight)
                transform.eulerAngles = new Vector3(0, yRot, transform.rotation.eulerAngles.z);
            else
            {
                transform.eulerAngles = new Vector3(0, yRot, -transform.rotation.eulerAngles.z);
            }
        }
        else
        {
            laserSight.enabled = false;
            transform.rotation = restingPosition;
        }

    }

    private IEnumerator ShootSniper()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Should be shooting");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
