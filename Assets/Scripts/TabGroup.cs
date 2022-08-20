using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public TabButton selectedTab;
    public List<GameObject> objectsToSwap;
    public PanelGroup panelGroup;

    public void Subscribe(TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if(selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHover;
            TooltipFollow.ShowTooltip_Static(button.toolTipText);
        }
        
    }


    public void OnTabExit(TabButton button)
    {
        TooltipFollow.HideTooltip_Static(); 
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        // Handle case where user clicks again to hide tab
        if(selectedTab == button)
        {
            selectedTab.Deselect();
            selectedTab = null;
            ResetTabs();
            SetAllChildrenInactive();
        }
        else
        {
            if(selectedTab != null)
            {
                selectedTab.Deselect();
            }

            selectedTab = button;

            selectedTab.Select();

            ResetTabs();
            button.background.sprite = tabActive;
            TooltipFollow.HideTooltip_Static(); 
            SetOtherChildrenInactive(button);
        }

        if (panelGroup != null)
        {
            panelGroup.SetPageIndex(selectedTab.transform.GetSiblingIndex());
        }

    }

    private void SetOtherChildrenInactive(TabButton button)
    {
        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i<objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }
    private void SetAllChildrenInactive()
    {
        for(int i = 0; i<objectsToSwap.Count; i++)
        {
            objectsToSwap[i].SetActive(false);
        }
    }

    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if(selectedTab!=null && button == selectedTab) { continue; }
            button.background.sprite = tabIdle;
        }
    }

    public void Start()
    {
        ResetTabs();
        SetAllChildrenInactive();
    }
}
