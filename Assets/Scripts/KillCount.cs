using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillCount : MonoBehaviour
{

    TextMeshProUGUI killCountText;
    private static KillCount instance;

    private void Awake()
    {
        instance = this;
        killCountText = GetComponent<TextMeshProUGUI>();
    }

    private void addKills(int kills)
    {
        //TooltipTop.ShowTooltipTop_Static("Trying to add kill");
        int currentKillsInt;
        int newKillsInt;
        string currentKillsString;
        string newKillsString;

        killCountText = GetComponent<TextMeshProUGUI>();
        currentKillsString = killCountText.text;
        //TooltipTop.ShowTooltipTop_Static(currentKillsString);
        currentKillsInt = int.Parse(currentKillsString);

        newKillsInt = currentKillsInt + kills;
        
        newKillsString = newKillsInt.ToString();
        killCountText.text = newKillsString;
    }

    // Adds a point to the bank
    public static void addKills_Static(int kills)
    {
        instance.addKills(kills);
    }

}
