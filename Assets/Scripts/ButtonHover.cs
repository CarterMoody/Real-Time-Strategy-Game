using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image background;

    public Sprite tabIdle;
    public Sprite tabHover;

    public string toolTipText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        background.sprite = tabHover;
        TooltipFollow.ShowTooltip_Static(toolTipText);                
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        background.sprite = tabIdle;
        TooltipFollow.HideTooltip_Static(); 
    }



    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();
        background.sprite = tabIdle;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
