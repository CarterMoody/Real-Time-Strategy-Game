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
using System.Collections.Generic;
using UnityEngine;
//using V_AnimationSystem;
using CodeMonkey.Utils;

/*
 * Unit_US
 * 
 * Contains base state machine for Unit_US behavior
 *
 * */

// Enemy is replaced by Unit_US for US soldiers

public class Unit_US : MonoBehaviour, IUnit {
    

    public static List<Unit_US> unit_USList = new List<Unit_US>();

    public static Unit_US GetClosestUnit_US(Vector3 position, float maxRange) {
        Unit_US closest = null;
        foreach (Unit_US unit_US in unit_USList) {
            if (unit_US.IsDead()) continue;
            if (Vector3.Distance(position, unit_US.GetPosition()) <= maxRange) {
                if (closest == null) {
                    closest = unit_US;
                } else {
                    if (Vector3.Distance(position, unit_US.GetPosition()) < Vector3.Distance(position, closest.GetPosition())) {
                        closest = unit_US;
                    }
                }
            }
        }
        return closest;
    }


    public static Unit_US Create(Vector3 position) {
        Transform unitTransform = Instantiate(GameAssets.i.pfUnit_US, position, Quaternion.identity);

        Unit_US unitHandler = unitTransform.GetComponent<Unit_US>();

        return unitHandler;
    }
    
    //public static Unit_US Create(Vector3 position, Unit_USType unitType) {
    public static Unit_US Create(Vector3 position, Unit_USType unitType) {
        Transform unitTransform = Instantiate(GameAssets.i.pfUnit_US, position, Quaternion.identity);

        Unit_US unitHandler = unitTransform.GetComponent<Unit_US>();
        unitHandler.SetUnit_USType(unitType);

        return unitHandler;
    }

    public enum Unit_USType {
        Gunner,
        Soldier,
        Yellow,
        Orange,
        Red,
    }


    private Unit_AI unit_AI;

    private IMoveRotation iMoveRotation;
    private const float SPEED = 1f;

    private HealthSystem healthSystem;
    private AmmoSystem ammoSystem;
    private EXPSystem expSystem;
    private RifleSkillSystem rifleSkillSystem;
    private Character_Base characterBase;
    private State state;
    private Vector3 lastMoveDir;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    private float pathfindingTimer;
    //private Func<IUnitTargetable> getUnitTarget;



    private float attackRange;
    
    private float fastAttackRange;
    private float attackDamage;
    private float reloadTime;
    private float rateOfFire;
    private float shootTimer;

    private float pinnedTimer;
    private float pinnedTime;
    private bool pinned;

    private bool aiming;
    private float aimTime;
    private float aimTimer;
    private float aimTimeMax;
    private float aimTimeMin;

    public IUnit currentTargetEnemy; // set when AimEnemy is called
    public Vector3 currentTargetAim; // What positon the unit should be aiming at
    public Vector3 currentTargetPosition; // set when AimPosition is called
    private AudioSource source;
    private AudioClip randomClip;
    public AudioClip[] FleshHits;



    private bool enemyInRange; // used to determine if should rotate based on movement or enemy position.

    //private Unit_USAnimType idleUnit_USAnim;
    //private Unit_USAnimType walkUnit_USAnim;
    //private Unit_USAnimType hitUnit_USAnim;
    //private Unit_USAnimType attackUnit_USAnim;

    private enum State {
        Normal,
        Attacking,
        Moving,
        Busy,
        Reloading,
        Pinned,
        Aiming
    }

    [SerializeField] Sprite usSoldierSprite;
    [SerializeField] Sprite usGunnerSprite;
    Sprite normalSprite;

    [SerializeField] Sprite usPinnedSprite;

    private void Awake() {
        unit_USList.Add(this);
        characterBase = gameObject.GetComponent<Character_Base>();
        unit_AI = GetComponent<Unit_AI>();
        iMoveRotation = GetComponent<IMoveRotation>();
        //enemyInRange = false;
        pinnedTime = 2f;
        aimTime = 2f;
        aimTimeMax = 5f;
        aimTimeMin = 1f;

        SetStateNormal();

        source = GetComponent<AudioSource>();           // Gets the component responsible for playing Sounds


    }

    private void Start() {
        //*
        World_Bar healthBar = new World_Bar(transform, new Vector3(0, -.5f), new Vector3(.7f, .07f), Color.grey, Color.red, 1f, 1000, new World_Bar.Outline { color = Color.black, size = .05f });
        healthSystem.OnHealthChanged += (object sender, EventArgs e) => {
            healthBar.SetSize(healthSystem.GetHealthNormalized());
        };
        //*/
    }



