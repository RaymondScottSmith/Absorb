using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AlienBreakout : MonoBehaviour
{
    [SerializeField] private Collider2D tankCollider;

    public UnityEvent OnBreakout;

    public UnityEvent OnCollide;

    [SerializeField] private string targetTag = "Player";

    [SerializeField]
    private int maxCollisions = 3;

    private int collisions = 0;

    [SerializeField] private AudioClip hitSound, breakSound;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = hitSound;
    }

    private void OnBreak()
    {
        audioSource.clip = breakSound;
        OnBreakout?.Invoke();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        collisions++;
        if (col.gameObject.CompareTag(targetTag))
        {
            if (collisions < maxCollisions)
            {
                OnCollide?.Invoke();
            }
            else
            {
                OnBreak();
            }
        }
    }
}
