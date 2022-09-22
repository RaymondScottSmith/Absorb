using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    private Transform cameraTransform;

    private Vector3 lastCameraPosition;

    [SerializeField] private Vector3 parallaxMulitiplier;

    private Camera mainCamera;

    private float cameraMinimumSize;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        cameraMinimumSize = mainCamera.GetComponentInParent<CameraController>().cameraMinimumSize;

    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxMulitiplier.x, + deltaMovement.y * parallaxMulitiplier.y);
        lastCameraPosition = cameraTransform.position;
        float bgSizeMultiplier = mainCamera.GetComponentInParent<CameraController>().bgSizeMultiplier;
        float camSize = mainCamera.orthographicSize/cameraMinimumSize;
        //transform.localScale = new Vector3(camSize*bgSizeMultiplier, camSize*bgSizeMultiplier, 1);
    }
    
}
