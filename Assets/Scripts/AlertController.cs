using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlertController : MonoBehaviour
{
    private PlayerController player;

    private new Camera camera;

    [SerializeField]
    private GameObject topAlert, bottomAlert, leftAlert, rightAlert;

    private bool looking = false;
    // Start is called before the first frame update
    void Start()
    {
        
        player = FindObjectOfType<PlayerController>();

        camera = FindObjectOfType<Camera>();
        ResetAlerts();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player.GetComponent<Renderer>().isVisible)
        {
            looking = true;
        }
        if (looking)
        {
            if (!player.GetComponent<Renderer>().isVisible)
            {
                FindPlayer();
            }
            else
            {
                ResetAlerts();
            }
        }
        
    }

    void FindPlayer()
    {
        Vector3 playerPos = player.gameObject.transform.position;
        Vector3 cameraPos = camera.gameObject.transform.position;

        ResetAlerts();
        //If X has the biggest difference between camera and player
        if (Mathf.Abs(playerPos.x - cameraPos.x) > Mathf.Abs(playerPos.y - cameraPos.y))
        {
            if (playerPos.x > cameraPos.x)
                leftAlert.SetActive(true);
            else
            {
                rightAlert.SetActive(true);
            }
        }
        else
        {
            if (playerPos.y > cameraPos.y)
                topAlert.SetActive(true);
            else
            {
                bottomAlert.SetActive(true);
            }
        }
    }

    void ResetAlerts()
    {
        rightAlert.SetActive(false);
        leftAlert.SetActive(false);
        topAlert.SetActive(false);
        bottomAlert.SetActive(false);
    }
}
