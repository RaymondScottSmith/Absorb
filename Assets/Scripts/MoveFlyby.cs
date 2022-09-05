using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFlyby : MonoBehaviour
{

    private SpriteRenderer mySpriteRenderer;
    private float despawnX;
    [SerializeField] private float speed;

    private bool despawnSet;

    void Awake()
    {
        despawnSet = false;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetDespawn(float despawnX)
    {
        this.despawnX = despawnX;
        if (despawnX < 0)
            mySpriteRenderer.flipX = true;
        despawnSet = true;

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!despawnSet)
            return;
        
        Vector3 currentPos = transform.position;
        if (mySpriteRenderer.flipX)
        {
            transform.position = new Vector3(currentPos.x - speed * Time.deltaTime, currentPos.y, 0);
            
            if (currentPos.x < despawnX)
                Destroy(gameObject);
                
        }
        else
        {
            transform.position = new Vector3(currentPos.x + speed * Time.deltaTime, currentPos.y, 0);
            
            if (currentPos.x > despawnX)
                Destroy(gameObject);
                
        }
    }
}
