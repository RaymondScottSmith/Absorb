using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShrink : MonoBehaviour
{
    [SerializeField] private int startingHealth;

    [SerializeField] private float minimumScale;

    [SerializeField] private float loseHealthPerSecond;

    public int currentHealth;
    
    //Calculated value to decrease the scale by for every 1 health lost
    private float scaleValue;

    // Start is called before the first frame update
    void Start()
    {
        //Set currentHealth to starting health
        currentHealth = startingHealth;
        //Calculate scale value
        scaleValue = (1 - minimumScale)/startingHealth;
        //Start the Coroutine that will decrease health over time
        StartCoroutine(LoseHealth());

        
    }
    

    private IEnumerator LoseHealth()
    {
        //Decrease health every second until it's 0
        while (currentHealth > 0)
        {
            currentHealth--;
            //Calculate the new scale
            float newScale = scaleValue * currentHealth + minimumScale;
            
            //Adjust scale as health decreases
            transform.localScale = new Vector3(newScale, newScale, 0);

            yield return new WaitForSeconds(1);
        }
        GameOver();
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
    }
}
