using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTransformVelocity : MonoBehaviour, IMoveVelocity
{

    [SerializeField] private float moveSpeed;
    
    private Vector3 velocityVector;


    // for animation
    // private Character_Base characterBase;

    public void Awake()
    {

    }
    public void SetVelocity(Vector3 velocityVector)
    {
        this.velocityVector = velocityVector;
        // for animation
        // characterBase = GetComponent<Character_Base>();
    }

    private void Update()
    {
        transform.position += velocityVector * moveSpeed * Time.deltaTime;
    }
    private void FixedUpdate()
    {


        // for animation
        // characterBase.PlayMoveAnim(velocityVector);
    }


    public void Disable() {
        this.enabled = false;
    }

    public void Enable() {
        this.enabled = true;
    }

}