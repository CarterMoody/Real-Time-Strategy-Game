using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image background;
    private SpriteRenderer spriteRenderer;

    public Sprite tabIdle;
    public Sprite tabHover;
    
    public bool toggleButton;
    private bool active;
    public Sprite activeSprite;
    public Sprite inactiveSprite;
    
    public string activeToolTipText;
    public string inactiveToolTipText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        background.sprite = tabHover;
        if (active == true)
        {
            TooltipFollow.ShowTooltip_Static(activeToolTipText);                
        }
        else if (active == false)
        {
            TooltipFollow.ShowTooltip_Static(inactiveToolTipText);                
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        background.sprite = tabIdle;
        TooltipFollow.HideTooltip_Static(); 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (toggleButton == true){
            Toggle();
        }
    }

    private void Toggle()
    {
        if (active == true)
        {
            active = false;
            spriteRenderer.sprite = inactiveSprite;
        }
        else if (active == false)
        {
            active = true;
            spriteRenderer.sprite = activeSprite;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        background = GetComponent<Image>();
        background.sprite = tabIdle;
        active = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
