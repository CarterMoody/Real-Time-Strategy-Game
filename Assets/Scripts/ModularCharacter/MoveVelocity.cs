using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : MonoBehaviour, IMoveVelocity
{

    [SerializeField] private float moveSpeed;
    
    private Vector3 velocityVector;
    private Rigidbody2D rigidbody2D;

    // for animation
    // private Character_Base characterBase;

    public void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    public void SetVelocity(Vector3 velocityVector)
    {
        this.velocityVector = velocityVector;
        // for animation
        // characterBase = GetComponent<Character_Base>();
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = velocityVector * moveSpeed;

        // for animation
        // characterBase.PlayMoveAnim(velocityVector);
    }

    public void Disable() {
        this.enabled = false;
        rigidbody2D.velocity = Vector3.zero;
    }

    public void Enable() {
        this.enabled = true;
    }
}
