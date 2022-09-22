using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;

    [SerializeField]
    public float cameraMinimumSize = 8f;
    [SerializeField]
    private float cameraMaximumSize = 19f;

    [SerializeField] private float scrollMultiplier = 0.5f;

    private PlayerShrink playerShrink;

    private Camera mainCamera;

    private Vector3 cameraOldPosition;

    [SerializeField] private Vector3 cameraCenterPosition;

    private AudioSource audioSource;

    [SerializeField] private AudioClip loseSound;

    private bool playedDeathSound;

    [SerializeField] private GameObject pausePanel;

    [SerializeField] private Slider scrollSlider;
    
    [SerializeField] private GameObject skyBox;

    public float bgSizeMultiplier = 1f;

    private bool isScrollSlider;

    private Animator animator;

    void Start()
    {
        playerShrink = FindObjectOfType<PlayerShrink>();
        mainCamera = GetComponentInChildren<Camera>();
        audioSource = GetComponentInChildren<AudioSource>();
        playedDeathSound = false;
        pausePanel.SetActive(false);
        animator = GetComponentInChildren<Animator>();

        if (PlayerPrefs.HasKey("ScrollMultiplier"))
        {
            scrollMultiplier = PlayerPrefs.GetFloat("ScrollMultiplier");
        }

        if (PlayerPrefs.HasKey("ScrollSlider"))
        {
            isScrollSlider = PlayerPrefs.GetInt("ScrollSlider") == 1;
        }

        scrollSlider.transform.parent.gameObject.SetActive(isScrollSlider);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(true);
            playerShrink.GetComponent<PlayerController>().readyToPlay = false;
            Time.timeScale = 0f;
        }
        
        
        if (!playerShrink.alive)
        {
            if (!playedDeathSound)
            {
                playedDeathSound = true;
                audioSource.Stop();
                audioSource.PlayOneShot(loseSound);
            }
            Vector3 corpsePos = playerShrink.corpse.transform.position;
            mainCamera.orthographicSize = 8;
            mainCamera.transform.position = new Vector3(corpsePos.x, corpsePos.y, -10);
            return;
        }

        transform.position = playerShrink.transform.position + (Vector3.forward * -10);

        if (isScrollSlider)
        {
            mainCamera.orthographicSize = scrollSlider.value;
        }
        else
        {
            float scrollValue = -Input.mouseScrollDelta.y;
            mainCamera.orthographicSize += scrollValue * scrollMultiplier;
        }

        
        
        if (mainCamera.orthographicSize > cameraMaximumSize)
        {
            mainCamera.orthographicSize = cameraMaximumSize;
        }else if (mainCamera.orthographicSize < cameraMinimumSize)
        {
            mainCamera.orthographicSize = cameraMinimumSize;
        }
        
        if (skyBox != null)
        {
            float camSize = mainCamera.orthographicSize/cameraMinimumSize;
            skyBox.transform.localScale = new Vector3(camSize * bgSizeMultiplier, camSize * bgSizeMultiplier, 1);
        }
    }


    public void ShakeScreen()
    {
        animator.SetTrigger("ShakeIt");
    }
    /*
    private void StayInBoundaries()
    {
        //Vector3 camPosition = transform.position;
        Vector3 adjustedPosition = transform.position;
        if (adjustedPosition.x > maxRight)
            adjustedPosition = new Vector3(maxRight, adjustedPosition.y, -10);
        else if (adjustedPosition.x < maxLeft)
            adjustedPosition = new Vector3(maxLeft, adjustedPosition.y, -10);
        
        if (adjustedPosition.y > maxUp)
            adjustedPosition = new Vector3(adjustedPosition.x, maxUp, -10);
        else if (adjustedPosition.y < maxDown)
            adjustedPosition = new Vector3(adjustedPosition.x, maxDown, -10);

        transform.position = adjustedPosition;
    }
*/
    public void Unpause()
    {
        playerShrink.GetComponent<PlayerController>().readyToPlay = true;
        playerShrink.GetComponent<PlayerController>().readyToLaunch = false;
        
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        
    }
    
    
}
