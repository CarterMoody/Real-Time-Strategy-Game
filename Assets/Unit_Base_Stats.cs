using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Base_Stats : MonoBehaviour
{

    // Values with Setter Methods that can be updated
    public float attackDamage_;
    public float attackRange_;
    public float reloadTime_;
    public float rateOfFire_;
    public float minimumDamage_;
    public float maximumDamage_;

    // Values at initialization of Unit that should never change
    public float startingHealth_;
    public float maxHealth;
    public float startingAmmo_;
    public float maxAmmo;
    public float startingEXP_;
    public float maxEXP_;
    public float startingRifleSkill_;
    public float maxRifleSkill_;

    // Setter Methods (Public Variables can also be controlled in Unity GUI)
    // Just make sure you save the scene!
    public void setAttackDamage(float attackDamage)
    {
        this.attackDamage_ = attackDamage;
    }
    public void setAttackRange(float attackRange)
    {
        this.attackRange_ = attackRange;
    }
    public void setReloadTime(float reloadTime)
    {
        this.reloadTime_ = reloadTime;
    }
    public void setRateOfFire(float rateOfFire)
    {
        this.rateOfFire_ = rateOfFire;
    }
    public void setMinimumDamage(float minimumDamage)
    {
        this.minimumDamage_ = minimumDamage;
    }
    public void SetMaximumDamage(float maximumDamage)
    {
        this.maximumDamage_ = maximumDamage;
    }
    public void setStartingHealth(float startingHealth)
    {
        this.startingHealth_ = startingHealth;
    }
    public void setMaxHealth(float maxHealth)
    {
        this.maxHealth_ = maxHealth;
    }
    public void setStartingAmmo(float startingAmmo)
    {
        this.startingAmmo_ = startingAmmo;
    }
    public void setMaxAmmo(float maxAmmo)
    {
        this.maxAmmo_ = maxAmmo;
    }
    public void setStartingEXP(float startingEXP)
    {
        this.startingEXP_ = startingEXP;
    }
    public void setMaxEXP(float maxEXP)
    {
        this.startingRifleSKill_ = startingRifleSkill;
    }
    public void setStartingRifleSkill(float startingRifleSkill)
    {
        this.startingRifleSKill_ = startingRifleSkill;
    }    
    public void setMaxRifleSKill(float maxRifleSkill)
    {
        this.maxRifleSKill_ = maxRifleSkill;
    }

    // Getter Methods
    public float getAttackDamage(float attackDamage)
    {
        return this.attackRange
    }
    public float getAttackRange(float attackRange)
    {
        this.attackRange_;
    }
    public float setReloadTime(float reloadTime)
    {
        return this.reloadTime_;
    }
    public float getRateOfFire(float rateOfFire)
    {
        return this.rateOfFire_;
    }
    public float getMinimumDamage(float minimumDamage)
    {
        return this.minimumDamage_;
    }
    public float getMaximumDamage(float maximumDamage)
    {
        return this.maximumDamage_;
    }
    public float getStartingHealth(float startingHealth)
    {
        return this.startingHealth_;
    }
    public float getMaxHealth(float maxHealth)
    {
        return this.maxHealth_;
    }
    public float getStartingAmmo(float startingAmmo)
    {
        return this.startingAmmo_;
    }
    public float getMaxAmmo(float maxAmmo)
    {
        return this.maxAmmo_;
    }
    public float getStartingEXP(float startingEXP)
    {
        return this.startingEXP_;
    }
    public float getMaxEXP(float maxEXP)
    {
        return this.startingRifleSKill_;
    }
    public float getStartingRifleSkill(float startingRifleSkill)
    {
        return this.startingRifleSKill_;
    }    
    public float getMaxRifleSKill(float maxRifleSkill)
    {
        return this.maxRifleSKill_;
    }

    // Start is called before the first frame update
    void Start()
    {
        setAttackDamage(20f);       // Units typically have 100f health
        setAttackRange(20f);        // The entire map is 32f Height by 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
