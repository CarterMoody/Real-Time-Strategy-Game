using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;


// This is the main logic for US soldiers
public class Unit_AI : MonoBehaviour, IUnit
{
    private static Unit_AI instance;

    private IUnit iUnit;
    private IMoveRotation iMoveRotation;
    public AudioSource source;
    public AudioClip AR15_Shot;
    public AudioClip AR15_Reload;

    public AudioClip randomClip;
    public AudioClip[] FleshHits;

    // Used to be in specific Unit class... may need moving
    // At least these are the main default states...
    //  Possibly create a specific unit state variable at a lower Unit specific level
    //  to handle special states
    public enum State {
        Normal,
        Attacking,
        Moving,
        Busy,
        Reloading,
        Pinned,
        Aiming
    }

     public State state;

// Need to switch to enum and possibly put in Unit_AI
/*     public enum Team{
        US,
        DE
    }

    public Team team_; */
    public string team_;


    private Vector3 projectileShootFromPosition_;
    private Vector3 projectileExtractorPosition_;

    // Unit Stat Boundaries (Initial and Max)
    private float minimumDamage_;
    private float maximumDamage_;
    private float startingHealth_;
    private float maxHealth_;
    private float startingAmmo_;
    private float maxAmmo_;
    private float startingEXP_;
    private float maxEXP_;
    private float startingRifleSkill_;
    private float rifleSkill_;
    private float maxRifleSkill_;

    // Shooting
    private float shootTimerMax_;
    private float shootTimer_;
    private float attackDamage_;
    private float attackRange_;
    private float rateOfFire_;

    // Reloading
    private float reloadTimer_;
    private float reloadTime_;
    private float reloadTimerMax_;
    private bool reloading_;

    // Targeting
    private bool enemyInRange_; // used to determine if should rotate based on movement or enemy position.
    private bool targeting_;
    private bool foundTarget_;
    private float targetTimer_;
    private float targetTime_;

    // Pinning
    private float pinnedTimer_;
    private float pinnedTime_;
    private bool pinned_;

    // Stat Tracking Systems
    // May want to privatize and control here eventually...
    public HealthSystem healthSystem;
    public AmmoSystem ammoSystem;
    public EXPSystem expSystem;
    public RifleSkillSystem rifleSkillSystem;

    private void Awake()
    {
        instance = this;
        projectileShootFromPosition_ = transform.Find("ProjectileShootFromPosition").position;
        projectileExtractorPosition_ = transform.Find("ProjectileExtractorPosition").position;

        enemyInRange_ = false;
        reloading_ = false;


        // Get Components for use later
        iUnit = GetComponent<IUnit>();
        iMoveRotation = GetComponent<IMoveRotation>();
        source = GetComponent<AudioSource>();           // Gets the component responsible for playing Sounds

        //range = iUnit.getRange();           // Range
        //damageAmount = iUnit.getDamage();   // Damage
        //shootTimerMax = iUnit.getRateOfFire();   // Rate of Fire

        // Reload
        //reloadTimerMax = iUnit.getReloadTime(); // Time to Reload
        //reloadTimer = reloadTimerMax;


    }

    private void Update()
    {
        projectileShootFromPosition_ = transform.Find("ProjectileShootFromPosition").position;
        projectileExtractorPosition_ = transform.Find("ProjectileExtractorPosition").position;
        if (Input.GetMouseButtonDown(0))
        {
            //CMDebug.TextPopupMouse("Click!");
        }

        //attack();
        shootTimer_ -= Time.deltaTime;

        if (reloading_){
            reloadTimer_ -= Time.deltaTime;
        }

        if (targeting_){
            targetTimer_ -= Time.deltaTime;
        }        
    }

    public bool Target(){
        if (!targeting_){
            targeting_ = true;
        }
        if (targetTimer_ <= 0f){
            targetTimer_ = targetTime_;
            return true;
        }
        else return false;
    }

