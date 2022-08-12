using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    public float power = 5f;

    private LineRenderer lr;

    private Rigidbody2D rb;

    private Vector2 startDragPos;

    private PlayerController player;
    
    
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && player.readyToLaunch)
        {
            //startDragPos = Input.mousePosition;
            startDragPos = transform.position;
            //Debug.Log(Input.mousePosition);
        }

        if (Input.GetMouseButton(0) && player.readyToLaunch)
        {
            lr.enabled = true;
            Vector2 endDragPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            Vector2 velocity = (endDragPos - startDragPos) * power;

            Vector2[] trajectory = Plot(rb, (Vector2)transform.position, velocity, 500);

            lr.positionCount = trajectory.Length;

            Vector3[] positions = new Vector3[trajectory.Length];

            for (int j = 0; j < trajectory.Length; j++)
            {
                positions[j] = trajectory[j];
            }
            
            lr.SetPositions(positions);

        }
        else
        {
            lr.enabled = false;
        }
        
    }

    private Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];
        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 moveStep = velocity * timestep;

        for (int j = 0; j < steps; j++)
        {
            pos += moveStep;
            results[j] = pos;
        }

        return results;
    }
}
