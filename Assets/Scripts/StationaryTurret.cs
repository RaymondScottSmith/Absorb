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
    
    
    // Start is called before the first frame update
    void Start()
    {
        isShooting = false;
        player = FindObjectOfType<PlayerController>();
        animator = GetComponentInParent<Animator>();
        //audioSource = GetComponentInParent<AudioSource>();
        LookingMode();
    }

    public void SpottedMode()
    {
        playerInRange = true;
    }

    private void FixedUpdate()
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
            
            transform.rotation = Quaternion.RotateTowards(oldTransform, transform.rotation, 180);
            transform.eulerAngles = new Vector3(0, 180, -transform.rotation.eulerAngles.z);

            if (oldTransform != transform.rotation && !servoAudio.isPlaying)
            {
                servoAudio.Play();
            }
            //transform.right = -(player.transform.position - transform.position);
            //transform.rotation = Quaternion.RotateTowards(oldTransform, transform.rotation,45);
            Debug.Log("In Spotted Mode");
        }
        else
        {
            isShooting = false;
            //shootingAudio.loop = false;
            //animator.SetBool("Firing", false);
            robotEye.sprite = blueEye;
            eyeLight.color = blueColor;
            coneLight.color = blueColor;
            spotLight.color = blueColor;
            //Debug.Log("In Looking Mode");
        }
    }
    
    private IEnum
    
    private IEnumerator ShootPlayer()
    {
        yield return new WaitForSeconds(1f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, 
            player.transform.position - transform.position, 
            40, ~(LayerMask.GetMask("CrewColliders", "Ignore Raycast")));
        
        if (!hit.collider.CompareTag("Player") || !playerInRange)
        {
            yield break;
        }
            
        
        if (!shootingAudio.isPlaying)
        {
            shootingAudio.loop = true;
            shootingAudio.Play();
        }
        animator.SetTrigger("Fire");
        player.TakeDamage(damageAmount, playerDamageSound);
        Debug.Log("Taking damage");
        yield return new WaitForSeconds(1f);
        shootingAudio.loop = false;
        isShooting = false;
        
    }

    public void LookingMode()
    {
        playerInRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
