using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZangekiCon : TowerAnimBase
{
    public override void AnimUpdate(float speed)
    {
        transform.position = new Vector3(transform.position.x + _moveVector.x, transform.position.y + _moveVector.y, transform.position.z + _moveVector.z);
    }
}
