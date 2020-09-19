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

    float getRange();
    float getDamage();
    float getRateOfFire();
    float getReloadTime();

    void Pin();

    string GetTeam();

}
