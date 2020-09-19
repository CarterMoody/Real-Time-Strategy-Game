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

public class EXPSystem {

    public event EventHandler OnEXPChanged;
    public event EventHandler OnEXPMaxChanged;
    public event EventHandler OnDamaged;
    public event EventHandler OnReloaded;
    public event EventHandler OnOutOfEXP;

    private int expMax;
    private int exp;

    public EXPSystem(int expMax) {
        this.expMax = expMax;
        exp = 0;
    }

    public int GetEXP() {
        return exp;
    }

    public int GetEXPMax() {
        return expMax;
    }

    public float GetEXPNormalized() {
        return (float)exp / expMax;
    }

    public void UseEXP(int amount) {
        exp -= amount;
        if (exp < 0) {
            exp = 0;
        }
        OnEXPChanged?.Invoke(this, EventArgs.Empty);
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (exp <= 0) {
            //Die();
            // Set to reload?
        }
    }

    public void OutOfEXP() {
        OnOutOfEXP?.Invoke(this, EventArgs.Empty);
    }

    public bool IsOutOfEXP() {
        return exp <= 0;
    }

    public void addEXP(int amount) {
        //Debug.Log("EXPSystem current EXP: " + exp);
        //Debug.Log("EXPSystem adding EXP: " + amount);
        exp += amount;
        //if (exp > expMax) {   // Limit the amount of EXP?
        //    exp = expMax;
        //}
        //Debug.Log("EXPSystem current EXP: " + exp);
        OnEXPChanged?.Invoke(this, EventArgs.Empty);
        OnReloaded?.Invoke(this, EventArgs.Empty);
    }



    public void ReloadComplete() {
        exp = expMax;
        OnEXPChanged?.Invoke(this, EventArgs.Empty);
        OnReloaded?.Invoke(this, EventArgs.Empty);
    }

    public void SetEXPMax(int expMax, bool fullEXP) {
        this.expMax = expMax;
        if (fullEXP) exp = expMax;
        OnEXPMaxChanged?.Invoke(this, EventArgs.Empty);
        OnEXPChanged?.Invoke(this, EventArgs.Empty);
    }

}
