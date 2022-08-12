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
 * Unit_DE
 * 
 * Contains base state machine for Unit_DE behavior
 *
 * */

// Enemy is replaced by Unit_DE for US soldiers

public class Unit_DE : MonoBehaviour, IUnit {
    

    public static List<Unit_DE> unit_DEList = new List<Unit_DE>();

    public static Unit_DE GetClosestUnit_DE(Vector3 position, float maxRange) {
        Unit_DE closest = null;
        foreach (Unit_DE unit_DE in unit_DEList) {
            if (unit_DE.IsDead()) continue;
            if (Vector3.Distance(position, unit_DE.GetPosition()) <= maxRange) {
                if (closest == null) {
                    closest = unit_DE;
                } else {
                    if (Vector3.Distance(position, unit_DE.GetPosition()) < Vector3.Distance(position, closest.GetPosition())) {
                        closest = unit_DE;
                    }
                }
            }
        }
        return closest;
    }


    public static Unit_DE Create(Vector3 position) {
        Transform unitTransform = Instantiate(GameAssets.i.pfUnit_DE, position, Quaternion.identity);

        Unit_DE unitHandler = unitTransform.GetComponent<Unit_DE>();
        
        return unitHandler;
    }
    
    //public static Unit_DE Create(Vector3 position, Unit_DEType unitType) {
    public static Unit_DE Create(Vector3 position, Unit_DEType unitType) {
        Transform unitTransform = Instantiate(GameAssets.i.pfUnit_DE, position, Quaternion.identity);

        Unit_DE unitHandler = unitTransform.GetComponent<Unit_DE>();
        unitHandler.SetUnit_DEType(unitType);

        return unitHandler;
    }

    public enum Unit_DEType {
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
    private Character_Base characterBase;
    private State state;
    private Vector3 lastMoveDir;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    private float pathfindingTimer;
    //private Func<IUnitTargetable> getUnitTarget;
    private IMovePosition movePosition;



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

    [SerializeField] Sprite deSoldierSprite;
    [SerializeField] Sprite dePinnedSprite;
    private Sprite normalSprite;
    private AudioSource source;
    private AudioClip randomClip;
    public AudioClip[] FleshHits;


    private bool enemyInRange; // used to determine if should rotate based on movement or enemy position.

    //private Unit_DEAnimType idleUnit_DEAnim;
    //private Unit_DEAnimType walkUnit_DEAnim;
    //private Unit_DEAnimType hitUnit_DEAnim;
    //private Unit_DEAnimType attackUnit_DEAnim;

    private enum State {
        Normal,
        Attacking,
        Moving,
        Busy,
        Reloading,
        Pinned,
        Aiming
    }

    private void Awake() {
        unit_DEList.Add(this);
        characterBase = gameObject.GetComponent<Character_Base>();
        unit_AI = GetComponent<Unit_AI>();
        healthSystem = new HealthSystem(100); // give 100 health
        ammoSystem = new AmmoSystem(5); // give 5 ammo
        expSystem = new EXPSystem(0); // give 0 EXP
        

        // Stuff below from Unit_AI
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
        unit_AI.SetRandomRifleSkill(10f, 20f); // purposefully low for now
        iMoveRotation = GetComponent<IMoveRotation>();
        //enemyInRange = false;
        pinnedTime = 2f;
        aimTime = 2f;
        aimTimeMax = 5f;
        aimTimeMin = 1f;
        

        normalSprite = deSoldierSprite;
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



    // colors the Unit_DE?
    private void SetUnit_DEType(Unit_DEType unitType) {
        //Material material;

        switch (unitType) {
        default:
        case Unit_DEType.Orange:      
            //material = GameAssets.i.m_Unit_DEOrange;
            //material = GameAssets.i.pfDEsoldier;
            healthSystem.SetHealthMax(80, true);
            ammoSystem.SetAmmoMax(5, true);
            break;
        //case Unit_DEType.Red:         
            //material = GameAssets.i.m_Unit_DERed;   
        //    material = GameAssets.i.pfDEsoldier;  
        //    healthSystem.SetHealthMax(130, true);
            //characterBase.SetIdleWalkAnims(Unit_DEAnimType.GetUnit_DEAnimType("dShielder_Idle"), Unit_DEAnimType.GetUnit_DEAnimType("dShielder_Walk"));
        //    break;
       // case Unit_DEType.Yellow:      
            //material = GameAssets.i.m_Unit_DEYellow;  
        //    material = GameAssets.i.pfDEsoldier;
        //    healthSystem.SetHealthMax(50, true);
            //characterBase.SetIdleWalkAnims(Unit_DEAnimType.GetUnit_DEAnimType("dArrow_Idle"), Unit_DEAnimType.GetUnit_DEAnimType("dArrow_Walk"));
        //    break;
        }


        //transform.Find("Body").GetComponent<MeshRenderer>().material = material; 
    }

 //public void SetGetTarget(Func<IUnit_DETargetable> getUnitTarget) {
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
            GetComponent<IMovePosition>().pauseMovement(); // stop moving so can attack
            
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
        SetSpritePinned();
        SetStatePinned();
    }
    private void PinnedLogic(){
        pinnedTimer -= Time.deltaTime;

        if (pinnedTimer <= 0f){
            pinned = false;
            SetSpriteNormal();
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
/*         if (pathVectorList != null) {
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
        } */
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
            //TooltipTop.ShowTooltipTop_Static("Unit_DE Killed"); // possibly name of unit died?
            KillCount.addKills_Static(1);
            // Add to death count statistic
            //FlyingBody.Create(GameAssets.i.pfUnit_DEFlyingBody, GetPosition(), bloodDir);

            // Give EXP to attackingUnit
            //Debug.Log("Adding 1 EXP to soldier");
            attackingUnit.addEXP(1);
            Destroy(gameObject);
            
        } else {
            Pin();
            // Knockback
            //transform.position += bloodDir * .0025f;
        }
    }


    public void addEXP(int amount){
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
    
    private void SetStateNormal() {
        state = State.Normal;
    }

    private void SetStateAiming(){
        state = State.Aiming;
    }

    private void SetStateAttacking() {
        state = State.Attacking;
    }

    private void SetStateReloading()
    {
        state = State.Reloading;
    }

    public void SetStatePinned()
    {
        
        state = State.Pinned;
    }

    public IUnit GetClosestEnemy() {
        return Unit_US.GetClosestUnit_US(transform.position, unit_AI.GetAttackRange());
    }

    private void SetSpritePinned(){
        GetComponent<SpriteRenderer>().sprite = dePinnedSprite;
    }
    private void SetSpriteNormal(){
        GetComponent<SpriteRenderer>().sprite = normalSprite;
    }

    public string GetTeam(){
        return "DE";
    }
    public void RotateTowards(Vector3 targetPosition){
        Vector3 rotationDir = (targetPosition - transform.position).normalized;
        iMoveRotation.SetRotation(rotationDir);
    }
}