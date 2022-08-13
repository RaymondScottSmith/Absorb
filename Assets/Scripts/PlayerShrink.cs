using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShrink : Shrink
{
    private List<GameObject> foodBeingEaten;

    [SerializeField]
    private Gradient playerColor;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        foodBeingEaten = new List<GameObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //Set currentHealth to starting health
        currentHealth = startingHealth;
        //Calculate scale value
        scaleValue = (transform.localScale.x - minimumScale)/startingHealth;
        //Start the Coroutine that will decrease health over time
        StartCoroutine(LoseHealth());

    }
    

    private void FixedUpdate()
    {
        //Calculate the new scale
        float newScale = scaleValue * currentHealth + minimumScale;
            
        //Adjust scale as health changes
        transform.localScale = new Vector3(newScale, newScale, 0);
        //Adjust color as health changes
        float healthColor = (float)currentHealth / startingHealth;
        spriteRenderer.color = playerColor.Evaluate(healthColor);

    }


    
    private IEnumerator LoseHealth()
    {
        //Decrease health every second until it's 0
        while (currentHealth > 0)
        {
            currentHealth--;
            yield return new WaitForSeconds(1);
        }
        GameOver();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            other.GetComponent<Shrink>().AttachToEater(this.gameObject);
        }
    }
}
