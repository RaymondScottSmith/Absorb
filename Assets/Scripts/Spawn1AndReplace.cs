using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn1AndReplace : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    private GameObject spawn;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorClose;
    private Animator animator;
    private AudioSource audioSource;
    private bool spawning = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawn == null && !spawning)
        {
            spawning = true;
            StartCoroutine(Spawn1());
        }
    }

    private IEnumerator Spawn1()
    {
        
        audioSource.Stop();
        
        animator.SetBool("Open", true);
        audioSource.PlayOneShot(doorOpen);
        yield return new WaitForSeconds(1f);
        spawn = Instantiate(spawnPrefab, spawnLocation.position, spawnPrefab.transform.rotation);
        spawning = false;
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(doorClose);
        animator.SetBool("Open", false);
    }
}
