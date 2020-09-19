using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePositionDirect : MonoBehaviour, IMovePosition
{

    private static MovePositionDirect instance;
    private Vector3 movePosition;

    private Rigidbody2D rb2D;
    private Vector3 lastMoveDir;

    private bool isMoving;
    private bool movementPaused;
    private void Awake()
    {
        instance = this;
        movePosition = transform.position;
        rb2D = GetComponent<Rigidbody2D>();
        movementPaused = false;
    }
    
    public void SetMovePosition(Vector3 movePosition)
    {
        this.movePosition = movePosition;  
    }

    private void Update()
    {
        Vector3 moveDir;

        if (movementPaused){
            GetComponent<IMoveVelocity>().SetVelocity(Vector3.zero);
        }
        else{
            if (Vector3.Distance(transform.position, movePosition) > .1f) 
            {
                isMoving = true;
                moveDir = (movePosition - transform.position).normalized;
                //Debug.Log("movePosition: " + movePosition);
                //Debug.Log("transform.position: " + transform.position);
                //Debug.Log("moveDir: " + moveDir);

                Vector3 rotationDir = moveDir;
                GetComponent<IMoveRotation>().SetRotation(rotationDir);
                GetComponent<IMoveVelocity>().SetVelocity(moveDir); // Don't move while shooting
                lastMoveDir = moveDir;
            }
            else
            {
                isMoving = false;
                moveDir = Vector3.zero; // Stop moving when near
                GetComponent<IMoveVelocity>().SetVelocity(moveDir);
            }
        }    
    }

    public bool getStatus_moving()
    {
        return isMoving;
    }

    public void pauseMovement()
    {
        //Debug.Log("PausingMovement");
        movementPaused = true;
    }

    public void resumeMovement()
    {
        //Debug.Log("ResumingMovement");
        movementPaused = false;
    }
}
