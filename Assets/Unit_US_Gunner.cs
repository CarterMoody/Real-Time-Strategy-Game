using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_US_Gunner : Unit_AI
{
    
    // Start is called before the first frame update
    void Start()
    {
        setAttackDamage(30f);
        setAttackRange(20f);
        setReloadTime(10f);
        setRateOfFire(.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
