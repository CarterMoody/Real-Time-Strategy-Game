﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class UIsoldierScript : MonoBehaviour
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
        //TooltipTop.ShowTooltipTop_Static("test");
         if (PointBank.subtractPoints_Static(3))
        {
            TooltipTop.ShowTooltipTop_Static("Spawning 1 soldier");
            US_Spawner.spawnTroops_Static(Unit_US.Unit_USType.Soldier);
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
            //TooltipFollow.ShowTooltip_Static("Private | M4A1");
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
