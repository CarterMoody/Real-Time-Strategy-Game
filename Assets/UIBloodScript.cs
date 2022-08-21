using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBloodScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private SpriteRenderer spriteRenderer;
    private GameObject bloodParticleSystemHandler;
    private bool bloodOn;


    // Called when mouse clicks on object
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Detected Mouse Down (Click)");
        if (bloodOn == true)
        {
            DisableBlood();
        }
        else
        {
            EnableBlood();
        }
    }

    // Called when mouse over object
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Detected Mouse Over");
        if (bloodOn == true)
        {
            //TooltipFollow.ShowTooltip_Static("Disable Blood");
        }
        else if (bloodOn == false)
        {
           // TooltipFollow.ShowTooltip_Static("Enable Blood");
        }
    }

    // Called when mouse not over object
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("No Longer Detected Mouse Over");
        TooltipFollow.HideTooltip_Static(); 
    }

    public void DisableBlood()
    {
        bloodOn = false;
        BloodParticleSystemHandler.Instance.DisableBlood();
    }
    public void EnableBlood()
    {
        bloodOn = true;
        BloodParticleSystemHandler.Instance.EnableBlood();
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        EnableBlood();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
