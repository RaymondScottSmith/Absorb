using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class StationaryTurret : MonoBehaviour
{
    [SerializeField]
    private Light2D eyeLight;

    [SerializeField] private Light2D coneLight;

    [SerializeField] private SpriteRenderer spotLight;

    [SerializeField] private Sprite redEye;
    [SerializeField] private Sprite blueEye;
    [SerializeField] private SpriteRenderer robotEye;

    private Color blueColor = new Color(0, 238/255f, 255/255f, 20/255f);
    private Color redColor = new Color(255/255f,10/255f,0, 20/255f);

    private PlayerController player;

    private bool playerInRange;

    private Animator animator;
    [SerializeField]
    private AudioSource shootingAudio;

    [SerializeField] private AudioSource servoAudio;

    [SerializeField] private int damageAmount = 10;

    private bool isShooting;

    [SerializeField] private AudioClip playerDamageSound;

    private CameraController camController;

    [SerializeField] private float maxTurn = -90f;

    [SerializeField] private float minTurn;

    private Quaternion firstPos;

    [SerializeField] private int hitsToKill = 3;

    [SerializeField] private bool facingRight = true;
    
    
    // Start is called before the first frame update
    void Start()
    {
        isShooting = false;
        player = FindObjectOfType<PlayerController>();
        animator = GetComponentInParent<Animator>();
        firstPos = transform.rotation;
        //audioSource = GetComponentInParent<AudioSource>();
        camController = FindObjectOfType<CameraController>();
        LookingMode();
    }

    public void SpottedMode()
    {
        Debug.Log("In Spotted Mode");
        playerInRange = true;
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, 
            player.transform.position - transform.position, 
            40, ~(LayerMask.GetMask("CrewColliders", "Ignore Raycast")));
        
        if (playerInRange && hit.collider.CompareTag("Player"))
        {
            

            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(ShootPlayer());
            }
            robotEye.sprite = redEye;
            eyeLight.color = redColor;
            coneLight.color = redColor;
            spotLight.color = redColor;
            Quaternion oldTransform = transform.rotation;
            int yRot = 180;
            transform.right = (player.transform.position - transform.position);
            
            
            
            //Debug.Log(transform.rotation.eulerAngles.z);
            if (facingRight)
            {
                //Debug.Log("Old Transform: " + oldTransform + " transform.rotation: " + transform.rotation);
                transform.rotation = Quaternion.RotateTowards(oldTransform, transform.rotation, 180);
                transform.eulerAngles = new Vector3(0, 180, -transform.rotation.eulerAngles.z);

                float zAngle = transform.rotation.eulerAngles.z;
                if (!((zAngle > 0 && zAngle < 90) || (zAngle <= 360 && zAngle > 270)))
                {
                    Debug.Log("Should be restricting here");
                    transform.rotation = oldTransform;
                }
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(oldTransform, transform.rotation, 180);
                transform.eulerAngles = new Vector3(0, 0, (transform.rotation.eulerAngles.z + 180));

                float zAngle = transform.rotation.eulerAngles.z;
                if (!((zAngle > 0 && zAngle < 90) || (zAngle <= 360 && zAngle > 270)))
                {
                    Debug.Log("Should be restricting here");
                    transform.rotation = oldTransform;
                }
            }
            
            
           
            if (oldTransform != transform.rotation && !servoAudio.isPlaying)
            {
                servoAudio.Play();
            }
            //transform.right = -(player.transform.position - transform.position);
            //transform.rotation = Quaternion.RotateTowards(oldTransform, transform.rotation,45);
        }
        else if (!isShooting)
        {
            Debug.Log("Should be turning blue");
            animator.SetTrigger("OutOfRange");
            isShooting = false;
            //shootingAudio.loop = false;
            //animator.SetBool("Firing", false);
            robotEye.sprite = blueEye;
            eyeLight.color = blueColor;
            coneLight.color = blueColor;
            spotLight.color = blueColor;
            if (transform.rotation != firstPos)
            {
                servoAudio.Play();
            }
            transform.rotation = firstPos;
            //Debug.Log("In Looking Mode");
        }
    }

    private IEnumerator ShowDamage()
    {
        yield return new WaitForSeconds(0.25f);
        camController.ShakeScreen();
        player.TakeDamage(damageAmount/2, null);
        yield return new WaitForSeconds(0.25f);
        player.TakeDamage(damageAmount/2, null, null, false);
        camController.ShakeScreen();
    }
    
    private IEnumerator ShootPlayer()
    {
        yield return new WaitForSeconds(1.5f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, 
            player.transform.position - transform.position, 
            40, ~(LayerMask.GetMask("CrewColliders", "Ignore Raycast")));
        
        if (!hit.collider.CompareTag("Player") || !playerInRange)
        {
            isShooting = false;
            animator.SetTrigger("OutOfRange");
            yield break;
        }

        StartCoroutine(ShowDamage());
        /*
        if (!shootingAudio.isPlaying)
        {
            shootingAudio.loop = true;
            shootingAudio.Play();
        }
        */
        shootingAudio.Play();
        animator.SetTrigger("Fire");
        
        
        Debug.Log("Taking damage");
        yield return new WaitForSeconds(0.5f);
        
        shootingAudio.loop = false;
        isShooting = false;
        
    }

    public void LookingMode()
    {
        Debug.Log("Should be in looking mode");
        playerInRange = false;
    }

    // Update is called once per frame
    
}