    // colors the Unit_US?
    private void SetUnit_USType(Unit_USType unitType) {
        //Material material;

        switch (unitType) {
        default:
        case Unit_USType.Soldier:
            //transform.Find("Body").GetComponent<SpriteRenderer>().sprite = GameAssets.i.s_USSoldier;
            normalSprite = usSoldierSprite;
            SetSpriteNormal();
            //material = 
            //ammoSystem.SetAmmoMax(100, true);
            healthSystem = new HealthSystem(100); // give 100 health
            ammoSystem = new AmmoSystem(5); // give 5 ammo
            expSystem = new EXPSystem(100); // set Max EXP to 100
            rifleSkillSystem = new RifleSkillSystem(100); // Set Max Rifle Skill to 100
            //attackDamage = 55;
            //attackRange = 20f;
            //fastAttackRange = 15f;
            //reloadTime = 5f;
            //rateOfFire = 1f;
            unit_AI.SetAttackDamage(50f);
            unit_AI.SetAttackRange(20f);
            unit_AI.SetReloadTime(5f);
            unit_AI.SetRateOfFire(1f);
            unit_AI.SetMinimumDamage(10f);
            unit_AI.SetRandomRifleSkill(10f, 40f);
            
            break;
        case Unit_USType.Gunner:
            //transform.Find("Body").GetComponent<SpriteRenderer>().sprite = GameAssets.i.s_USGunner;
            normalSprite = usGunnerSprite;
            SetSpriteNormal();
            healthSystem = new HealthSystem(100); // give 100 health
            ammoSystem = new AmmoSystem(100); // give 5 ammo
            expSystem = new EXPSystem(100); // set Max EXP to 100
            rifleSkillSystem = new RifleSkillSystem(100); // Set Max Rifle Skill to 100
            //attackDamage = 30;
            //attackRange = 20f;
            //fastAttackRange = 15f;
            //reloadTime = 10f;
            //rateOfFire = .1f; 
            unit_AI.SetAttackDamage(30f);
            unit_AI.SetAttackRange(20f);
            unit_AI.SetReloadTime(10f);
            unit_AI.SetRateOfFire(.1f);
            unit_AI.SetMinimumDamage(10f);
            unit_AI.SetRandomRifleSkill(10f, 40f);
            break;        

        //case Unit_USType.Red:         
            //material = GameAssets.i.m_Unit_USRed;   
        //    material = GameAssets.i.pfDEsoldier;  
        //    healthSystem.SetHealthMax(130, true);
            //characterBase.SetIdleWalkAnims(Unit_USAnimType.GetUnit_USAnimType("dShielder_Idle"), Unit_USAnimType.GetUnit_USAnimType("dShielder_Walk"));
        //    break;
       // case Unit_USType.Yellow:      
            //material = GameAssets.i.m_Unit_USYellow;  
        //    material = GameAssets.i.pfDEsoldier;
        //    healthSystem.SetHealthMax(50, true);
            //characterBase.SetIdleWalkAnims(Unit_USAnimType.GetUnit_USAnimType("dArrow_Idle"), Unit_USAnimType.GetUnit_USAnimType("dArrow_Walk"));
        //    break;
        }


        //transform.Find("Body").GetComponent<MeshRenderer>().material = material; 
    }

 //public void SetGetTarget(Func<IUnit_USTargetable> getUnitTarget) {
 //       this.getUnitTarget = getUnitTarget;
  //  }

    private void Update() {
        pathfindingTimer -= Time.deltaTime;
        shootTimer -= Time.deltaTime;

        switch (state) {
        case State.Normal:
            FindTarget();
            break;
        case State.Moving:
            break;
        case State.Attacking:
            Attack();
            break;
        case State.Busy:
            break;
        case State.Reloading:
            Reload();
            break;
        case State.Pinned:
            PinnedLogic();
            break;
        case State.Aiming:
            AimLogic();
            break;
        }
    }


    private void Attack(){
        IUnit enemy = GetClosestEnemy();

        if (enemy != null) { // Enemy in range   
            GetComponent<Unit_AI>().attack(); // begin attacking
        }
        else { // No enemies in range
            SetStateNormal(); 
        }

    }
    private void FindTarget() {
        IUnit enemy = GetClosestEnemy();

        if (enemy != null) { // ENEMY IN RANGE
            currentTargetEnemy = enemy;
            Vector3 enemyPosition = enemy.GetPosition();
            //AimPosition(enemyPosition);
            AimEnemy(currentTargetEnemy);
        }

        else { // ENEMY NOT IN RANGE
            GetComponent<IMovePosition>().resumeMovement(); // cancel a pause on movement if any
            //Debug.Log("Enemy not in range");
        }

    }

    public void AimPosition(Vector3 position){
        Vector3 currentTargetPositon = position;
        currentTargetAim = position;
        GetComponent<IMovePosition>().pauseMovement(); // stop moving so can attack
        aimTime = calculateAimTime();
        aimTimer = aimTime;
        aiming = true;
        //SetSpriteAiming;
        SetStateAiming();
    }

    public void AimEnemy(IUnit enemy){
        currentTargetEnemy = enemy;
        GetComponent<IMovePosition>().pauseMovement(); // stop moving so can attack
        aimTime = calculateAimTime();
        aimTimer = aimTime;
        aiming = true;
        //SetSpriteAiming;
        SetStateAiming();
    }
    public void Pin(){
        pinnedTimer = pinnedTime;
        pinned = true; 
        SetStatePinned();
    }
    private void PinnedLogic(){
        pinnedTimer -= Time.deltaTime;

        if (pinnedTimer <= 0f){
            pinned = false;         
            SetStateNormal();
        }   
    }

