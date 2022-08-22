using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MiniRocket : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float speed = 30f;

    [SerializeField] private float maxTurn = 15f;
    [SerializeField] private float updateTime = 0.5f;

    private PlayerController player;

    private Vector3 holder;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ChangeDirection", 0f, updateTime);
        
    }

    private void ChangeDirection()
    {
        rb.velocity = Vector2.zero;
        Quaternion oldTransform = transform.rotation;
        transform.right = player.transform.position - transform.position;
        transform.rotation = Quaternion.RotateTowards(oldTransform, transform.rotation, maxTurn);
        rb.AddForce(transform.right * speed, ForceMode2D.Force);
    }

    
}
