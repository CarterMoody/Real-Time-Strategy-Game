/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleSkillSystem {

    public event EventHandler OnRifleSkillChanged;
    public event EventHandler OnRifleSkillMaxChanged;
    public event EventHandler OnDamaged;
    public event EventHandler OnReloaded;
    public event EventHandler OnOutOfRifleSkill;

    private int expMax;
    private int exp;

    public RifleSkillSystem(int expMax) {
        this.expMax = expMax;
        exp = 0;
    }

    public int GetRifleSkill() {
        return exp;
    }

    public int GetRifleSkillMax() {
        return expMax;
    }

    public float GetRifleSkillNormalized() {
        return (float)exp / expMax;
    }

    public void UseRifleSkill(int amount) {
        exp -= amount;
        if (exp < 0) {
            exp = 0;
        }
        OnRifleSkillChanged?.Invoke(this, EventArgs.Empty);
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (exp <= 0) {
            //Die();
            // Set to reload?
        }
    }

    public void OutOfRifleSkill() {
        OnOutOfRifleSkill?.Invoke(this, EventArgs.Empty);
    }

    public bool IsOutOfRifleSkill() {
        return exp <= 0;
    }

    public void addRifleSkill(int amount) {
        //Debug.Log("RifleSkillSystem current RifleSkill: " + exp);
        //Debug.Log("RifleSkillSystem adding RifleSkill: " + amount);
        exp += amount;
        //if (exp > expMax) {   // Limit the amount of RifleSkill?
        //    exp = expMax;
        //}
        //Debug.Log("RifleSkillSystem current RifleSkill: " + exp);
        OnRifleSkillChanged?.Invoke(this, EventArgs.Empty);
        OnReloaded?.Invoke(this, EventArgs.Empty);
    }



    public void ReloadComplete() {
        exp = expMax;
        OnRifleSkillChanged?.Invoke(this, EventArgs.Empty);
        OnReloaded?.Invoke(this, EventArgs.Empty);
    }

    public void SetRifleSkillMax(int expMax, bool fullRifleSkill) {
        this.expMax = expMax;
        if (fullRifleSkill) exp = expMax;
        OnRifleSkillMaxChanged?.Invoke(this, EventArgs.Empty);
        OnRifleSkillChanged?.Invoke(this, EventArgs.Empty);
    }

}
