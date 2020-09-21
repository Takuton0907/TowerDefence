using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZangekiCon : TowerAnimBase
{
    [SerializeField] float _animationTime = 1f;
    bool _judge = true;

    public override void AnimUpdate(float speed)
    {
        if (_judge)
        {
            _moveVector *= 1.5f;
            transform.position = new Vector3(_animTargetPosi.x, _animTargetPosi.y, _animTargetPosi.z) * speed;
            _judge = false;
        }
        if (_animationTime <= 0)
        {
            _deth = true;
            return;
        }
        _animationTime -= Time.deltaTime * speed;
    }
}
