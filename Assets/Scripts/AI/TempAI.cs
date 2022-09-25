using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TempAI : MonoBehaviour
{
    public Transform targetPosition;

    private Seeker seeker;

    public Path path;

    public float speed = 2;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public bool reachedEndOfPath;

    public bool isPursuing;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        
        //This is only for 3D pathfinding
        //controller = GetComponent<CharacterController>();
        
        
        // Start to calculate a new path to the targetPosition object, return the result to the OnPathComplete method.
        // Path requests are asynchronous, so when the OnPathComplete method is called depends on how long it
        // takes to calculate the path. Usually it is called the next frame.
        //seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
        
        InvokeRepeating("UpdatePath", 0.5f, 0.5f);
    }

    public void StartMoving(Transform target)
    {
        targetPosition = target;
        isPursuing = true;
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }
    private void UpdatePath()
    {
        if (targetPosition == null || !isPursuing)
        {
            return;
        }
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }

    public void StopPursuit()
    {
        targetPosition = null;
        isPursuing = false;
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
}
