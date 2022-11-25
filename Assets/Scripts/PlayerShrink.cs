using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

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

    [SerializeField] private AudioClip loseMusic;

    [SerializeField] private GameObject losePanel;

    public GameObject corpse;

    public List<Shrink> currentlyEating = new List<Shrink>();
    private PlayerController player;

    public bool isBeingDrained;

    [SerializeField] public GameObject EatingPanel;

    [SerializeField] private List<Vector3> hideEdges;

    public bool isHidden;
    
    
    //[SerializeField] private bool isTutorial;

    //private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        isHidden = false;
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

    public bool CheckIfHidden(Collider2D hideCollider)
    {
        foreach (Vector3 edge in hideEdges)
        {
            Vector3 pointToCheck = transform.position + edge;

            Collider2D[] hideColliders = Physics2D.OverlapPointAll(pointToCheck);
            if (!hideColliders.Contains(hideCollider))
                return false;
        }

        return true;
    }
    
    private void FixedUpdate()
    {
        //Calculate the new scale
        if (alive)
        {
            float newScale = scaleValue * currentHealth + minimumScale;
            
            //Adjust scale as health changes
            if (transform.parent == null)
                transform.localScale = new Vector3(newScale, newScale, 0);
            else
            {
                newScale = (scaleValue * currentHealth + minimumScale) / transform.parent.lossyScale.x;
                transform.localScale = new Vector3(newScale, newScale, 0);
            }
            //Adjust color as health changes
            float healthColor = (float)currentHealth / startingHealth; 
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
                RemoveFood(food);
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
        //Debug.Log("Player should die here");
        foreach (Shrink food in currentlyEating)
        {
           Destroy(food.gameObject); 
        }
        alive = false;
        
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        GetComponent<PlayerController>().readyToPlay = false;
        //Destroy(GetComponent<LineRenderer>());
        FindObjectOfType<CameraController>().PlayOneShot(loseMusic);
        //corpse = Instantiate(corpsePrefab, transform.position, corpsePrefab.transform.rotation);
        GetComponent<SpriteRenderer>().sprite = corpseSprite;
        
        gameObject.transform.localScale = Vector3.one;
        GetComponent<CircleCollider2D>().radius = 0.15f;
        if (leaderboard != null)
            yield return leaderboard.SubmitScoreRoutine((int)Mathf.Round(time));
        //timeLabel.SetText("Time: " + (int)Mathf.Round(time));
        
        yield return new WaitForSeconds(1.5f);

        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        
        foreach(AudioSource aS in audioSources)
        {
            aS.Stop();
        }
        losePanel.gameObject.SetActive(true);
        Time.timeScale = 0f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //yield return new WaitForSeconds(1f);
        //Time.timeScale = 0f;
        //gameObject.SetActive(false);
    }

    public void RemoveAllFood()
    {
        List<Shrink> toRemove = new List<Shrink>();
        foreach (Shrink food in currentlyEating)
        {
            
            Destroy(food.gameObject); 
            toRemove.Add(food);
        }

        if (toRemove.Count > 0)
        {
            currentlyEating.RemoveAll(x => toRemove.Contains(x));
        }
        
        Transform[] allChildren = EatingPanel.GetComponentsInChildren<Transform>();
        
        foreach (Transform child in EatingPanel.transform)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }
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
