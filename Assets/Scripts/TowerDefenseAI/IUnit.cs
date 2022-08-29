using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{

    //IUnit GetClosestUnit(Vector3 position, float maxRange);

    Vector3 GetPosition();

    //void Damage(int damageAmount);

    bool IsDead();

    void Damage(int damageAmount, Vector3 damageDirection, IUnit attackingUnit);

    void useAmmo(int amount);

    void reloadAmmoComplete();

    IUnit GetClosestEnemy();

    void addEXP(int amount);

    int getEXP();

    void Pin();

    string GetTeam();



    // Setter Methods (Public Variables can also be controlled in Unity GUI)
    // Just make sure you save the scene!
    void setAttackDamage(float attackDamage);
    void setAttackRange(float attackRange);
    void setReloadTime(float reloadTime);
    void setRateOfFire(float rateOfFire);
    void setMinimumDamage(float minimumDamage);
    void SetMaximumDamage(float maximumDamage);
    void setStartingHealth(float startingHealth);
    void setMaxHealth(float maxHealth);
    void setStartingAmmo(float startingAmmo);
    void setMaxAmmo(float maxAmmo);
    void setStartingEXP(float startingEXP);
    void setMaxEXP(float maxEXP);
    void setStartingRifleSkill(float startingRifleSkill);
    void setMaxRifleSKill(float maxRifleSkill);
    // Getter Methods
    float getAttackDamage();
    float getAttackRange();
    float getReloadTime();
    float getRateOfFire();
    float getMinimumDamage();
    float getMaximumDamage();
    float getStartingHealth();
    float getMaxHealth();
    float getStartingAmmo();
    float getMaxAmmo();
    float getStartingEXP();
    float getMaxEXP();
    float getStartingRifleSkill();
    float getMaxRifleSKill();


}
