using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
    
    public string targetTag = "Player";

    private PlayerShrink player;

    private bool isTouching;

    private Vector2 point;
    
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            player = collision.gameObject.GetComponent<PlayerShrink>();
            isTouching = true;

        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            isTouching = false;
            player.isHidden = false;
        }
    }

    private void FixedUpdate()
    {
        if (!isTouching)
            return;

        if (player.CheckIfHidden(GetComponent<Collider2D>()))
        {
            //Debug.Log("Now Fully Hidden");
            player.isHidden = true;
        }
        else
        {
            player.isHidden = false;
        }
    }
}
