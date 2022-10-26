using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class HunterTurret : MonoBehaviour
{
    [SerializeField]
    private Vector2 areaMins, areaMaxes;
    

    private Vector3 targetPos;

    [SerializeField]
    private float searchSpeed = 4f;

    [SerializeField] private float detectedSpeed = 8f;

    private HuntState myState;

    [SerializeField] private float searchSize = 7f;

    private PlayerShrink player;

    private Light2D searchLight;

    private SpriteRenderer sp;

    private bool isInCollider;

    private bool isLooking = false;

    private AudioSource audio;
    

    private bool isShooting;

    [SerializeField] private AudioClip gunfire;

    [SerializeField] private AudioClip alarmSound;

    [SerializeField] private GameObject gunfirePrefab;
    // Start is called before the first frame update
    void Start()
    {
        targetPos = FindRandomPos();
        myState = HuntState.Searching;
        GetComponent<CircleCollider2D>().radius = searchSize;
        searchLight = GetComponentInChildren<Light2D>();
        searchLight.pointLightOuterRadius = searchSize;
        searchLight.pointLightInnerRadius = searchSize;
        sp = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        isShooting = false;
    }

    private IEnumerator Shoot()
    {
        isShooting = true;
        yield return new WaitForSeconds(0.5f);
        if (!player.isHidden)
        {
            Instantiate(gunfirePrefab, transform.position, gunfirePrefab.transform.rotation);
            audio.PlayOneShot(gunfire);

            if (Vector3.Distance(player.transform.position, transform.position) < 0.5f)
            {
                player.TakeDamage(20);
            }
        }
        isShooting = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        switch (myState)
        {
            case HuntState.Searching:
                Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos, searchSpeed * Time.deltaTime);
                transform.position = newPos;
                searchLight.intensity = 2.3f;
                searchLight.color = Color.white;
                if (Vector3.Distance(transform.position, targetPos) < 0.5f)
                    targetPos = FindRandomPos();
                break;
            case HuntState.Detected:
                if (!player.alive)
                    return;
                Vector3 playerPos = Vector3.MoveTowards(transform.position, player.transform.position, detectedSpeed * Time.deltaTime);
                transform.position = playerPos;
                searchLight.intensity = 8;
                searchLight.color = Color.red;
                if (Vector3.Distance(transform.position, player.transform.position) < 0.5f && !player.isHidden)
                {
                    if (!isShooting)
                        StartCoroutine(Shoot());
                }
                    
                /*
                if (player.isHidden)
                {
                    if (!isLooking)
                    {
                        isLooking = true;
                        StartCoroutine(TurnOffTarget());
                    }
                        
                    break;
                }
                */
                sp.enabled = true;
                //Vector3 playerPos = Vector3.MoveTowards(transform.position, player.transform.position, detectedSpeed * Time.deltaTime);
                //transform.position = playerPos;
                break;
        }
    }
    
    private Vector3 FindRandomPos()
    {
        float xPos = Random.Range(areaMins.x, areaMaxes.x);

        float yPos = Random.Range(areaMins.y, areaMaxes.y);
        //Debug.Log(xPos + ", " + yPos);
        return new Vector3(xPos, yPos, transform.position.z);
    }

    private IEnumerator TurnOffTarget()
    {
        isLooking = true;
        yield return new WaitForSeconds(2f);
        
        if (player.isHidden)
        {
            Debug.Log("Should stop looking");
            myState = HuntState.Searching;
            sp.enabled = false;
            targetPos = FindRandomPos();
            isLooking = false;
        }

    }
        
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInCollider = true;
            player = collision.gameObject.GetComponent<PlayerShrink>();
            if (player.isHidden)
            {
                if (myState == HuntState.Detected && !isLooking)
                {
                    //Debug.Log("Hitting this thing");
                    isLooking = true;
                    StartCoroutine(TurnOffTarget());
                }
                return;
            }
            else if (myState != HuntState.Detected)
            {
                audio.PlayOneShot(alarmSound);
                isLooking = false;
                myState = HuntState.Detected;
                
                //Debug.Log("PLAYER DETECTED!");
            }
            else
            {
                isLooking = false;
                myState = HuntState.Detected;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInCollider = false;
        }
    }
    
    private enum HuntState
    {
        Off,
        Searching,
        Detected,
        Shooting
    }
    
    
}
