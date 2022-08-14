using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shrink : MonoBehaviour
{
    
    [SerializeField] protected int startingHealth;

    [SerializeField] protected float minimumScale;

    [SerializeField] protected int damageOverTime = 1;

    [SerializeField] private int eatPerSecond;
    
    public int currentHealth;
    protected float scaleValue;

    private bool beingEaten = false;
    private Transform attachedEater;

    private Vector3 attachPoint;
    protected SpriteRenderer spriteRenderer;

    [SerializeField] private Gradient foodGradient;

    protected Collider2D shrinkCollider;

    private Animator animator;

    private Runner runner;

    private SpawnManager spawnManager;
    

    private void Awake()
    {
        currentHealth = startingHealth;
        scaleValue = (transform.localScale.x - minimumScale)/startingHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        shrinkCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        runner = GetComponent<Runner>();
        spawnManager = FindObjectOfType<SpawnManager>();

    }

    private void Update()
    {
        //Calculate the new scale
        float newScale = scaleValue * currentHealth + minimumScale;
            
        //Adjust scale as health decreases
        transform.localScale = new Vector3(newScale, newScale, 0);
        //Adjust color as health decreases
        
        if (beingEaten)
        {
            transform.position = attachedEater.position + attachPoint;
            transform.rotation = attachedEater.rotation;
            float healthColor = (float)currentHealth / startingHealth;
            spriteRenderer.color = foodGradient.Evaluate(healthColor);

        }
            
    }

    public void AttachToEater(GameObject eater)
    {
        spawnManager.PlayScream();
        float eaterRadius = eater.transform.localScale.x / 2;
        attachPoint = new Vector3(Random.Range(-eaterRadius, eaterRadius), Random.Range(-eaterRadius, eaterRadius), 0);
        GetComponent<Collider2D>().enabled = false;
        attachedEater = eater.transform;
        beingEaten = true;
        currentHealth = startingHealth;
        runner.isJourneying = false;
        if (animator != null)
            animator.SetTrigger("Dying");
        StartCoroutine(BeEaten(eater.GetComponent<Shrink>()));
    }

    private IEnumerator BeEaten(Shrink eater)
    {
        //Decrease health every second until it's 0
        while (currentHealth > 0)
        {
            eater.GainHealth(eatPerSecond);
            currentHealth-=damageOverTime;
            yield return new WaitForSeconds(1);
        }
        
        Destroy(gameObject);
    }
    
    public void GainHealth(int health)
    {
        currentHealth += health;
    }
    
}
