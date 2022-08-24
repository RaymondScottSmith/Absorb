using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShootingFood : MonoBehaviour
{
    public bool allowedToShoot;

    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private float fireRate;

    [SerializeField] private float lookRange;

    [SerializeField] private CircleCollider2D circleCollider2D;
    [SerializeField] private Transform firingPoint;

    private Animator animator;

    private PlayerController player;
    
    private CrewShrink crewShrink;
    

    // Start is called before the first frame update
    void Start()
    {
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
        if (!allowedToShoot || crewShrink.beingEaten)
        {
            return;
        }

        if (player.transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);

        if (hit.collider.CompareTag("Player"))
        {
            StartCoroutine(ShootProjectile());
        }

    }

    private IEnumerator ShootProjectile()
    {
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.75f);
        //audioSource.PlayOneShot(firingSound);
        Instantiate(projectilePrefab, firingPoint);
    }
}
