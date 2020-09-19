using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DE_Spawner : MonoBehaviour
{
    private static DE_Spawner instance;

    


    // Here are all the troops to populate
    [SerializeField] private GameObject DESoldierPrefab;

    [SerializeField] private int startPositionY;
    [SerializeField] private bool immediatelySpawnTroops;

    void Awake()
    {
        instance = this;
        if (startPositionY == 0){
            startPositionY = 17;
        }


    }
    // Start is called before the first frame update
    void Start()
    {
    if (immediatelySpawnTroops){
        spawnTroops();
    }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void spawnTroops()
    {
        int waveNum;
        int maxToSpawn;

        waveNum = WaveTracker.getWaveCount_Static();
        maxToSpawn = Math.Max( (waveNum / 10), 4);
        int amountToSpawn = UnityEngine.Random.Range(1, maxToSpawn);
        int spawnNum = 0;
        
        while (spawnNum < amountToSpawn)
        {
            //Enemy.EnemyType enemyType = Enemy.EnemyType.Orange; // fake enemy type for now
            float randPosition = UnityEngine.Random.Range(-8.0f, 8.0f); // 16 is current size map, centered on 0
            Vector3 spawnPosition = new Vector3(randPosition, startPositionY, 0);

            // Spawn a unit
            Unit_DE unit_DE = Unit_DE.Create(spawnPosition);
            spawnNum++;
        }
    }

    public static void spawnTroops_Static()
    {
        instance.spawnTroops();
    }
}
