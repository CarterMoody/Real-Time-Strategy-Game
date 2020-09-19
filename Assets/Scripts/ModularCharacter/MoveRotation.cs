using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRotation : MonoBehaviour, IMoveRotation
{

    [SerializeField] private float rotationSpeed;
    
    private float moveAngle;
    private Rigidbody2D rigidbody2D;

    private Vector3 velocityVector;

    private Vector3 moveDir;
    private Vector3 lastMoveDir;


    public void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    public void SetRotation(Vector3 moveDir)
    {
        //Debug.Log("setting moveDir: " + moveDir);
        this.moveDir = moveDir;
    }

    private void Update()
    {
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 0) * moveDir;

        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 2f);
    }
    public void Disable() {
        this.enabled = false;
        //rigidbody2D.velocity = Vector3.zero;
    }

    public void Enable() {
        this.enabled = true;
    }
}
