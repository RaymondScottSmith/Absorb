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

    private Collider2D rocketCollider;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject explosion;

    private AudioSource myAudioSource;

    private ShootingFood source;

    private Vector2 originalVelocity;
    void Awake()
    {
        rocketCollider = GetComponent<Collider2D>();
        player = FindObjectOfType<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        myAudioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ChangeDirection", 0f, updateTime);
        
    }

    public void SetSource(ShootingFood firingObject)
    {
        
        source = firingObject;
        transform.SetParent(null);
    }

    private void ChangeDirection()
    {
        originalVelocity = rb.velocity;
        rb.velocity = Vector2.zero;
        Quaternion oldTransform = transform.rotation;
        transform.right = player.transform.position - transform.position;
        transform.rotation = Quaternion.RotateTowards(oldTransform, transform.rotation, maxTurn);
        rb.AddForce(transform.right * speed, ForceMode2D.Force);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        myAudioSource.Stop();
        CancelInvoke();
        StartCoroutine(Explode(col));

    }

    private IEnumerator Explode(Collision2D col)
    {
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.zero;
        spriteRenderer.enabled = false;
        explosion.SetActive(true);
        source.ReadyWeapon();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public void HitPlayer()
    {
        player.ChangeDirection(originalVelocity);
        player.TakeDamage(20,null);
        
    }


}
