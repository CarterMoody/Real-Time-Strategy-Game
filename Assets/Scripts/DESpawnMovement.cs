using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DESpawnMovement : MonoBehaviour
{
    private bool firstSpawned; 
    private IMovePosition movePosition;
    [SerializeField] private Vector3 spawnWalkGoal; // After spawn walk here

    private void Awake()
    {
       firstSpawned = true;
       movePosition = GetComponent<IMovePosition>();
       spawnWalkGoal += new Vector3(transform.position.x, 0, 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        firstSpawned = true;
        movePosition.SetMovePosition(spawnWalkGoal);
    }

    // Update is called once per frame
    void Update()
    {

    }

}