    private void AimLogic(){
        IUnit enemy = GetClosestEnemy();
        if (enemy == null){ // enemy has died since aiming
            SetStateNormal();
        }
        else {
            currentTargetAim = enemy.GetPosition();
            RotateTowards(currentTargetAim);
            aimTimer -= Time.deltaTime;
            if (aimTimer <= 0f){
                aiming = false;
                SetStateAttacking();
            }
        }
    }

    private float calculateAimTime(){
        // calculates an upper bound for max aim time based on experience level
        int unitEXP = expSystem.GetEXP();
        float unitEXPNormalized = unitEXP * .01f;
        if (unitEXPNormalized > 1f){
            unitEXPNormalized = 1f;
        }
        float percentMissingEXP = 1f - unitEXPNormalized;       
        float additionalAimTimeMax = aimTimeMax - aimTimeMin;   // Calculate maximum additional aim time
        float additionalAimTimeEXPOffset = additionalAimTimeMax * percentMissingEXP; // unit with 25 exp gets .75f aim Time increase
        float upperBoundAimTime = aimTimeMin + additionalAimTimeEXPOffset;   // calculate an upper bound base don experience
        float calculatedAimTime = UnityEngine.Random.Range(aimTimeMin, upperBoundAimTime);
        return calculatedAimTime;
    }

    public void useAmmo(int amount)
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

    private void Reload() // Called repeatedly when in StateReloading
    {
        int currentAmmo = ammoSystem.GetAmmo();
        if (IsOutOfAmmo()){
            unit_AI.Reload(); // begin reloading
        }
        else{ // finished reloading 
            SetStateNormal();
        }


    }





    private void HandleMovement() {
        if (pathVectorList != null) {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            
            if (Vector3.Distance(transform.position, targetPosition) > .1f) {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                //characterBase.PlayMoveAnim(moveDir);
                transform.position = transform.position + moveDir * SPEED * Time.deltaTime;

                // Rotation
                Vector3 rotationDir = moveDir;
                transform.up = rotationDir;

                
            } else {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count) {
                    StopMoving();
                    //characterBase.PlayIdleAnim();
                }
            }
        } else {
            //characterBase.PlayIdleAnim();
        }
    }


    public void Damage(int damageAmount, Vector3 damageDirection, IUnit attackingUnit) {
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

    public void addEXP(int amount){
        //Debug.Log("Receiving EXP: " + amount);
        expSystem.addEXP(amount);
    }

    public int getEXP(){
        return expSystem.GetEXP();
    }

    public float GetRange() {
        return attackRange;
    }

    public void UpgradeRange(int amount) {
        attackRange += amount;
    }

    public void UpgradeDamageAmount(int amount) {
        attackDamage += amount;
    }


    private void StopMoving() {
        pathVectorList = null;
    }

    public void SetTargetPosition(Vector3 targetPosition) {
        currentPathIndex = 0;
        //pathVectorList = GridPathfinding.instance.GetPathRouteWithShortcuts(GetPosition(), targetPosition).pathVectorList;
        pathVectorList = new List<Vector3> { targetPosition };
        if (pathVectorList != null && pathVectorList.Count > 1) {
            pathVectorList.RemoveAt(0);
        }
    }

    public void SetPathVectorList(List<Vector3> pathVectorList) {
        this.pathVectorList = pathVectorList;
    }

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

    public float getRange(){
        return attackRange;
    }
    public float getDamage(){
        return attackDamage;
    }
    public float getRateOfFire(){
        return rateOfFire;
    }
    public float getReloadTime(){
        return reloadTime;
    }
    
    private void SetStateNormal() {
        SetSpriteNormal();
        state = State.Normal;
    }

    private void SetStateAttacking() {
        state = State.Attacking;
    }

    private void SetStateReloading()
    {
        state = State.Reloading;
    }

    private void SetStatePinned()
    {
        SetSpritePinned();
        state = State.Pinned;
    }

    private void SetStateAiming()
    {
        //SetSpriteAiming();
        state = State.Aiming;
    }

    public IUnit GetClosestEnemy() {
        return Unit_DE.GetClosestUnit_DE(transform.position, unit_AI.GetAttackRange());
    }

    private void SetSpriteNormal(){
        transform.Find("Body").GetComponent<SpriteRenderer>().sprite = normalSprite;
    }
    private void SetSpritePinned(){
        //transform.Find("Body").GetComponent<SpriteRenderer>().sprite = usGunnerSprite;
        transform.Find("Body").GetComponent<SpriteRenderer>().sprite = usPinnedSprite;
    }

    public string GetTeam(){
        return "US";
    }

    public void RotateTowards(Vector3 targetPosition){
        Vector3 rotationDir = (targetPosition - transform.position).normalized;
        iMoveRotation.SetRotation(rotationDir);
    }
}