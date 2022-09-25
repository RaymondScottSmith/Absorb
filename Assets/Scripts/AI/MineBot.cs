using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;

public class MineBot : MonoBehaviour
{

    private TempAI myAI;

    private bool isPursuing;
    private bool isInRange;

    private GameObject target;

    [SerializeField] private SpriteRenderer midBar;
    [SerializeField] private SpriteRenderer circleSprite;
    [SerializeField] private Light2D midLight;
    [SerializeField] private Light2D circleLight;
    private bool isExploding;

    private float currentDistance;

    private Color passiveColor;

    private AudioSource audioSource;

    private Animator animator;

    [SerializeField] private int explosionDamage;

    [SerializeField] private AudioClip explosionSound;

    private CameraController camController;
    
    // Start is called before the first frame update
    void Start()
    {
        myAI = GetComponent<TempAI>();
        passiveColor = midBar.color;
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        camController = FindObjectOfType<CameraController>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            target = collision.gameObject;
            if (!isInRange)
                isInRange = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            isPursuing = false;
            myAI.StopPursuit();
            UpdateColors(passiveColor);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isInRange && !isExploding)
        {
            if (!isPursuing)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, target.transform.position - transform.position);

                if (hit.collider.CompareTag("Player"))
                {
                    StartBeeping();
                    isPursuing = true;
                    myAI.StartMoving(hit.collider.transform);
                    UpdateColors(Color.yellow);
                }
            }
            else
            {
                currentDistance = Vector3.Distance(transform.position, target.transform.position);
                UpdateColors(Color.Lerp(Color.red, Color.yellow, currentDistance/10f));
            }
        }

    }

    private void StartBeeping()
    {
        StartCoroutine(Beep());
    }

    private IEnumerator Beep()
    {
        float timeDist = currentDistance / 20f;
        //Debug.Log(timeDist);
        yield return new WaitForSeconds(timeDist);

        if (!isPursuing || isExploding)
            yield break;
        audioSource.Play();
        StartCoroutine(Beep());
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && !isExploding)
        {
            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            isExploding = true;
            isPursuing = false;
            myAI.StopPursuit();
            player.TakeDamage(explosionDamage,null);
            player.ChangeDirection(col.contacts[0].normal * 10f);
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        animator.SetTrigger("Explode");
        audioSource.Stop();
        audioSource.PlayOneShot(explosionSound);
        camController.ShakeScreen();
        yield return new WaitForSeconds(1.3f);
        Destroy(gameObject);
    }

    private void UpdateColors(Color newColor)
    {
        midBar.color = newColor;
        midLight.color = newColor;
        circleLight.color = newColor;
        circleSprite.color = newColor;
    }
}
