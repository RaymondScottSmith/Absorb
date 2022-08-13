using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shrink : MonoBehaviour
{
    
    [SerializeField] protected int startingHealth;

    [SerializeField] protected float minimumScale;
    
    public int currentHealth;
    protected float scaleValue;

    private bool beingEaten = false;
    private Transform attachedEater;

    private Vector3 attachPoint;
    

    private void Awake()
    {
        currentHealth = startingHealth;
        scaleValue = (transform.localScale.x - minimumScale)/startingHealth;
        
    }

    private void Update()
    {
        //Calculate the new scale
        float newScale = scaleValue * currentHealth + minimumScale;
            
        //Adjust scale as health decreases
        transform.localScale = new Vector3(newScale, newScale, 0);
        if (beingEaten)
            transform.position = attachedEater.position + attachPoint;
    }

    public void AttachToEater(GameObject eater)
    {
        float eaterRadius = eater.transform.localScale.x / 2;
        attachPoint = new Vector3(Random.Range(-eaterRadius, eaterRadius), Random.Range(-eaterRadius, eaterRadius), 0);
        GetComponent<CircleCollider2D>().enabled = false;
        attachedEater = eater.transform;
        beingEaten = true;
        currentHealth = startingHealth;
        StartCoroutine(BeEaten(eater.GetComponent<Shrink>()));
    }

    private IEnumerator BeEaten(Shrink eater)
    {
        //Decrease health every second until it's 0
        while (currentHealth > 0)
        {
            eater.GainHealth(2);
            currentHealth--;
            yield return new WaitForSeconds(1);
        }
        Destroy(gameObject);
    }
    
    public void GainHealth(int health)
    {
        currentHealth += health;
    }
    
}
