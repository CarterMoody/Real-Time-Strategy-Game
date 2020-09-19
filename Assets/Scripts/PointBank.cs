using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PointBank : MonoBehaviour
{

    private static PointBank instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void addPoints(int points)
    {
        int currentPointsInt;
        int newPointsInt;
        Text currentPointsText;
        string currentPointsString;
        string newPointsString;

        currentPointsText = GameObject.Find("Points_Count").GetComponent<Text>();
        currentPointsString = currentPointsText.text;
        currentPointsInt = int.Parse(currentPointsString);

        newPointsInt = currentPointsInt + points;
        
        newPointsString = newPointsInt.ToString();
        currentPointsText.text = newPointsString;
    }

    // Adds a point to the bank
    public static void addPoints_Static(int points)
    {
        instance.addPoints(points);
    }


    private bool subtractPoints(int points)
    {
        int currentPointsInt;
        int newPointsInt;
        Text currentPointsText;
        string currentPointsString;
        string newPointsString;

        currentPointsText = GameObject.Find("Points_Count").GetComponent<Text>();
        currentPointsString = currentPointsText.text;
        currentPointsInt = int.Parse(currentPointsString);

        newPointsInt = currentPointsInt - points;
        if (newPointsInt < 0)
        {
            TooltipTop.ShowTooltipTop_Static("Not enough points!");
            return false;
        }
        
        newPointsString = newPointsInt.ToString();
        currentPointsText.text = newPointsString;
        return true;
    }
    // This will return false if not enough points to buy
    public static bool subtractPoints_Static(int points)
    {
        return instance.subtractPoints(points);
    }
}
