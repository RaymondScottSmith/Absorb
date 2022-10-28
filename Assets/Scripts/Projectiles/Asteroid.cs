using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Vector3 moveDirection;

    private float moveSpeed;

    [SerializeField]
    private GameObject explosionPrefab;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Activate(Vector3 direction, Vector3 maxMinSize, Vector2 maxMinSpeed, Vector2 maxMinRotation)
    {
        float size = Random.Range(maxMinSize.y, maxMinSize.x);
        transform.localScale = new Vector3(size, size, size);
        GetComponent<Animator>().SetFloat("Speed Multiplier", Random.Range(maxMinRotation.y, maxMinRotation.x));
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
            Debug.Log("Hit the player Ship");
            GameObject explosion = Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);
            explosion.GetComponent<AsteroidExplosion>().SetMovement(moveDirection, moveSpeed, transform.localScale);
            Destroy(gameObject);
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
