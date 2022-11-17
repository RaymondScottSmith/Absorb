using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Patrol : MonoBehaviour
{
    public bool isPatrolling = false;

    public Vector2 position1;
    public Vector2 position2;

    public float speed;

    private bool movingToOne = false;

    void Start()
    {
        if (position1 == Vector2.zero)
        {
            position1 = gameObject.transform.position;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isPatrolling)
            return;
        Vector2 currentPos = transform.position;
        if (movingToOne)
        {
            transform.Translate((position1 - currentPos).normalized * speed);
            if (Vector2.Distance(currentPos, position1) < 0.1f)
                movingToOne = false;
        }
        else
        {
            transform.Translate((position2 - currentPos).normalized * speed);
            if (Vector2.Distance(currentPos, position2) < 0.1f)
                movingToOne = true;
        }
        
    }
}
