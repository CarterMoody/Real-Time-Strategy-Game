using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class UISoundScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    private AudioSource source;
    private bool mouseOver;
    private bool soundOn = true;
    // Start is called before the first frame update
    void Start()
    {
        mouseOver = false;
        source = GetComponent<AudioSource>();           // Gets the component responsible for playing Sounds
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Called when mouse clicks on object
    void OnMouseDown()
    {
        Debug.Log("toggling mute");
        //TooltipTop.ShowTooltipTop_Static("test");
        if (soundOn)
        {
            TooltipTop.ShowTooltipTop_Static("Ears off");
            AudioListener.volume = 0;
            spriteRenderer.sprite = soundOffSprite;
            soundOn = false;
        }
        else {
            TooltipTop.ShowTooltipTop_Static("Ears on");
            AudioListener.volume = 1;
            spriteRenderer.sprite = soundOnSprite;
            soundOn = true;
        }
        
        
    }

    // Called when mouse over object
    void OnMouseOver()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //Debug.Log("Detected Mouse Over");
        if (mouseOver == false)
        {
            mouseOver = true;
            TooltipFollow.ShowTooltip_Static("Mute");
            //Debug.Log("Mouse over");
        }
        

    }

    // Called when mouse not over object
    void OnMouseExit()
    {
        //Debug.Log("mouse not on US");
        mouseOver = false;
        TooltipFollow.HideTooltip_Static(); 
    }
}
