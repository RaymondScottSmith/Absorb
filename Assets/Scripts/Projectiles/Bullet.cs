using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed=1f;

    [SerializeField] private int damage;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("TimeoutDestroy", 2f, 10f);
    }

    private void TimeoutDestroy()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
