using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class WaveTracker : MonoBehaviour
{

    private static WaveTracker instance;

    private float globalTimer;
    private int waveCounter;
    private float sizeWaveCounterOriginal;
    private float sizeWaveCounter;
    private float waveRunTimeFixedUpdates;
    [SerializeField] private float waveRunTimeSeconds;  // Serializefield makes it show up in the inspector
    private float waveRunTimeRemainingPercent;
    private float newSizeWaveCounter;
    private float sizeWaveCounterDifference;
    private Vector3 originalScale;

    public GameObject Waves_Count_TMP;
    //public GameObject Point_Count;


    private void Awake()
    {
        instance = this;
        //var de_soldier = Resources.Load("DEsoldier");
        //DESoldierPrefabResource = de_soldier as GameObject;
    }


    // Start is called before the first frame update
    private void Start()
    {
        globalTimer = 0;
        waveCounter = 0;
        sizeWaveCounterOriginal = transform.localScale.x;
        originalScale = transform.localScale;


        // Edit waveRunTimeSeconds to change the duration of a wave
        //waveRunTimeSeconds = 10;
        waveRunTimeFixedUpdates = waveRunTimeSeconds * 50;

        //Debug.Log(sizeWaveCounter);
        
    }

    // Update is called once per frame
    private void Update()
    {
        //transform.localScale = transform.localScale + new Vector3(-.0005f, 0, 0);
    }

    // late update is called once per frame, after regular Update() finishes
    private void LateUpdate()
    {
        
    }

    // FixedUpdate is called once per Fixed TimeStep (Edit > Project Settings > Fixed Timestep)
    private void FixedUpdate()
    {
        globalTimer += 1;
        //Debug.Log("globalTimer");
        //Debug.Log(globalTimer);

        updateWaveTrackerSize();

        if (globalTimer == waveRunTimeFixedUpdates) // waveRunTimeSeconds has elapsed
        {
            globalTimer = 0;
            Debug.Log("WaveRunTimeSeconds has elapsed!");
            newWave();
        }
    }
    
    private void updateWaveTrackerSize()
    {
        // figure out what percent of the wave remains
        waveRunTimeRemainingPercent = (1f - (globalTimer / waveRunTimeFixedUpdates));
        //waveRunTimeRemainingPercent = (globalTimer / waveRunTimeFixedUpdates);
        //Debug.Log("waveRunTimeRemainingPercent");
        //Debug.Log(waveRunTimeRemainingPercent);
        newSizeWaveCounter = (sizeWaveCounterOriginal * waveRunTimeRemainingPercent);
        //Debug.Log("newSizeWaveCounter");
        //Debug.Log(newSizeWaveCounter);
        sizeWaveCounterDifference = newSizeWaveCounter - sizeWaveCounterOriginal;
        //Debug.Log("sizeWaveCounterDifference");
        //Debug.Log(sizeWaveCounterDifference);
        // adjust the size of the wave counter accordingly
        transform.localScale = originalScale + new Vector3(sizeWaveCounterDifference, 0, 0);
    }

    // Starts process of a newWave
    private void newWave()
    {
        waveCounter++;
        visibleWaveCounterIncrease();
        addWavePoints();
        //TooltipTop.ShowTooltipTop_Static("Trying to spawnNewWave");
        Debug.Log("trying spawnNewWave()");
        spawnNewWave();
    }


    private void addWavePoints()
    {
        // Add 2 points if < 10 waves
        if (waveCounter < 5)
        {
            PointBank.addPoints_Static(2);
        }
        else
        {
            PointBank.addPoints_Static(1);
        }
    }

    private void visibleWaveCounterIncrease()
    {
        string waveCounterString = waveCounter.ToString();
        //visibleWaveCounter = GameObject.Find("Waves_Count_TMP");
        //Debug.Log(Waves_Count_TMP);
        TextMeshProUGUI textmeshPro = Waves_Count_TMP.GetComponent<TextMeshProUGUI>();
        //Debug.Log(textmeshPro);
        textmeshPro.SetText(waveCounterString);

        //Waves_Count_TMP.GetComponent<TextMeshPro>().SetText(waveCounterString);
    }


    // Spawns necessary troops for the new wave
    private void spawnNewWave()
    {
        DE_Spawner.spawnTroops_Static();
    }

    public static int getWaveCount_Static()
    {
        return instance.waveCounter;
    }



}
