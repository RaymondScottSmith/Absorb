using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 direction;

    [SerializeField] private Transform startPosition;

    [SerializeField] private GameObject redDotLight;

    private float redR = 1;
    private float redB = 0;
    private float redG = 0;

    private float redA = 0;
    
    
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
        if (redA < 1)
            redA = redA + 0.01f;
        else
        {
            redA = 0;
        }
        lineRenderer.startColor = new Color(redR,redG,redB,redA);
        lineRenderer.endColor = new Color(redR,redG,redB,redA);
        
        Vector3 direction;

    
        direction = transform.right;
        RaycastHit2D hit = Physics2D.Raycast(startPosition.position, direction, 900,
            ~(LayerMask.GetMask("CrewColliders", "Ignore Raycast")));
        
        if (hit.collider != null)
        {
            lineRenderer.positionCount += 1;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
            redDotLight.transform.position = hit.point;
        }
        
    }
}
