using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingCollider : MonoBehaviour
{
    public int segments = 16;

    public float xradius = 5f;

    public float yradius = 5f;

    public float edgeRadius = 1f;

    private EdgeCollider2D edge;
    // Start is called before the first frame update
    void Start()
    {
        edge = GetComponent<EdgeCollider2D>();
        edge.edgeRadius = edgeRadius;
        float x;
        float y;
        float z;
        
        float angle = 20f;
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;
            
            //line.SetPosition (i,new Vector3(x,y,0) );

            points.Add(new Vector2(x,y));
            angle += (360f / segments);
        }

        edge.SetPoints(points);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
