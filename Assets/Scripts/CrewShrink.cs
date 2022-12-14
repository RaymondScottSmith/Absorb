using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrewShrink : Shrink
{
    [SerializeField] private Animator myAnimator;
    [SerializeField] private GameObject eatSymbolPrefab;
    [SerializeField] private Collider2D moveCollider;
    private AudioSource audioSource;

    [SerializeField] private AudioClip[] deathSounds;

    private Rigidbody2D rb;

    public int keyValue = 0;

    private PlayerShrink playerShrink;

    [SerializeField] private float eatenEventDelay;
    public UnityEvent WhenEaten;

    void Start()
    {
        beingEaten = false;
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }
    public override void AttachToEater(GameObject eater)
    {
        audioSource.Stop();
        if (deathSounds.Length > 0 && audioSource != null)
        {
            int randSound = Random.Range(0, deathSounds.Length);
            audioSource.PlayOneShot(deathSounds[randSound]);
        }

        StartCoroutine(WhenEatenCoroutine());
        beingEaten = true;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.gravityScale = 0;
        playerShrink = eater.GetComponent<PlayerShrink>();
        if (playerShrink != null)
            playerShrink.AddFood(this);
        if (moveCollider != null)
            moveCollider.enabled = false;
        float eaterRadius = eater.transform.localScale.x / 2;
        attachPoint = new Vector3(Random.Range(-eaterRadius, eaterRadius), Random.Range(-eaterRadius, eaterRadius), 0);
        GetComponent<Collider2D>().enabled = false;
        attachedEater = eater.transform;
        beingEaten = true;
        currentHealth = startingHealth;
        if (myAnimator != null)
            myAnimator.SetTrigger("Dying");
        StartCoroutine(BeEaten(eater.GetComponent<Shrink>()));

        eatSymbolPrefab = Instantiate(eatSymbolPrefab, eater.GetComponent<PlayerShrink>().EatingPanel.transform);
        
    }

    private IEnumerator WhenEatenCoroutine()
    {
        yield return new WaitForSeconds(eatenEventDelay);
        WhenEaten.Invoke();
    }

    protected override IEnumerator BeEaten(Shrink eater)
    {
        //Decrease health every second until it's 0
        while (currentHealth > 0)
        {
            eater.GainHealth(eatPerSecond);
            currentHealth-=damageOverTime;
            yield return new WaitForSeconds(1);
        }
        playerShrink.RemoveFood(this);
        Destroy(eatSymbolPrefab);
        Destroy(gameObject);
    }
}
