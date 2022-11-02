using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealthUp : MonoBehaviour
{
    
    private Vector3 moveDirection;

    private float moveSpeed;
    public void Activate(Vector3 direction, Vector3 maxMinSize, Vector2 maxMinSpeed)
    {
        float size = Random.Range(maxMinSize.y, maxMinSize.x);
        transform.localScale = new Vector3(size, size, size);
        moveDirection = direction;
        moveSpeed = Random.Range(maxMinSpeed.y, maxMinSpeed.x);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = transform.position + moveDirection * moveSpeed * Time.deltaTime;
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (col.gameObject.GetComponent<ShipControls>() != null)
            {
                col.gameObject.GetComponent<ShipControls>().HealthPickup();
                Destroy(gameObject);
            }
            
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Killzone"))
        {
            Destroy(gameObject);
        }
    }
}
