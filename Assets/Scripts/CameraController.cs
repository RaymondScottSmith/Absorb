using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;

    [SerializeField]
    private float cameraMinimumSize = 8f;
    [SerializeField]
    private float cameraMaximumSize = 19f;

    //private bool isZoomedOut = false;

    private PlayerShrink playerShrink;

    private Camera mainCamera;

    private Vector3 cameraOldPosition;

    [SerializeField] private Vector3 cameraCenterPosition;

    private AudioSource audioSource;

    [SerializeField] private AudioClip loseSound;

    private bool playedDeathSound;

    [SerializeField] private GameObject pausePanel;

    void Start()
    {
        playerShrink = FindObjectOfType<PlayerShrink>();
        mainCamera = GetComponent<Camera>();
        audioSource = GetComponent<AudioSource>();
        playedDeathSound = false;
        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(true);
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
            Vector3 shrinkPos = playerShrink.transform.position;
            mainCamera.orthographicSize = 8;
            mainCamera.transform.position = new Vector3(shrinkPos.x, shrinkPos.y, -10);
            return;
        }

        transform.position = playerShrink.transform.position + (Vector3.forward * -10);

        float scrollValue = -Input.mouseScrollDelta.y;
        mainCamera.orthographicSize += scrollValue/2f;

        if (mainCamera.orthographicSize > cameraMaximumSize)
        {
            mainCamera.orthographicSize = cameraMaximumSize;
        }else if (mainCamera.orthographicSize < cameraMinimumSize)
        {
            mainCamera.orthographicSize = cameraMinimumSize;
        }

        /*
        if (!isZoomedOut)
        {
            float horizInput = Input.GetAxis("Horizontal");
            float vertInput = Input.GetAxis("Vertical");

            if (horizInput != 0 || vertInput != 0)
            {
                float translateMultiplier = scrollSpeed * Time.deltaTime;
                transform.Translate(new Vector3(horizInput * translateMultiplier, vertInput * translateMultiplier, 0));
                StayInBoundaries();
            }
            cameraOldPosition = mainCamera.transform.position;
        }
        

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (!isZoomedOut)
                cameraOldPosition = mainCamera.transform.position;
            
            isZoomedOut = true;
            mainCamera.orthographicSize = 28;
            mainCamera.transform.position = cameraCenterPosition;
        }
        else
        {
            mainCamera.orthographicSize = 8;
            mainCamera.transform.position = cameraOldPosition;
            isZoomedOut = false;
        }
        */
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

    public void Unpause()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }
    
    */
}
