using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Pathfinding;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Jetpack : MonoBehaviour
{

    [SerializeField] private float updateTimer = 0.5f;
    [SerializeField] private Transform gunMuzzle;

    [SerializeField] private LineRenderer fireLine;

    [SerializeField] private float targetLockTime = 0.5f;

    [SerializeField] private float fireTime = 1f;
    [SerializeField] private float xFireDistance = 5f;

    [SerializeField] private GameObject thruster;

    public GameObject bulletPrefab;
    
    private Seeker seeker;

    //Turns on/off the AI
    public bool isPursuing = false;
    
    public Vector3 targetPosition;
    
    public Path path;

    private PlayerController player;
    
    private int currentWaypoint = 0;

    private bool reachedEndOfPath;
    
    public float nextWaypointDistance = 3;
    
    public float speed = 2;

    private float previousY;

    private float currentY;

    private Animator animator;

    private AudioSource myAudio;

    private bool audioPlaying;

    private bool isFiringGun;
    // Start is called before the first frame update
    void Start()
    {
        currentY = transform.position.y;
        previousY = currentY;
        seeker = GetComponent<Seeker>();
        player = FindObjectOfType<PlayerController>();
        InvokeRepeating("UpdatePath", updateTimer, updateTimer);
        animator = GetComponent<Animator>();
        myAudio = GetComponent<AudioSource>();
    }

    public void StartPursuit()
    {
        isPursuing = true;
    }

    public void StopPursuit()
    {
        isPursuing = false;
        animator.SetBool("IsShooting", false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isPursuing)
            return;
        currentY = transform.position.y;
        //GoingUp
        if (Mathf.Abs(currentY - previousY) > 0.05f)
        {
            
                
            if (currentY > previousY)
            {
                animator.SetBool("Rising", true);
            }
            else if (currentY < previousY)
            {
                animator.SetBool("Rising", false);
            }
        }

        previousY = currentY;

        if (Mathf.Abs(currentY - player.transform.position.y) < 1f)
        {
            animator.SetBool("IsShooting", true);
            if (!isFiringGun)
                StartCoroutine(ShootGun());
        }
        else
        {
            animator.SetBool("IsShooting", false);
        }
    }

    private IEnumerator ShootGun()
    {
        if (transform.position.x < player.transform.position.x)
        {
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f,0f,0f));
        }
        else
        {
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f,180f,0f));
        }
        
        isFiringGun = true;
        yield return new WaitForSeconds(targetLockTime);
        GetComponentInChildren<JetpackGun>().StartFiring();
        FireBullet();
        yield return new WaitForSeconds(fireTime);
        GetComponentInChildren<JetpackGun>().StopFiring();
        isFiringGun = false;
    }
    
    private void UpdatePath()
    {
        if (targetPosition == null || !isPursuing)
        {
            return;
        }

        Vector3 playerPos = player.transform.position;
        //Left of player
        if (transform.position.x < playerPos.x)
        {
            float newX;
            RaycastHit2D hit = Physics2D.Raycast(playerPos, Vector3.left, xFireDistance,  ~LayerMask.GetMask("Crew"));
            if (hit.distance > xFireDistance)
            {
                newX = playerPos.x - (hit.distance - 1f);
            }
            else
            {
                newX = playerPos.x - xFireDistance;
            }
            if (!isFiringGun)
                transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f,0f,0f));
            targetPosition = new Vector3(newX, playerPos.y, playerPos.z);
        }
        else
        {
            float newX;
            RaycastHit2D hit = Physics2D.Raycast(playerPos, Vector3.right, xFireDistance,  ~LayerMask.GetMask("Crew"));
            if (hit.distance > xFireDistance)
            {
                newX = playerPos.x + (hit.distance - 1f);
            }
            else
            {
                newX = playerPos.x + xFireDistance;
            }
            if (!isFiringGun)
                transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f,180f,0f));
            targetPosition = new Vector3(newX, playerPos.y, playerPos.z);
        }
        seeker.StartPath(transform.position, targetPosition, OnPathComplete);
    }
    
    public void OnPathComplete(Path p)
    {
        //Debug.Log("We have a path back. Did it have an error? " + p.error);
        if (!p.error)
        {
            path = p;
            //Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (path == null || !isPursuing)
        {
            //We have no path, so don't do anything
            return;
        }

        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedEndOfPath = false;
        float distanceToWaypoint;
        if (!audioPlaying)
        {
            myAudio.volume = 0.5f;
            audioPlaying = true;
        }
        while (true)
        {
            //For maximum performance change to using squared distance
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                //Check if there is another waypoint, or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    //Set a status variable to indicate that the agent has reached the end of the path.
                    //Do anything needed when reaching the end of the path
                    reachedEndOfPath = true;
                    myAudio.volume = 0.2f;
                    audioPlaying = false;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        //Slow down smoothly upon approaching the end of the path
        //This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        //Direction to the next waypoint
        //Normalize it so that it has a length of 1 world unit
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        //Multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * speed * speedFactor;
        
        //Move the agent using CharacterController
        //SimpleMove takes a velocity in meters/second. So don't multiply by Time.deltaTime
        //controller.SimpleMove(velocity);
        //If writing a 2D game should remove CharacterController code above and instead move transform directly
        transform.position += velocity * Time.deltaTime;
    }

    public void Die()
    {
        thruster.SetActive(false);
        isPursuing = false;
        animator.SetBool("IsShooting", false);
    }

    public void FireBullet()
    {
        Instantiate(bulletPrefab, gunMuzzle.position, transform.rotation);
    }
    
}
