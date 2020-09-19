using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class ProjectileBulletDefault : MonoBehaviour
{
    //[SerializeField] private GameObject pfProjectileArrow;
    public static void Create(Vector3 spawnPosition, IUnit attackingUnit, IUnit targetUnit, int damageAmount, float accuracy)
    {
        Transform arrowTransformGeneric = Instantiate(GameAssets.i.pfProjectileBulletDefault, spawnPosition, Quaternion.identity);

        ProjectileBulletDefault projectileBulletDefault = arrowTransformGeneric.GetComponent<ProjectileBulletDefault>();
        projectileBulletDefault.Setup(attackingUnit, targetUnit, damageAmount, accuracy);
    }



    private IUnit targetUnit;
    private IUnit attackingUnit;
    private int damageAmount;
    private float accuracy;
    private float destroySelfDistance;
    private Vector3 targetPosition;
    private Vector3 moveDir;
    private float bulletVelocity;
    private Vector3 actualTargetPosition;
    private float hitDetectionSize;
    private float pinDetectionSize;
    private void Setup(IUnit attackingUnit, IUnit targetUnit, int damageAmount, float accuracy) {
        this.attackingUnit = attackingUnit;
        this.targetUnit = targetUnit;
        this.damageAmount = damageAmount;
        this.accuracy = accuracy;
        this.targetPosition = targetUnit.GetPosition();
        this.destroySelfDistance = .1f;
        
        this.bulletVelocity = 50f;
        this.hitDetectionSize = .25f;
        this.pinDetectionSize = 1f;

        // Add innacurracy
        this.actualTargetPosition = AddInaccuracy(targetUnit, accuracy);
        this.moveDir = (actualTargetPosition - transform.position).normalized;

        // Calculate where to point the bullet (rotation of sprite)
        float angle = UtilsClass.GetAngleFromVectorFloat(moveDir);
        angle = angle -90f;
        transform.eulerAngles = new Vector3(0, 0, angle);

        //this.calculatedShot = calculateShot();
        //float accurateBoundLow = -10f;
        //float accurateBoundHigh = 10f;
        //float randomAngleTweak = randomAngleTweak.Range()
    }

    Vector3 AddInaccuracy(IUnit targetUnit, float accuracy){
        float lowerBoundMax = -1f;
        float upperBoundMax = 1f;

        float lowerBoundCalculated = lowerBoundMax * (1 - accuracy);
        float upperBoundCalculated = upperBoundMax * (1 - accuracy);

        float randomXOffset = Random.Range(lowerBoundCalculated, upperBoundCalculated);
        float randomYOffset = Random.Range(lowerBoundCalculated, upperBoundCalculated);

        float actualTargetPositionX = targetPosition.x + randomXOffset;
        float actualTargetPositionY = targetPosition.y + randomYOffset;

        Vector3 actualTargetPositionVector = new Vector3(actualTargetPositionX, actualTargetPositionY, targetPosition.z);
        return actualTargetPositionVector;
    }

    private void Update()
    {
        if (targetUnit == null || targetUnit.IsDead()) { // If targetUnit is dead
            // Unit already dead
            Destroy(gameObject);
            // should add logic here for bullet to pass thru to other enemies
            return;
        }

        transform.position += moveDir * bulletVelocity * Time.deltaTime;

        // Damage Logic
        IUnit closestUnit = GetClosestUnit(transform.position, hitDetectionSize);
        if (closestUnit != null){
            closestUnit.Damage(damageAmount, moveDir, attackingUnit);
            Destroy(gameObject);
        }

        // Pin Logic
        closestUnit = GetClosestUnit(transform.position, pinDetectionSize);
        if (closestUnit != null){
            if (SameTeam(attackingUnit, closestUnit)){
                return;
            }
            else{
                closestUnit.Pin();
            }
            
        }

        // Destroy out of bounds logic
        if (transform.position.x < -20f | transform.position.x > 20f){
            if (transform.position.y < -20f | transform.position.y > 20f){
                Debug.Log("bullet out of bounds");
                Destroy(gameObject);
            }
        }

        
        //if (Vector3.Distance(transform.position, targetPosition) < destroySelfDistance)
        //if (Vector3.Distance(transform.position, actualTargetPosition) < destroySelfDistance)
        //{
        //    targetUnit.Damage(damageAmount, moveDir, attackingUnit);
        //    Destroy(gameObject);
        //    
        //}

    }

    IUnit GetClosestUnit(Vector3 position, float maxRange){
        Unit_US closestUnit_US = Unit_US.GetClosestUnit_US(position, maxRange);
        Unit_DE closestUnit_DE = Unit_DE.GetClosestUnit_DE(position, maxRange);
        if (closestUnit_DE != null){
            return closestUnit_DE;
        }
        else if (closestUnit_US!= null){
            return closestUnit_US;
        }
        else{
            return null;
        }
    }

    bool SameTeam(IUnit attackingUnit, IUnit targetUnit){
        if (attackingUnit.GetTeam() == targetUnit.GetTeam()){
            return true;
        }
        else return false;
    }
}
