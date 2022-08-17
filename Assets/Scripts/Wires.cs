using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wires : MonoBehaviour
{
    private ParticleSystem pSystem;
    private Animator animator;

    [SerializeField] private float maxStartDelay;
    // Start is called before the first frame update
    void Start()
    {
        pSystem = GetComponentInChildren<ParticleSystem>();
        animator = GetComponent<Animator>();
        pSystem.Stop();
        animator.enabled = false;
        float randomStart = Random.Range(0, maxStartDelay);

        StartCoroutine(startSparking(randomStart));
    }

    private IEnumerator startSparking(float delay)
    {
        yield return new WaitForSeconds(delay);
        pSystem.Play();
        animator.enabled = true;
    }
}
