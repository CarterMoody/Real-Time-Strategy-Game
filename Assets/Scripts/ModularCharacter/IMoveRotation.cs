using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveRotation
{
    void SetRotation(Vector3 moveDir);
    void Disable();
    void Enable();

}