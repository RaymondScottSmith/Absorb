using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;

    private int cameraBaseSize = 8;

    private int cameraZoomedOutSize = 28;

    [SerializeField] private float maxLeft, maxRight, maxUp, maxDown;

    private bool isZoomedOut = false;

    private PlayerShrink playerShrink;

    private Camera mainCamera;

    void Start()
    {
        playerShrink = FindObjectOfType<PlayerShrink>();
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerShrink.alive)
        {
            mainCamera.orthographicSize = 8;
            return;
        }
        
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
        }
        

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            isZoomedOut = true;
            mainCamera.orthographicSize = 28;
        }
        else
        {
            mainCamera.orthographicSize = 8;
            isZoomedOut = false;
        }
    }

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
}
