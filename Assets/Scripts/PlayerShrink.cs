using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShrink : Shrink
{
    private List<GameObject> foodBeingEaten;

    // Start is called before the first frame update
    void Start()
    {
        foodBeingEaten = new List<GameObject>();
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
            
        //Adjust scale as health decreases
        transform.localScale = new Vector3(newScale, newScale, 0);
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
