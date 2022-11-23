using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

public class ColorShift : MonoBehaviour
{

    public LaserColor affectedColor;

    public float time = 6f;
    
    //public UnityEvent OnPlayerEnterEvent;

    private Animator animator;

    private AudioSource audioSource;

    private Light2D signalLight;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        signalLight = GetComponentInChildren<Light2D>();
        switch (affectedColor)
        {
            case LaserColor.Red:
                signalLight.color = Color.red;
                break;
            case LaserColor.Blue:
                signalLight.color = Color.blue;
                break;
            case LaserColor.Green:
                signalLight.color = Color.green;
                signalLight.intensity = 3f;
                break;
            case LaserColor.Orange:
                signalLight.color = new Color(255, 95, 0);
                break;
            case LaserColor.Yellow:
                signalLight.color = Color.yellow; //new Color(247, 255, 0);
                signalLight.intensity = .035f;
                break;
            case LaserColor.Purple:
                signalLight.color = new Color(250, 0, 240);
                break;
        }

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(collision.gameObject.GetComponent<PlayerController>().SetColor(affectedColor, time));
            if (animator != null)
            {
                animator.SetTrigger("PlayerTouch");
            }

            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
