using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class UIbackScript : MonoBehaviour
{

    private bool mouseOver;
    // Start is called before the first frame update
    void Start()
    {
        mouseOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Called when mouse clicks on object
    void OnMouseDown()
    {
        // Move this sprites position away, and move the Children UI in its place
        //Debug.Log("Detected Mouse Down (Click)");
    }

    // Called when mouse over object
    void OnMouseOver()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //Debug.Log("Detected Mouse Over");
        if (mouseOver == false)
        {
            mouseOver = true;
            TooltipFollow.ShowTooltip_Static("Back");
            Debug.Log("Mouse over");
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
