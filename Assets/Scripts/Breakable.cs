using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    [SerializeField] private int numSurvivable;

    [SerializeField] private List<Sprite> sprites;
    
    [SerializeField] private Collider2D bounceSurface;

    private SpriteRenderer spriteRenderer;

    private ParticleSystem particleSystem;

    private bool readyToBreak;
    // Start is called before the first frame update
    void Start()
    {
        if (numSurvivable > 0)
        {
            bounceSurface.enabled = true;
        }
        particleSystem = GetComponentInChildren<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (sprites.Count > 0)
        {
            spriteRenderer.sprite = sprites[0];
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            numSurvivable--;
            if (sprites.Count > 0)
            {
                sprites.RemoveAt(0);
                spriteRenderer.sprite = sprites[0];
            }
            if (numSurvivable <= 0)
            {
                bounceSurface.enabled = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (spriteRenderer.enabled == false)
        {
            return;
        }
        if (col.CompareTag("Player"))
        {
            StartCoroutine(Break());
        }
    }

    private IEnumerator Break()
    {
        spriteRenderer.enabled = false;
        particleSystem.Play();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);

    }
}
