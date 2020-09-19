using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class TooltipFollow : MonoBehaviour
{

    // this static instance and static methods allow access from any other class
    //    also add "instance = this" in awake
    private static TooltipFollow instance;



    [SerializeField] private Camera uiCamera;
    private TextMeshProUGUI tooltipText;
    private RectTransform backgroundRectTransform;
    private RectTransform parentRectTransform;
    private void Awake()
    {
        instance = this;
        backgroundRectTransform = transform.Find("ttf_background").GetComponent<RectTransform>();
        tooltipText = transform.Find("ttf_text").GetComponent<TextMeshProUGUI>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        HideTooltip();

        //ShowTooltip("Random tooltip text1");
    }


    private void Update()
    {
        Vector2 localPoint;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, Input.mousePosition, uiCamera, out localPoint);
        //localPoint = localPoint + new Vector2(tooltipText.preferredWidth / 2f, tooltipText.preferredHeight / 2f);
        transform.localPosition = localPoint;
    }
    private void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);

        tooltipText.SetText(tooltipString);
        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2f, tooltipText.preferredHeight + textPaddingSize * 2f);
        backgroundRectTransform.sizeDelta = backgroundSize;
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }


    // Static methods available from any class
    public static void ShowTooltip_Static(string tooltipString)
    {
        instance.ShowTooltip(tooltipString);
    }

    public static void HideTooltip_Static()
    {
        instance.HideTooltip();
    }
}
