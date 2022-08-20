using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewShrink : Shrink
{
    [SerializeField] private Animator myAnimator;
    [SerializeField] private GameObject eatSymbolPrefab;
    [SerializeField] private Collider2D moveCollider;

    private Rigidbody2D rb;

    public int keyValue = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public override void AttachToEater(GameObject eater)
    {
        rb.constraints = RigidbodyConstraints2D.None;
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

        eatSymbolPrefab = Instantiate(eatSymbolPrefab, LevelManager.Instance.eatingPanel.transform);
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
        Destroy(eatSymbolPrefab);
        Destroy(gameObject);
    }
}
