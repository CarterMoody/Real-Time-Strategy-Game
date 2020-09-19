using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class UITroopSelectionMouse : MonoBehaviour
{

    private bool mouseOver;
    // Start is called before the first frame update
    void Start()
    {
        mouseOver = false;
        //transform.Find("UITroopSelection").GetComponent<Button_UI>().MouseOverOnceFunc = () => TooltipFollow.ShowTooltip_Static("Train Troops");
        //transform.Find("UITroopSelection").GetComponent<Button_UI>().MouseOutOnceFunc = () => TooltipFollow.HideTooltip_Static();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Called when mouse clicks on object
    void OnMouseDown()
    {
        // Move this sprites position away, and move the Children UI in its place
        Debug.Log("Detected Mouse Down (Click)");
    }

    // Called when mouse over object
    void OnMouseOver()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //Debug.Log("Detected Mouse Over");
        if (mouseOver == false)
        {
            mouseOver = true;
            TooltipFollow.ShowTooltip_Static("Train Troops");
            Debug.Log("Mouse over");
        }
        //TooltipFollow.ShowTooltip_Static("Train Troops");
        //TooltipFollow.ShowTooltip_Static("Train Troops");
        //Debug.Log(gameObject.name);
        

    }

    // Called when mouse not over object
    void OnMouseExit()
    {
    //    SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //Debug.Log("mouse not on US");
        mouseOver = false;
        TooltipFollow.HideTooltip_Static(); 
    //    spriteRenderer.sprite = USsoldierSprite;
    }
}
