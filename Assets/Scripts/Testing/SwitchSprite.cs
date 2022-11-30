using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSprite : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private int spriteNumber;
    // Start is called before the first frame update
    void Start()
    {

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (sprites.Count > 0)
        {
            spriteRenderer.sprite = sprites[0];
            spriteNumber = 0;
        }
    }

    public void Swap(int nextSprite)
    {
        if (nextSprite < sprites.Count)
        {
            spriteRenderer.sprite = sprites[nextSprite];
        }
        else
        {
            Debug.Log("There are less than " + nextSprite + " sprites to use.");
        }
    }

    public void Iterate()
    {
        if (spriteNumber + 1 >= sprites.Count)
        {
            spriteNumber = 0;
        }
        else
        {
            spriteNumber++;
        }

        spriteRenderer.sprite = sprites[spriteNumber];
    }
}
