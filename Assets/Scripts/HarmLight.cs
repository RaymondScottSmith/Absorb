using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmLight : MonoBehaviour
{

    [SerializeField] private int damagePerDuration;
    [SerializeField] private float duration;
    [SerializeField] private float damageDelay;
    private BoxCollider2D myCollider;
    private Animator myAnimator;
    private AudioSource myAudio;

    private bool playerInside;
    private bool isOn;
    private bool isWaiting;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
        myAudio = GetComponent<AudioSource>();
    }

    public void TurnOn()
    {
        myAudio.Play();
        myAnimator.SetBool("PlayerIn", true);
        StartCoroutine(DelayDamage());
    }

    public void TurnOff()
    {
        myAudio.Stop();
        isOn = false;
        myAnimator.SetBool("PlayerIn", false);
    }

    private IEnumerator DoDamage(PlayerShrink ps)
    {
        ps.TakeDamage(damagePerDuration);
        yield return new WaitForSecondsRealtime(duration);
        isWaiting = false;
    }

    private IEnumerator DelayDamage()
    {
        yield return new WaitForSecondsRealtime(damageDelay);
        isOn = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isOn)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Player") && !isWaiting)
        {
            isWaiting = true;
            StartCoroutine(DoDamage(collision.gameObject.GetComponent<PlayerShrink>()));
        }
    }
}
