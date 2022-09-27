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
        Physics2D.queriesStartInColliders = false;
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
        
        int layerMask = ~(LayerMask.GetMask("Player") + LayerMask.GetMask("CrewColliders"));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 500, layerMask);
        //Debug.Log(hit.collider.tag);
        if (hit.collider.CompareTag("Player") && !alreadyFired)
        {
            StartCoroutine(ShootProjectile());
        }

    }

    public void ReadyWeapon()
    {
        alreadyFired = false;
    }

    private IEnumerator ShootProjectile()
    {
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.75f);
        if (crewShrink.beingEaten)
            yield break;
        //audioSource.PlayOneShot(firingSound);
        alreadyFired = true;
        GameObject projectile = Instantiate(projectilePrefab, firingPoint);
        projectile.GetComponent<MiniRocket>().SetSource(this);
    }
}
