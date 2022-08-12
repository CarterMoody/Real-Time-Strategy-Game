using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipTop : MonoBehaviour
{

    [SerializeField] int toolTipTime;

    // this static instance and static methods allow access from any other class
    //    also add "instance = this" in awake
    private static TooltipTop instance;

    private TextMeshProUGUI tooltipText;
    private RectTransform backgroundRectTransform;
    private void Awake()
    {
        instance = this;
        backgroundRectTransform = transform.Find("ttt_background").GetComponent<RectTransform>();
        tooltipText = transform.Find("ttt_text").GetComponent<TextMeshProUGUI>();

        //ShowTooltip("Welcome to Blood N Mud | Enjoy");
        ShowTooltipTop_Static("Welcome to Carter's RNG RTS | Enjoy");
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

    public static void HideTooltipTop_Static()
    {
        instance.HideTooltip();
    }

    // Static methods available from any class
    // This will start a coroutine which shows the tool tip for a speecified time
    public static void ShowTooltipTop_Static(string tooltipString)
    {
        //gameObject.SetActive(true);
        //instance.StartCoroutine(displayToolTip(tooltipString));
        instance.coRoutineWrapper(tooltipString);
    }

    private void coRoutineWrapper(string tooltipString)
    {
        gameObject.SetActive(true);
        instance.StartCoroutine(displayToolTip(tooltipString));
    }


    static IEnumerator displayToolTip(string tooltipString)
    {
        
        // display the tooltipString
        instance.ShowTooltip(tooltipString);

        yield return new WaitForSeconds(instance.toolTipTime);

        instance.HideTooltip();
    }


}
