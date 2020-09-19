using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovePosition {

    void SetMovePosition(Vector3 movePosition);
    void pauseMovement();
    void resumeMovement();

}
