using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public bool isRotating = false;

    public float speed;

    public Vector3 rotateAxis;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isRotating)
            return;
        transform.rotation =Quaternion.Euler(rotateAxis * speed * Time.deltaTime + transform.rotation.eulerAngles);
    }
}
