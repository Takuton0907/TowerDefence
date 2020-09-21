using Boo.Lang;
using System.Collections;
using UnityEngine;

public class TowerAttackAnimCon : TowerAnimBase
{
    public override void AnimUpdate(float speed)
    {
        if ((_animTargetPosi - transform.position).magnitude >= 0.3f)
        {
            transform.position += _moveVector * speed * _animSpeed;
        }
    }
}
