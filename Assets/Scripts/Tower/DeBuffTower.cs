using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary> debuffを与えるタワー </summary>
public class DeBuffTower : TowerBase
{
    [SerializeField] GameObject _effectObj;

    public override void Init()
    {
        base.Init();

        GameObject effect = Instantiate(_effectObj, transform.position, Quaternion.identity, gameObject.transform);
        effect.transform.GetChild(0).localScale = new Vector3(_area / 10, _area / 10, _area / 10);

        SoundManager.Instance.SetDefenseAudio(_actionAudio.clip);
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
