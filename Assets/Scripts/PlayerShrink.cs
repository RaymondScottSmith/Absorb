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
    [SerializeField] private Sprite corpseSprite;

    [SerializeField] private GameObject losePanel;

    public GameObject corpse;

    public List<Shrink> currentlyEating = new List<Shrink>();
    private PlayerController player;

    public bool isBeingDrained;
    
    
    //[SerializeField] private bool isTutorial;

    //private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        isBeingDrained = false;
        player = GetComponent<PlayerController>();
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

        StartCoroutine(HandleDrain());
    }

    public bool IsBeingDrained
    {
        get => isBeingDrained;
        set => isBeingDrained = value;
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


    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        //Calculate the new scale
        if (alive)
        {
            float newScale = scaleValue * currentHealth + minimumScale;
            
            //Adjust scale as health changes
            transform.localScale = new Vector3(newScale, newScale, 0);
            //Adjust color as health changes
            float healthColor = (float)currentHealth / startingHealth;
            spriteRenderer.color = playerColor.Evaluate(healthColor);
        }
            
        

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

    public void DeleteKey(int keyNumber)
    {
        List<Shrink> toRemove = new List<Shrink>();
        foreach (Shrink food in currentlyEating)
        {
            if (keyNumber == food.GetComponent<CrewShrink>().keyValue)
            {
                Destroy(food.gameObject);
                toRemove.Add(food);
            }
        }

        if (toRemove.Count > 0)
        {
            currentlyEating.RemoveAll(x => toRemove.Contains(x));
        }
    }
    
    private IEnumerator LoseHealth()
    {
        //Decrease health every second until it's 0
        while (currentHealth > 0)
        {
            if (player.readyToPlay)
            {
                currentHealth--;
            }

            if (isBeingDrained)
                yield return new WaitForSeconds(0.1f);
            else
            {
                yield return new WaitForSeconds(1);
            }
            
        }
        Die();
    }

    private IEnumerator HandleDrain()
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(.1f);
            if (!isBeingDrained || currentHealth <= 0) continue;
            currentHealth -= 1;
            
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
        foreach (Shrink food in currentlyEating)
        {
           Destroy(food.gameObject); 
        }
        alive = false;
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        //corpse = Instantiate(corpsePrefab, transform.position, corpsePrefab.transform.rotation);
        GetComponent<SpriteRenderer>().sprite = corpseSprite;
        gameObject.transform.localScale = Vector3.one * 2f;
        GetComponent<CircleCollider2D>().radius = 0.15f;
        if (leaderboard != null)
            yield return leaderboard.SubmitScoreRoutine((int)Mathf.Round(time));
        //timeLabel.SetText("Time: " + (int)Mathf.Round(time));
        losePanel.GetComponent<LosePanel>().GameOver((int)Mathf.Round(time));
        //yield return new WaitForSeconds(1f);
        //Time.timeScale = 0f;
        //gameObject.SetActive(false);
    }
    
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            //Debug.Log("Should be attaching");
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
