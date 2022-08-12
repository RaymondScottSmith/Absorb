using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 1;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            var mouseDir = mousePos - gameObject.transform.position;
            mouseDir = mouseDir.normalized;
            rb.AddForce(mouseDir * baseSpeed, ForceMode2D.Impulse);
        }
    }
}
