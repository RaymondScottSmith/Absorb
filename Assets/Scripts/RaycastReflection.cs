using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class RaycastReflection : MonoBehaviour
{
    public int reflections;

    public float maxLength;

    private LineRenderer lineRenderer;
    private Vector3 direction;
    private PlayerController player;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && player.readyToLaunch)
        {
            lineRenderer.enabled = true;
            Vector3 endDragPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            Vector3 origin = transform.position;
            Vector3 velocity = (endDragPos - origin);
            velocity.Normalize();
            //Debug.Log("32: velocity: " + velocity);
            // RaycastHit2D hit = Physics2D.Raycast(transform.position, velocity);

            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0,origin);
            
            float remainingLength = maxLength;
            for (int i = 0; i < reflections; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(origin, velocity);
                if (hit.collider != null)
                {
                    
                    if (Vector3.Distance(origin, hit.point)>= remainingLength)
                    {
                        Vector3 newDirection = (Vector3)hit.point - origin;
                        newDirection.Normalize();
                        hit.point = (Vector3)origin + (newDirection * remainingLength);
                    }
                    
                    lineRenderer.positionCount += 1;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    remainingLength -= Vector3.Distance(origin, hit.point);
                    origin = hit.point;
                    velocity = Vector2.Reflect(velocity, hit.normal);
                    velocity.Normalize();
                    //Debug.Log("58: velocity: " + velocity);
                    if (hit.collider.CompareTag("Food"))
                    {
                        //break;
                    }
                    else if (hit.collider.CompareTag("Bounceable") && i == reflections)
                    {
                        break;
                    }
                    else
                    {
                        /*
                        lineRenderer.positionCount += 1;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, (Vector3)origin + velocity * remainingLength);
                        */
                    }

                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.enabled = false;
        }
    }
}