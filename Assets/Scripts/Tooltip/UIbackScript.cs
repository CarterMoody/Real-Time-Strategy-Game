using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIbackScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject UIMenu;
    public Image background;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public Sprite menuHiddenSprite;
    public Sprite menuVisibleSprite;
        
    private SpriteRenderer spriteRenderer;
    private bool mouseOver;
    private bool menuHidden;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        background = GetComponent<Image>();
        mouseOver = false;
        ShowUIMenu();
        background.sprite = tabIdle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Called when mouse clicks on object
    public void OnPointerClick(PointerEventData eventData)
    {
        // Move this sprites position away, and move the Children UI in its place
        //Debug.Log("Detected Mouse Down (Click)");
        if (menuHidden == true)
        {

            ShowUIMenu();
        }
        else
        {
            HideUIMenu();
        }
        

    }

    // Called when mouse over object
    public void OnPointerEnter(PointerEventData eventData)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //Debug.Log("Detected Mouse Over");
        if (mouseOver == false)
        {
            mouseOver = true;
            if (menuHidden == true)
            {
                TooltipFollow.ShowTooltip_Static("Show Menu");
            }
            if (menuHidden == false)
            {
                TooltipFollow.ShowTooltip_Static("Hide Menu");
            }
        }
        

    }

    // Called when mouse not over object
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("mouse not on US");
        mouseOver = false;
        TooltipFollow.HideTooltip_Static(); 
    }

    // Hides all UI buttons underneath the UIMenu object in the scene hierarchy (Except this back button)
    public void HideUIMenu()
    {
        int index = this.transform.GetSiblingIndex(); // get this childs (back button) index from within the group of UI buttons so we don't hide it
        for(int i=0; i<UIMenu.transform.childCount; i++)
        {
            if (i == index) { continue; }
            UIMenu.transform.GetChild(i).gameObject.SetActive(false);
        }
        spriteRenderer.sprite = menuHiddenSprite;
        menuHidden = true;
    }

    // Shows all UI buttons underneath the UIMenu object in the scene hierarchy
    public void ShowUIMenu()
    {
        int index = this.transform.GetSiblingIndex(); // get this childs (back button) index from within the group of UI buttons so we don't hide it
        for(int i=0; i<UIMenu.transform.childCount; i++)
        {
            if (i == index) { continue; }
            UIMenu.transform.GetChild(i).gameObject.SetActive(true);
        }
        spriteRenderer.sprite = menuVisibleSprite;
        menuHidden = false;  
    }
}
