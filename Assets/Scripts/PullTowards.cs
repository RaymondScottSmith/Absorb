using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class PullTowards : MonoBehaviour
{
    public Direction towardsEmitter;
    public string targetTag = "Player";

    [SerializeField] private float strength = 1f;

    private GameObject target;

    private bool isActing;

    [SerializeField]
    private GravityState gravityState;

    [SerializeField] private ParticleSystem pullLights;

    [SerializeField] private ParticleSystem pushLights;
    
    [SerializeField]
    private Light2D spotlight;

    private AudioSource activeSound;

    [SerializeField] private List<Light2D> sideLights;

    [SerializeField] private bool pullWhenImmobile;

    private void Awake()
    {
        activeSound = GetComponent<AudioSource>();
        switch (gravityState)
        {
            case GravityState.Off:
                TurnOff();
                break;
            case GravityState.Pull:
                TurnAttractOn();
                break;
            case GravityState.Push:
                TurnRepulseOn();
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            target = collision.gameObject;
            isActing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            target = null;
            isActing = false;
        }
    }

    public void TurnAttractOn()
    {
        activeSound.Play();
        gravityState = GravityState.Pull;
        pushLights.gameObject.SetActive(false);
        pullLights.gameObject.SetActive(true);
        spotlight.intensity = 2f;
        spotlight.color = Color.cyan;
        foreach (Light2D sidelight in sideLights)
        {
            sidelight.color = Color.cyan;
        }

    }

    public void TurnRepulseOn()
    {
        activeSound.Play();
        gravityState = GravityState.Push;
        pushLights.gameObject.SetActive(true);
        pullLights.gameObject.SetActive(false);
        spotlight.intensity = 2.85f;
        spotlight.color = Color.red;
        foreach (Light2D sidelight in sideLights)
        {
            sidelight.color = Color.red;
        }
    }

    public void TurnOff()
    {
        activeSound.Stop();
        gravityState = GravityState.Off;
        pushLights.gameObject.SetActive(false);
        pullLights.gameObject.SetActive(false);
        spotlight.intensity = 0f;
    }

    public void SwitchAttractRepulse()
    {
        switch (gravityState)
        {
            case GravityState.Pull:
                TurnRepulseOn();
                break;
            case GravityState.Push:
                TurnAttractOn();
                break;
            default:
                TurnAttractOn();
                break;
        }
    }

    public void SetState(GravityState newState)
    {
        gravityState = newState;
    }

    private void FixedUpdate()
    {
        if (isActing && target != null && gravityState != GravityState.Off)
        {
            PlayerController player = target.GetComponent<PlayerController>();
            if (player != null)
            {
                if (!pullWhenImmobile && !player.moving)
                {
                    return;
                }
            }
            
            switch (towardsEmitter)
            {
                case Direction.Up:
                    if (gravityState == GravityState.Pull)
                        target.GetComponent<Rigidbody2D>().AddForce(Vector2.up * strength, ForceMode2D.Impulse);
                    else
                    {
                        target.GetComponent<Rigidbody2D>().AddForce(Vector2.down * strength, ForceMode2D.Impulse);
                    }
                    break;
                case Direction.Down:
                    if (gravityState == GravityState.Pull)
                        target.GetComponent<Rigidbody2D>().AddForce(Vector2.down * strength, ForceMode2D.Impulse);
                    else
                    {
                        target.GetComponent<Rigidbody2D>().AddForce(Vector2.up * strength, ForceMode2D.Impulse);
                    }

                    break;
                case Direction.Left:
                    if (gravityState == GravityState.Pull)
                        target.GetComponent<Rigidbody2D>().AddForce(Vector2.left * strength, ForceMode2D.Impulse);
                    else
                    {
                        target.GetComponent<Rigidbody2D>().AddForce(Vector2.right * strength, ForceMode2D.Impulse);
                    }

                    break;
                case Direction.Right:
                    if (gravityState == GravityState.Pull)
                        target.GetComponent<Rigidbody2D>().AddForce(Vector2.right * strength, ForceMode2D.Impulse);
                    else
                    {
                        target.GetComponent<Rigidbody2D>().AddForce(Vector2.left * strength, ForceMode2D.Impulse);
                    }
                    break;
            }
            
        }
    }
    
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public enum GravityState
{
    Pull,
    Push,
    Off
}
