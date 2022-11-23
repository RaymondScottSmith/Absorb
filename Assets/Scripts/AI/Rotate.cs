using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public bool isRotating = false;

    public bool rotateSetAmount;

    public Vector3 rotateAngle;

    public float rotateTimer;

    public float speed;

    public Vector3 rotateAxis;

    public int rotateAngleSteps = 4;

    private AudioSource rotateSound;
    // Start is called before the first frame update
    void Start()
    {
        rotateSound = GetComponent<AudioSource>();
        if (rotateSetAmount)
        {
            InvokeRepeating("RotateSetAmount",0,rotateTimer);
        }
    }

    public void RotateSetAmount()
    {
        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotateAngle);
        if (rotateSound != null)
        {
            rotateSound.Play();
        }
        StartCoroutine(RotateThrough());
    }

    private IEnumerator RotateThrough()
    {
        float z = transform.rotation.eulerAngles.z;
        if (rotateAngle.z > 0)
        {
            for (float i = 1; i <= rotateAngle.z/rotateAngleSteps; i++)
            {
                yield return new WaitForFixedUpdate();
                transform.rotation = Quaternion.Euler(0,0,z+i*rotateAngleSteps);
            }
        }
        else
        {
            for (float i = -1; i >= rotateAngle.z/rotateAngleSteps; i--)
            {
                yield return new WaitForFixedUpdate();
                transform.rotation = Quaternion.Euler(0,0,z+i*rotateAngleSteps);
            }
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isRotating || rotateSetAmount)
            return;
        
        transform.rotation =Quaternion.Euler(rotateAxis * speed * Time.deltaTime + transform.rotation.eulerAngles);
        
        
    }
}
