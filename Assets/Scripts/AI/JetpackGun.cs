using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackGun : MonoBehaviour
{
    private GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = GetComponentInParent<Jetpack>().bulletPrefab;
    }

    public void StartFiring()
    {
        GetComponent<Animator>().SetBool("Firing", true);
    }

    public void StopFiring()
    {
        GetComponent<Animator>().SetBool("Firing", false);
    }

    public void FireBullet()
    {
        Instantiate(bulletPrefab, transform.position, GetComponentInParent<Transform>().rotation);
    }
}
