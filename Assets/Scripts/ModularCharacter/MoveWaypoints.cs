﻿/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MoveWaypoints : MonoBehaviour {
    
    [SerializeField] private Vector3[] waypointList;
    private int waypointIndex;

    //private static MoveWaypoints instance;
    private void Awake()
    {
        //instance = this;
    }
    private void Update() {
        SetMovePosition(GetWaypointPosition());

        float arrivedAtPositionDistance = 1f;
        if (Vector3.Distance(transform.position, GetWaypointPosition()) < arrivedAtPositionDistance) {
            // Reached position
            waypointIndex = (waypointIndex + 1) % waypointList.Length;
        }
    }

    private Vector3 GetWaypointPosition() {
        return waypointList[waypointIndex];
    }

    private void SetMovePosition(Vector3 movePosition) {
        GetComponent<IMovePosition>().SetMovePosition(movePosition);
    }

    public void setWayPoint(int waypointIndex, Vector3 newWaypoint)
    {
        waypointList[waypointIndex] = newWaypoint;
    }
    //public static void setWayPoint_Static(int waypointIndex, Vector3 newWaypoint)
    //    instance.setWayPoint(waypointIndex, newWaypoint);
    //}

}
