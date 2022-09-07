using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 direction;

    [SerializeField] private Transform startPosition;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0,startPosition.position);
        RaycastHit2D hit = Physics2D.Raycast(startPosition.position, Vector3.right, 900,
            ~(LayerMask.GetMask("CrewColliders", "Ignore Raycast")));
        if (hit.collider != null)
        {
            lineRenderer.positionCount += 1;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
        }
        
    }
}
