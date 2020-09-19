using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class USonMouseOver : MonoBehaviour
{

    public Sprite USdeadSprite;
    public Sprite USsoldierSprite;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when mouse over object
    //public void OnMouseOver()
    void OnMouseOver()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Debug.Log("Detected Mouse Over");
        Debug.Log(gameObject.name);
        spriteRenderer.sprite = USdeadSprite;
        spriteRenderer.fadeSprite(this, 60f, DestroySprite); // fades sprite in total of 60 seconds
        

    }

    // Called when mouse not over object
    //public void OnMouseExit()
    void OnMouseExit()
    {
    //    SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Debug.Log("mouse not on US");
    //    spriteRenderer.sprite = USsoldierSprite;
    }

private void DestroySprite (SpriteRenderer spriteRenderer)
{
    Destroy(spriteRenderer.gameObject);
}

}
