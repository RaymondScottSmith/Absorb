using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;

    [SerializeField]
    public float cameraMinimumSize = 8f;
    [SerializeField]
    private float cameraMaximumSize = 19f;

    [SerializeField] private float bossCameraSize = 12.75f;
    [SerializeField] private float bossCameraMin = 11.6f;

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

    public bool isBossChamber;

    public GameObject focusObject;

    [SerializeField] private float bossChamberYPos = -14.7f;

    private AudioSource musicPlayer;

    public bool huntingMode = false;

    [SerializeField]
    private Vector2 huntingMins;
    [SerializeField]
    private Vector2 huntingMaxes;

    [SerializeField] private bool cameraIsFixed = false;

    void Awake()
    {
        
    }
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
        
        playerShrink = FindObjectOfType<PlayerShrink>();
        mainCamera = GetComponentInChildren<Camera>();
        audioSource = GetComponentInChildren<AudioSource>();
        playedDeathSound = false;
        pausePanel.SetActive(false);
        animator = GetComponentInChildren<Animator>();
        ResetFocusToPlayer();

        if (PlayerPrefs.HasKey("ScrollMultiplier"))
        {
            scrollMultiplier = PlayerPrefs.GetFloat("ScrollMultiplier");
        }

        if (PlayerPrefs.HasKey("ScrollSlider"))
        {
            isScrollSlider = PlayerPrefs.GetInt("ScrollSlider") == 1;
        }

        if (scrollSlider != null)
            scrollSlider.transform.parent.gameObject.SetActive(isScrollSlider);
        
        //StartHuntingMode(huntingMins, huntingMaxes);
    }

    public void ResetFocusToPlayer()
    {
        focusObject = playerShrink.gameObject;
    }

    public void ChangeMusic(AudioClip newSong)
    {
        musicPlayer.Stop();
        musicPlayer.clip = newSong;
        musicPlayer.Play();
    }

    public void StopMusic()
    {
        musicPlayer.Stop();
    }

    public void PlayOneShot(AudioClip oneShot)
    {
        musicPlayer.Stop();
        musicPlayer.PlayOneShot(oneShot);
    }

    public void FocusOnTarget(GameObject target)
    {
        focusObject = target;
    }

    public void StartHuntingMode(Vector2 areaMin, Vector2 areaMax)
    {
        huntingMins = areaMin;
        huntingMaxes = areaMax;
        huntingMode = true;
        GetComponent<HuntingCamera>().StartHunting(areaMin, areaMax);
    }

    public void StopHuntingMode()
    {
        huntingMode = false;
        GetComponent<HuntingCamera>().StopHunting();
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraIsFixed)
            return;

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

        if (huntingMode)
        {
            return;
        }

        if (!isBossChamber)
            transform.position = focusObject.transform.position + (Vector3.forward * -10);
        else
        {
            transform.position = new Vector3(focusObject.transform.position.x, bossChamberYPos, -10f);
            //Debug.Log(focusObject.transform.position.x);
        }

        if (isScrollSlider)
        {
            mainCamera.orthographicSize = scrollSlider.value;
        }
        else
        {
            float scrollValue = -Input.mouseScrollDelta.y;
            mainCamera.orthographicSize += scrollValue * scrollMultiplier;
        }

        if (isBossChamber)
        {
            if (mainCamera.orthographicSize > bossCameraSize)
                mainCamera.orthographicSize = bossCameraSize;
            else if (mainCamera.orthographicSize < bossCameraMin)
            {
                mainCamera.orthographicSize = bossCameraMin;
            }
        }
        else if (mainCamera.orthographicSize > cameraMaximumSize)
        {
            mainCamera.orthographicSize = cameraMaximumSize;
        }
        else if (mainCamera.orthographicSize < cameraMinimumSize)
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
        //Fix after refactoring camera animations
    }

    public void LoadScene(int sceneNumber)
    {
        Unpause();
        ScreenTransition.Instance.LoadScene(sceneNumber);
    }
    
    public void Unpause()
    {
        playerShrink.GetComponent<PlayerController>().readyToPlay = true;
        playerShrink.GetComponent<PlayerController>().readyToLaunch = false;
        
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        
    }
    
    
}
