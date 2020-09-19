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

public class AmmoSystem {

    public event EventHandler OnAmmoChanged;
    public event EventHandler OnAmmoMaxChanged;
    public event EventHandler OnDamaged;
    public event EventHandler OnReloaded;
    public event EventHandler OnOutOfAmmo;

    private int ammoMax;
    private int ammo;

    public AmmoSystem(int ammoMax) {
        this.ammoMax = ammoMax;
        ammo = ammoMax;
    }

    public int GetAmmo() {
        return ammo;
    }

    public int GetAmmoMax() {
        return ammoMax;
    }

    public float GetAmmoNormalized() {
        return (float)ammo / ammoMax;
    }

    public void UseAmmo(int amount) {
        ammo -= amount;
        if (ammo < 0) {
            ammo = 0;
        }
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (ammo <= 0) {
            //Die();
            // Set to reload?
        }
    }

    public void OutOfAmmo() {
        OnOutOfAmmo?.Invoke(this, EventArgs.Empty);
    }

    public bool IsOutOfAmmo() {
        return ammo <= 0;
    }

    public void Reload(int amount) {
        ammo += amount;
        if (ammo > ammoMax) {
            ammo = ammoMax;
        }
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
        OnReloaded?.Invoke(this, EventArgs.Empty);
    }



    public void ReloadComplete() {
        ammo = ammoMax;
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
        OnReloaded?.Invoke(this, EventArgs.Empty);
    }

    public void SetAmmoMax(int ammoMax, bool fullAmmo) {
        this.ammoMax = ammoMax;
        if (fullAmmo) ammo = ammoMax;
        OnAmmoMaxChanged?.Invoke(this, EventArgs.Empty);
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
    }

}
