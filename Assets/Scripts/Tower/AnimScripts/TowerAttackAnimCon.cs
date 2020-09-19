using UnityEngine;

public class TowerAttackAnimCon : TowerAnimBase
{
    public override void AnimUpdate(float speed)
    {
        if ((_animTargetPosi - transform.position).magnitude >= 0.5f)
        {
            transform.position += _moveVector * speed * _animSpeed;
        }
        else
        {
            foreach (var item in particleSystem)
            {
                if (item.IsAlive()) return;
            }
        }
    }
}
