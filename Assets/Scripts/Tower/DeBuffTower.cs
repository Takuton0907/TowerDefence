using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBuffTower : TowerMonoBehaviur
{
    [SerializeField] GameObject _effectObj;

    int _count = 0;

    float _time = 0;

    public override void Init()
    {
        base.Init();

        Instantiate(_effectObj, transform.position, Quaternion.identity);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(this.transform.position, this._area);
    }
#endif

    public override void Action(float speed)
    {
        foreach (var item in LevelManager.Instance._enemyManager.instanceEnemys)
        {
            if ((item.transform.position - transform.position).magnitude <= _area)
            {
                item._enemySpeed = EnemyCon.EnemySpeedState.DOWN;
            }
        }
    }
}
