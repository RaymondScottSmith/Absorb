using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

public class ColorShift : MonoBehaviour
{

    public LaserColor[] affectedColor;

    public float time = 6f;
    
    //public UnityEvent OnPlayerEnterEvent;

    private Animator animator;

    private AudioSource audioSource;

    private Light2D signalLight;

    public bool changingColors;

    public float changeTime = 4f;

    private int currentColor = 0;
    
    private LaserColor changedColor;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        signalLight = GetComponentInChildren<Light2D>();

        if (!changingColors)
        {
            changedColor = affectedColor[0];
            ChangeColor(affectedColor[0]);
        }
        else
        {
            InvokeRepeating("RotateColors",0,changeTime);
        }
        

    }

    private void RotateColors()
    {
        changedColor = affectedColor[currentColor];
        ChangeColor(changedColor);
        currentColor++;
        if (currentColor >= affectedColor.Length)
        {
            currentColor = 0;
        }
    }

    private void ChangeColor(LaserColor color)
    {
        switch (color)
        {
            case LaserColor.Red:
                signalLight.color = Color.red;
                signalLight.intensity = 3f;
                break;
            case LaserColor.Blue:
                signalLight.color = new Color(56/255f, 215/255f, 255/255f);
                signalLight.intensity = 4f;
                break;
            case LaserColor.Green:
                signalLight.color = Color.green;
                signalLight.intensity = 3f;
                break;
            case LaserColor.Orange:
                signalLight.color = new Color(255/255f, 95/255f, 0);
                signalLight.intensity = 4f;
                break;
            case LaserColor.Yellow:
                signalLight.color = Color.yellow; //new Color(247, 255, 0);
                signalLight.intensity = 3f;
                break;
            case LaserColor.Purple:
                signalLight.color = new Color(250/255f, 0, 240/255f);
                signalLight.intensity = 3f;
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
            //StartCoroutine(collision.gameObject.GetComponent<PlayerController>().SetColor(changedColor, time));
            collision.gameObject.GetComponent<PlayerController>().SetPlayerColor(changedColor, time);
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
