using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class ShipControls : MonoBehaviour
{
    [SerializeField]
    private ForwardDir bowDirection;

    [SerializeField] private float frictionValue = 0.1f;

    [SerializeField] private float veerForce = 1f;
    
    [SerializeField] private Vector2 maxPositions = new Vector2(10,10);

    [SerializeField] private Vector2 minPositions = new Vector2(-10, -10);

    [SerializeField] private bool invincible = false;

    [SerializeField] private int maxHealth = 100;

    [SerializeField] private int asteroidDamage = 20;

    [SerializeField] private int healthPickup = 20;

    [SerializeField] private AudioClip healthPickupSound;

    private int currentHealth;
    
    private Rigidbody2D myRB;

    private PlayerController player;

    private AudioSource audioSource;

    [SerializeField] private Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.minValue = 0;
        myRB = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        player = FindObjectOfType<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        healthSlider.value = currentHealth;
        //Friction in space?
        myRB.velocity -= myRB.velocity * frictionValue;
        
        //Check within x bounds
        if (myRB.position.x < minPositions.x)
        {
            myRB.position = new Vector2(minPositions.x, myRB.position.y);
        }
        else if (myRB.position.x > maxPositions.x)
        {
            myRB.position = new Vector2(maxPositions.x, myRB.position.y);
        }
        
        //Check within y bounds
        if (myRB.position.y < minPositions.y)
        {
            myRB.position = new Vector2(myRB.position.x, minPositions.y);
        }
        else if (myRB.position.y > maxPositions.y)
        {
            myRB.position = new Vector2(myRB.position.x, maxPositions.y);
        }
        
        
    }

    public void HardPort()
    {
        Vector2 impDirection = Vector2.zero;
        switch (bowDirection)
        {
            case ForwardDir.FRight:
                if (Distance(myRB.position.y, maxPositions.y) > 0.5f)
                    impDirection = Vector2.up;
                break;
            case ForwardDir.FUp:
                impDirection = Vector2.left;
                break;
        }
        
        myRB.AddForce(impDirection * veerForce, ForceMode2D.Impulse);
    }

    public void HardStarboard()
    {
        
        Vector2 impDirection = Vector2.zero;
        switch (bowDirection)
        {
            case ForwardDir.FRight:
                if (Distance(myRB.position.y, minPositions.y) > 0.5f)
                    impDirection = Vector2.down;
                break;
            
            case ForwardDir.FUp:
                impDirection = Vector2.right;
                break;
        }
        
        myRB.AddForce(impDirection * veerForce, ForceMode2D.Impulse);
    }

    public void Accelerate()
    {
        Vector2 impDirection = Vector2.zero;
        switch (bowDirection)
        {
            case ForwardDir.FRight:
                if (Distance(myRB.position.x, maxPositions.x) > 0.5f)
                    impDirection = Vector2.right;
                break;
            
            case ForwardDir.FUp:
                impDirection = Vector2.up;
                break;
        }
        
        myRB.AddForce(impDirection * veerForce, ForceMode2D.Impulse);
    }

    public void Decelerate()
    {
        Vector2 impDirection = Vector2.zero;
        switch (bowDirection)
        {
            case ForwardDir.FRight:
                if (Distance(myRB.position.x, minPositions.x) > 0.5f)
                    impDirection = Vector2.left;
                break;
            
            case ForwardDir.FUp:
                impDirection = Vector2.down;
                break;
        }
        
        myRB.AddForce(impDirection * veerForce, ForceMode2D.Impulse);
    }
    
    private float Distance(float num1, float num2)
    {
        return Mathf.Abs(num1 - num2);
    }

    public void HealthPickup()
    {
        currentHealth += healthPickup;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        GetComponent<Animator>().SetTrigger("Healed");
        audioSource.PlayOneShot(healthPickupSound);
    }
    
    public void AsteroidHit()
    {
        currentHealth -= asteroidDamage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroyed();
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Damaged");
        }
    }

    private void Destroyed()
    {
        player.HoldPlayerInPlace();
        GetComponent<PlayableDirector>().Play();
    }

    public void DestroyMe()
    {
        player.GetComponent<PlayerShrink>().Die();
        Destroy(gameObject);
    }
    
    private enum ForwardDir
    {
        FRight,
        FLeft,
        FDown,
        FUp
    }
}