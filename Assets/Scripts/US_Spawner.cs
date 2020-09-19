using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class US_Spawner : MonoBehaviour
{

    private static US_Spawner instance;

    // Here are all the troops to populate
    [SerializeField] private GameObject USSoldierPrefab;

    [SerializeField] private int startingUnitQuantity;

    
    void Awake()
    {
        instance = this;
        if (startingUnitQuantity == 0){
            startingUnitQuantity = 4;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       spawnInitial(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void spawnInitial()
    {
        int amountToSpawn = startingUnitQuantity;
        int spawnNum = 0;
        while (spawnNum < amountToSpawn)
        {
            spawnTroops(Unit_US.Unit_USType.Soldier);
            spawnNum++;
        }
    }


    private void spawnTroops(Unit_US.Unit_USType troopType)
    {
        float randPosition = Random.Range(-8.0f, 8.0f); // 16 is current size map, centered on 0
        Vector3 spawnPosition = new Vector3(randPosition, -17, 0);
        Unit_US.Create(spawnPosition, troopType);
        
        
/*         switch (troopType)
        {
            case Unit_US.Unit_USType.Soldier:
                Unit_US.Create(spawnPosition, troopType); 
                break;
            case Unit_US.Unit_USType.Gunner:
                Unit_US.Create(spawnPosition, troopType); 
                break;
            default:
                Debug.Log("Invalid troopType to spawn: " + troopType);
                break;
        } */
    }


    public static void spawnTroops_Static(Unit_US.Unit_USType troopType)
    {
        instance.spawnTroops(troopType);
    }
}
