using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USonDeath : MonoBehaviour
{

    public Sprite USdeadSprite;
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = USdeadSprite;

    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.color = spriteRenderer.color + new Color(0, 0, 0, .5f);
    }
}
