using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;


// This is the main logic for US soldiers
public class Unit_AI : MonoBehaviour
{


    private static Unit_AI instance;

    private IUnit iUnit;

    private IMoveRotation iMoveRotation;


    private Vector3 projectileShootFromPosition;

    private float attackRange;
    private float attackDamage;
    private float minimumDamage;
    private float rifleSkill;
    private float shootTimerMax;
    private float shootTimer;
    private float reloadTimer;
    private float reloadTimerMax;
    private float rateOfFire;
    private float reloadTime;
    private bool reloading;
    private bool enemyInRange; // used to determine if should rotate based on movement or enemy position.
    private bool targeting;
    private bool foundTarget;
    private float targetTimer;
    private float targetTime;
    private void Awake()
    {
        instance = this;
        projectileShootFromPosition = transform.Find("ProjectileShootFromPosition").position;

        enemyInRange = false;
        reloading = false;


        // Get Components for use later
        iUnit = GetComponent<IUnit>();
        iMoveRotation = GetComponent<IMoveRotation>();

        //range = iUnit.getRange();           // Range
        //damageAmount = iUnit.getDamage();   // Damage
        //shootTimerMax = iUnit.getRateOfFire();   // Rate of Fire

        // Reload
        //reloadTimerMax = iUnit.getReloadTime(); // Time to Reload
        //reloadTimer = reloadTimerMax;


    }

    private void Update()
    {
        projectileShootFromPosition = transform.Find("ProjectileShootFromPosition").position;
        if (Input.GetMouseButtonDown(0))
        {
            //CMDebug.TextPopupMouse("Click!");
        }

        //attack();
        shootTimer -= Time.deltaTime;

        if (reloading){
            reloadTimer -= Time.deltaTime;
        }

        if (targeting){
            targetTimer -= Time.deltaTime;
        }        
    }

    public bool Target(){
        if (!targeting){
            targeting = true;
        }
        if (targetTimer <= 0f){
            targetTimer = targetTime;
            return true;
        }
        else return false;
    }

    public void attack()
    {
        
        IUnit enemy = iUnit.GetClosestEnemy();
        if (enemy != null) { // ENEMY IN RANGE
            enemyInRange = true;

            // rotate based on position of enemy
            Vector3 targetPosition = enemy.GetPosition();
            Vector3 rotationDir = (targetPosition - transform.position).normalized;
            iMoveRotation.SetRotation(rotationDir);

            if (shootTimer <= 0f) {
                shootTimer = rateOfFire;
                // Calculate damage based on range
                float currentDistanceToEnemy = Vector3.Distance(enemy.GetPosition(), projectileShootFromPosition);
                float percentDistanceToEnemyOfRange = currentDistanceToEnemy / attackRange;
                float rangeAdjustedaccuracy = (1f - percentDistanceToEnemyOfRange);
                float rifleSkillAdjustedAccuracy = (rifleSkill * .01f) + rangeAdjustedaccuracy;
                float accuracy = rifleSkillAdjustedAccuracy;
                if (accuracy > 1f){
                    accuracy = 1f;
                }
                int rangeAdjustedDamage = (int) (attackDamage * accuracy);
                
                IUnit attackingUnit = iUnit;
                int attackingUnitEXP = attackingUnit.getEXP();
                //Debug.Log("attackingUnit current Exp: " + attackingUnitEXP);
                int expAdjustedDamage = rangeAdjustedDamage + attackingUnitEXP;

                int finalAdjustedDamage = expAdjustedDamage;
                if (finalAdjustedDamage < minimumDamage){
                    finalAdjustedDamage = (int) minimumDamage;
                }
                
                ProjectileBulletDefault.Create(projectileShootFromPosition, attackingUnit, enemy, finalAdjustedDamage, accuracy);
                iUnit.useAmmo(1);

                // Spawn Shell
                // get direction to spawn shell
                Vector3 shellMoveDir = UtilsClass.ApplyRotationToVector(rotationDir, -120f);
                float randomAngleTweak = Random.Range(-10.0f, 10.0f);
                shellMoveDir = UtilsClass.ApplyRotationToVector(shellMoveDir, randomAngleTweak);
                Vector3 extractorPosition = projectileShootFromPosition + new Vector3(.25f, .25f, 0);
                ShellParticleSystemHandler.Instance.SpawnShell(extractorPosition, shellMoveDir);
            }
        }
        else { // ENEMY NOT IN RANGE
            enemyInRange = false;
            //Debug.Log("Enemy not in range");
        }        
    }

    public void Reload()
    {
        
        Vector3 randomDirection = UtilsClass.GetRandomDir();
        Vector3 magPosition = iUnit.GetPosition();
        //Debug.Log("Mag Position: " + magPosition);
        //Debug.Log("randomDirection: " + randomDirection);
        if (!reloading){
            MagParticleSystemHandler.Instance.SpawnMag(magPosition, randomDirection);
            reloading = true;
        }

        if (reloadTimer <= 0f) {
            reloading = false;
            reloadTimer = reloadTime;
            //GetComponent<Unit>().reloadAmmoComplete();
            iUnit.reloadAmmoComplete();
        }
    }





    //private void OnMouseEnter() {
    //    UpgradeOverlay.Show_Static(this);
    //
    public static bool getStatus_enemyInRange_Static()
    {
        return instance.enemyInRange;
    }

    public static bool getStatus_reloading()
    {
        return instance.reloading;
    }

    public float GetAttackRange() {
        return attackRange;
    }

    public void SetAttackRange(float amount){
        attackRange = amount;
    }
    public void UpgradeRange(float amount) {
        attackRange += amount;
    }

    public void SetAttackDamage(float amount){
        attackDamage = amount;
    }

    public void SetMinimumDamage(float amount){
        minimumDamage = amount;
    }

    public void UpgradeAttackDamage(float amount) {
        attackDamage += amount;
    }

    public float GetRateOfFire(){
        return rateOfFire;
    }
    public void SetRateOfFire(float amount){
        rateOfFire = amount;
    }

    public void SetRandomRifleSkill(float lowBound, float highBound){
        rifleSkill = Random.Range(lowBound, highBound);
    }
    public float GetReloadTime(){
        return reloadTime;
    }
    
    public void SetReloadTime(float amount){
        reloadTime = amount;
    }


}
