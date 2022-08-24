using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
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

    private Collider2D collider2D;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject explosion;
    void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        player = FindObjectOfType<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    void OnCollisionEnter2D(Collision2D col)
    {
        CancelInvoke();
        StartCoroutine(Explode(col));

    }

    private IEnumerator Explode(Collision2D col)
    {
        collider2D.enabled = false;
        rb.velocity = Vector2.zero;
        spriteRenderer.enabled = false;
        explosion.SetActive(true);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public void HitPlayer()
    {
        player.TakeDamage(20,null);
    }


}