    public void attack()
    {
        
        IUnit enemy = iUnit.GetClosestEnemy();
        if (enemy != null) { // ENEMY IN RANGE
            enemyInRange_ = true;

            // rotate based on position of enemy
            Vector3 targetPosition = enemy.GetPosition();
            Vector3 rotationDir = (targetPosition - transform.position).normalized;
            iMoveRotation.SetRotation(rotationDir);

            if (shootTimer_ <= 0f) {
                shootTimer_ = rateOfFire_;
                // Calculate damage based on range
                float currentDistanceToEnemy = Vector3.Distance(enemy.GetPosition(), projectileShootFromPosition_);
                float percentDistanceToEnemyOfRange = currentDistanceToEnemy / attackRange_;
                float rangeAdjustedaccuracy = (1f - percentDistanceToEnemyOfRange);
                float rifleSkillAdjustedAccuracy = (rifleSkill_ * .01f) + rangeAdjustedaccuracy;
                float accuracy = rifleSkillAdjustedAccuracy;
                if (accuracy > 1f){
                    accuracy = 1f;
                }
                int rangeAdjustedDamage = (int) (attackDamage_ * accuracy);
                
                IUnit attackingUnit = iUnit;
                int attackingUnitEXP = attackingUnit.getEXP();
                //Debug.Log("attackingUnit current Exp: " + attackingUnitEXP);
                int expAdjustedDamage = rangeAdjustedDamage + attackingUnitEXP;

                int finalAdjustedDamage = expAdjustedDamage;
                if (finalAdjustedDamage < minimumDamage_){
                    finalAdjustedDamage = (int) minimumDamage_;
                }
                
                ProjectileBulletDefault.Create(projectileShootFromPosition_, attackingUnit, enemy, finalAdjustedDamage, accuracy);
                iUnit.useAmmo(1);
                // Play shooting sound
                source.PlayOneShot(AR15_Shot);
                

                // Spawn Shell
                // get direction to spawn shell
                Vector3 shellMoveDir = UtilsClass.ApplyRotationToVector(rotationDir, -120f);
                float randomAngleTweak = Random.Range(-10.0f, 10.0f);
                shellMoveDir = UtilsClass.ApplyRotationToVector(shellMoveDir, randomAngleTweak);
                ShellParticleSystemHandler.Instance.SpawnShell(projectileExtractorPosition_, shellMoveDir);
            }
        }
        else { // ENEMY NOT IN RANGE
            enemyInRange_ = false;
            //Debug.Log("Enemy not in range");
        }        
    }

    public void ExecuteReload()
    {
        
        Vector3 randomDirection = UtilsClass.GetRandomDir();
        Vector3 magPosition = iUnit.GetPosition();
        //Debug.Log("Mag Position: " + magPosition);
        //Debug.Log("randomDirection: " + randomDirection);
        if (!reloading_){
            MagParticleSystemHandler.Instance.SpawnMag(magPosition, randomDirection);
            reloading_ = true;
            // Play Reloading sound
            source.PlayOneShot(AR15_Reload);
        }

        if (reloadTimer_ <= 0f) {
            reloading_ = false;
            reloadTimer_ = reloadTime_;
            //GetComponent<Unit>().reloadAmmoComplete();
            iUnit.reloadAmmoComplete();
        }
    }





    //private void OnMouseEnter() {
    //    UpgradeOverlay.Show_Static(this);
    //
    public static bool getStatus_enemyInRange_Static()
    {
        return instance.enemyInRange_;
    }

    public static bool getStatus_reloading()
    {
        return instance.reloading_;
    }

    public void UpgradeRange(float amount) {
        attackRange_ += amount;
    }




    public void UpgradeAttackDamage(float amount) {
        attackDamage_ += amount;
    }


    public void setRandomRifleSkill(float lowBound, float highBound){
        rifleSkill_ = Random.Range(lowBound, highBound);
    }


