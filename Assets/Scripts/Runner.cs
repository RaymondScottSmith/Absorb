using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Runner : MonoBehaviour
{
    public Vector2 destination;
    private Vector2 waypoint;
    private int currentFloor;
    private int destinationFloor;

    [SerializeField] private float speed = 5f;

    private bool isJourneying = false;

    private SpriteRenderer spriteRenderer;

    private Animator runnerAnimator;

    private static readonly int Running = Animator.StringToHash("Running");
    private bool isAtWrongHeight = false;

    private ClimbState climbState = ClimbState.NotClimbing;
    

    private SpawnManager spawnManager;

    // Start is called before the first frame update

    public void StartJourney(Vector2 dest, int startingFloor, int destFloor, SpawnManager sm)
    {
        destinationFloor = destFloor;
        currentFloor = startingFloor;
        spriteRenderer = GetComponent<SpriteRenderer>();
        runnerAnimator = GetComponent<Animator>();
        isJourneying = true;
        destination = dest;
        runnerAnimator.SetBool(Running, true);

        climbState = ClimbState.NotClimbing;

        if (transform.position.x > 0)
            spriteRenderer.flipX = true;
        else if (transform.position.x < 0)
            spriteRenderer.flipX = false;
        spawnManager = sm;

        if (destinationFloor != currentFloor)
        {
            Debug.Log("Finding Ladder");
            waypoint = FindClosestLadder();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (isJourneying)
        {
            if (climbState == ClimbState.NotClimbing)
            {
                //If we are at the correct floor
                if (destinationFloor == currentFloor)
                {
                    if (CloseEnough(transform.position, destination))
                    {
                        runnerAnimator.SetBool(Running, false);
                        Destroy(gameObject);
                    }
                    else if (transform.position.x > destination.x)
                    {
                        spriteRenderer.flipX = true;
                        transform.Translate(Vector2.left * Time.deltaTime * speed);
                    }
                    else
                    {
                        spriteRenderer.flipX = false;
                        transform.Translate(Vector2.right * Time.deltaTime * speed);
                    }

                    return;
                }
            
                // if arrived at waypoint
                if (CloseEnough(waypoint.x, transform.position.x))
                {
                    transform.position = waypoint;
                    ArrivedAtLadder();
                }
                else if (transform.position.x > waypoint.x)
                {
                    spriteRenderer.flipX = true;
                    transform.Translate(Vector2.left * Time.deltaTime * speed);
                }
                else
                {
                    spriteRenderer.flipX = false;
                    transform.Translate(Vector2.right * Time.deltaTime * speed);
                }
            }
            else if(climbState == ClimbState.ClimbingDown)
                transform.Translate(Vector2.down * Time.deltaTime * speed);
            else if(climbState == ClimbState.ClimbingUp)
                transform.Translate(Vector2.up * Time.deltaTime * speed);

        }
    }

    private bool CloseEnough(Vector2 firstPos, Vector2 secondPos)
    {
        if (Vector2.Distance(firstPos, secondPos) < 0.5f)
            return true;
        return false;
    }

    private bool CloseEnough(float firstValue, float secondValue)
    {
        if (MathF.Abs(firstValue - secondValue) < 0.5f)
            return true;
        return false;
    }

    private void ArrivedAtLadder()
    {
        //If we need to go down
        if (destinationFloor < currentFloor)
        {
            climbState = ClimbState.ClimbingDown;
        }
        else
        {
            climbState = ClimbState.ClimbingUp;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (climbState)
        {
            case ClimbState.NotClimbing:
                break;
            case ClimbState.ClimbingUp:
                if (col.gameObject.CompareTag("LadderTop"))
                {
                    transform.position = col.transform.position;
                    climbState = ClimbState.NotClimbing;
                    currentFloor++;
                    if (destinationFloor != currentFloor)
                        waypoint = FindClosestLadder();
                }
                break;
            case ClimbState.ClimbingDown:
                if (col.gameObject.CompareTag("LadderBottom"))
                {
                    transform.position = col.transform.position;
                    climbState = ClimbState.NotClimbing;
                    currentFloor--;
                    if (destinationFloor != currentFloor)
                        waypoint = FindClosestLadder();
                }
                break;
        }

    }

    private Vector2 FindClosestLadder()
    {
        Transform targetLadder = new RectTransform();
        if (currentFloor > destinationFloor)
        {
            Debug.Log("Current Floor greater than destination");
            float dist = 9000f;
            foreach (GameObject ladderTop in spawnManager.ladderTops)
            {
                if (Vector2.Distance(ladderTop.transform.position, transform.position) < dist)
                {
                    dist = Vector2.Distance(ladderTop.transform.position, transform.position);
                    targetLadder = ladderTop.transform;
                }
            }
            
        }

        if (currentFloor < destinationFloor)
        {
            float dist = 9000f;
            foreach (GameObject ladder in spawnManager.ladderBottoms)
            {
                if (Vector2.Distance(ladder.transform.position, transform.position) < dist)
                {
                    dist = Vector2.Distance(ladder.transform.position, transform.position);
                    targetLadder = ladder.transform;
                }
            }
        }
        Debug.Log(targetLadder.position);
        return targetLadder.position;
    }

    public enum ClimbState
    {
        ClimbingUp,
        ClimbingDown,
        NotClimbing
    }

}
