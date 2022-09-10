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
    // Start is called before the first frame update
    void Start()
    {
        alreadyFired = false;
        crewShrink = GetComponent<CrewShrink>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        circleCollider2D.radius = lookRange;
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

        if (player.transform.position.x < transform.position.x)
        {
            facingRight = false;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            facingRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);

        if (hit.collider.CompareTag("Player") && !alreadyFired)
        {
            StartCoroutine(ShootSniper());
        }
        
        Quaternion oldTransform = transform.rotation;
        
        transform.right = player.transform.position - transform.position;
        
        
        transform.rotation = Quaternion.RotateTowards(oldTransform, transform.rotation, 170);
        //rb.AddForce(transform.right * speed, ForceMode2D.Force);
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
