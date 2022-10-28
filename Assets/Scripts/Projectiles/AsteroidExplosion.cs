using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidExplosion : MonoBehaviour
{
    private float moveSpeed;
    private Vector3 moveDirection;
    public void SetMovement(Vector3 direction, float speed, Vector3 size)
    {
        transform.localScale = size;
        moveSpeed = speed;
        moveDirection = direction;
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.position = transform.position + moveDirection * moveSpeed * Time.deltaTime;
    }
}