    // Setter Methods (Public Variables can also be controlled in Unity GUI)
    // Just make sure you save the scene!
    public void SetMinimumDamage(float amount){
        minimumDamage_ = amount;
    }
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
        this.maxEXP_ = maxEXP;
    }
    public void setStartingRifleSkill(float startingRifleSkill)
    {
        this.startingRifleSkill_ = startingRifleSkill;
    }    
    public void setMaxRifleSKill(float maxRifleSkill)
    {
        this.maxRifleSkill_ = maxRifleSkill;
    }

    // Getter Methods

    public float getAttackDamage()
    {
        return this.attackRange_;
    }
    public float getAttackRange()
    {
        return this.attackRange_;
    }
    public float getReloadTime()
    {
        return this.reloadTime_;
    }
    public float getRateOfFire()
    {
        return this.rateOfFire_;
    }
    public float getMinimumDamage()
    {
        return this.minimumDamage_;
    }
    public float getMaximumDamage()
    {
        return this.maximumDamage_;
    }
    public float getStartingHealth()
    {
        return this.startingHealth_;
    }
    public float getMaxHealth()
    {
        return this.maxHealth_;
    }
    public float getStartingAmmo()
    {
        return this.startingAmmo_;
    }
    public float getMaxAmmo()
    {
        return this.maxAmmo_;
    }
    public float getStartingEXP()
    {
        return this.startingEXP_;
    }
    public float getMaxEXP()
    {
        return this.maxEXP_;
    }
    public float getStartingRifleSkill()
    {
        return this.startingRifleSkill_;
    }    
    public float getMaxRifleSKill()
    {
        return this.maxRifleSkill_;
    }

    // From Unit_US during migration to this class

    public Vector3 GetPosition() {
        return transform.position;
    }

    public bool IsDead() {
        return healthSystem.IsDead();
    }

    public bool IsOutOfAmmo()
    {
        return ammoSystem.IsOutOfAmmo();
    }



    public void SetStateNormal() {
        SetSpriteNormal();
        state = State.Normal;
    }

    public void SetStateAttacking() {
        state = State.Attacking;
    }

    public void SetStateReloading()
    {
        state = State.Reloading;
    }

    public void SetStatePinned()
    {
        SetSpritePinned();
        state = State.Pinned;
    }

    public void SetStateAiming()
    {
        //SetSpriteAiming();
        state = State.Aiming;
    }

    public void RotateTowards(Vector3 targetPosition){
        Vector3 rotationDir = (targetPosition - transform.position).normalized;
        iMoveRotation.SetRotation(rotationDir);
    }

    // Does virtual mean it can be overridden?
    public virtual void Damage(int damageAmount, Vector3 damageDirection, IUnit attackingUnit) {
        Vector3 bloodDir = UtilsClass.GetRandomDir();
        //Blood_Handler.SpawnBlood(GetPosition(), bloodDir);
        BloodParticleSystemHandler.Instance.SpawnBlood(transform.position, damageDirection);

        DamagePopup.Create(GetPosition(), damageAmount, false);
        // Play random Hit Sound
        randomClip = FleshHits[UnityEngine.Random.Range(0, FleshHits.Length)];
        source.PlayOneShot(randomClip);
        

        healthSystem.Damage(damageAmount);
        if (IsDead()) {
            //TooltipTop.ShowTooltipTop_Static("Unit_US Killed"); // possibly name of unit died?
            //KillCount.addKills_Static(1);
            // Add to death count statistic
            //FlyingBody.Create(GameAssets.i.pfUnit_USFlyingBody, GetPosition(), bloodDir);

            // Give EXP to attackingUnit
            attackingUnit.addEXP(1);
            Destroy(gameObject);
            
        } else {
            Pin();
            // Knockback
            //transform.position += bloodDir * .0025f;
        }
    }

    public virtual void useAmmo(int amount)
    {
        //Debug.Log("amount bullets used: " + amount.ToString());
        ammoSystem.UseAmmo(amount);
        int currentAmmo = ammoSystem.GetAmmo();
        //Debug.Log("Amount ammo left: " + currentAmmo.ToString());
        if (IsOutOfAmmo()){
            //Debug.Log("Setting State To Reloading");     
            GetComponent<IMovePosition>().resumeMovement(); // cancel a pause on movement if any
            SetStateReloading();
        }
    }

    public void reloadAmmoComplete()
    {
        ammoSystem.ReloadComplete();
    }

    public void Reload() // Called repeatedly when in StateReloading
    {
        int currentAmmo = ammoSystem.GetAmmo();
        if (IsOutOfAmmo()){
            ExecuteReload(); // begin reloading
        }
        else{ // finished reloading 
            SetStateNormal();
        }


    }

    // This will likely need fixing...
    // Need to make this pure virtual with only implementation in child class maybe?
    public virtual IUnit GetClosestEnemy() {
        if (team_ == "US")
        {
            return Unit_DE.GetClosestUnit_DE(transform.position, getAttackRange());
        }
        else if (team_ == "DE")
        {
            return Unit_US.GetClosestUnit_US(transform.position, getAttackRange());
        }
        return null;
        
        //
        //Debug.Log("Need to Implement GetClosestEnemy in Specific Unit_US or Unit_DE class?");

    }

    public void addEXP(int amount){
        //Debug.Log("Receiving EXP: " + amount);
        expSystem.addEXP(amount);
    }

    public int getEXP(){
        return expSystem.GetEXP();
    }

    // likely needs fixing
    public void Pin(){
        pinnedTimer_ = pinnedTime_;
        pinned_ = true; 
        SetStatePinned();
    }

    public virtual string GetTeam(){
        print("Need to set in specific Unit class");
        return team_;
    }
    public void setTeam(string team)
    {
        team_ = team;
    }

    public virtual void SetSpriteNormal(){
        //transform.Find("Body").GetComponent<SpriteRenderer>().sprite = normalSprite;
        // implement this in specific class please!
        Debug.Log("Implement in specific Unit class");
    }
    public virtual void SetSpritePinned(){
        //transform.Find("Body").GetComponent<SpriteRenderer>().sprite = normalSprite;
        // implement this in specific class please!
        Debug.Log("Implement in specific Unit class");
    }

}