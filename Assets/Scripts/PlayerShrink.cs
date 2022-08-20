using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerShrink : Shrink
{
    private List<GameObject> foodBeingEaten;

    [SerializeField]
    private Gradient playerColor;

    private Leaderboard leaderboard;
    
    public bool alive = true;

    public float time = 0;
    
    [SerializeField]
    private TMP_Text timeLabel;

    [SerializeField] private GameObject corpsePrefab;

    [SerializeField] private GameObject losePanel;

    public GameObject corpse;

    private List<Shrink> currentlyEating = new List<Shrink>();
    
    //[SerializeField] private bool isTutorial;

    //private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        leaderboard = FindObjectOfType<Leaderboard>();
        foodBeingEaten = new List<GameObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //Set currentHealth to starting health
        currentHealth = startingHealth;
        alive = true;
        time = 0;
        //Calculate scale value
        scaleValue = (transform.localScale.x - minimumScale) / startingHealth;
        //Start the Coroutine that will decrease health over time
        if (!isTutorial)
        {
            StartCoroutine(LoseHealth());
            StartCoroutine(RunTimer());
        }

    }

    private IEnumerator RunTimer()
    {
        while (alive)
        {
            timeLabel.SetText("Time: " + Mathf.Round(time).ToString());
            yield return new WaitForSeconds(1);
            time++;
        }

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

    public void AddFood(Shrink food)
    {
        currentlyEating.Add(food);
    }

    public void RemoveFood(Shrink food)
    {
        currentlyEating.Remove(food);
    }

    public bool CheckForKey(int keyNumber)
    {
        foreach (Shrink food in currentlyEating)
        {
            if (keyNumber == food.GetComponent<CrewShrink>().keyValue)
                return true;
        }

        return false;
    }
    
    private IEnumerator LoseHealth()
    {
        //Decrease health every second until it's 0
        while (currentHealth > 0)
        {
            currentHealth--;
            yield return new WaitForSeconds(1);
        }
        Die();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;
    }

    public void Die()
    {
        
        alive = false;
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        corpse = Instantiate(corpsePrefab, transform.position, corpsePrefab.transform.rotation);
        yield return leaderboard.SubmitScoreRoutine((int)Mathf.Round(time));
        timeLabel.SetText("Time: " + (int)Mathf.Round(time));
        losePanel.GetComponent<LosePanel>().GameOver((int)Mathf.Round(time));
        gameObject.SetActive(false);
    }
    
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Debug.Log("Should be attaching");
            other.GetComponent<Shrink>().AttachToEater(this.gameObject);
        }

        /*
        if (other.CompareTag("Terminal"))
        {
            Debug.Log("We're hitting the terminal at least");
        }
        */
    }
}
