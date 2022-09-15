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
    private AudioSource audioSource;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        animator = GetComponentInParent<Animator>();
        audioSource = GetComponentInParent<AudioSource>();
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
            if (!audioSource.isPlaying)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
                
            animator.SetBool("Firing", true);
            robotEye.sprite = redEye;
            eyeLight.color = redColor;
            coneLight.color = redColor;
            spotLight.color = redColor;
            Quaternion oldTransform = transform.rotation;
            int yRot = 180;
            transform.right = (player.transform.position - transform.position);
            
            transform.rotation = Quaternion.RotateTowards(oldTransform, transform.rotation, 180);
            transform.eulerAngles = new Vector3(0, 180, -transform.rotation.eulerAngles.z);
            //transform.right = -(player.transform.position - transform.position);
            //transform.rotation = Quaternion.RotateTowards(oldTransform, transform.rotation,45);
            Debug.Log("In Spotted Mode");
        }
        else
        {
            audioSource.loop = false;
            animator.SetBool("Firing", false);
            robotEye.sprite = blueEye;
            eyeLight.color = blueColor;
            coneLight.color = blueColor;
            spotLight.color = blueColor;
            //Debug.Log("In Looking Mode");
        }
    }
    
    private IEnumerator ShootPlayer()
    {
    
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
