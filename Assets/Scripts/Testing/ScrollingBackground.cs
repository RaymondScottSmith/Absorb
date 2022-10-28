using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = -1f;

    private Vector2 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float newX = Mathf.Repeat(Time.time * scrollSpeed, 70);
        transform.position = startPos + Vector2.right * newX;
    }
}
