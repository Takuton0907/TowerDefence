using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaffTower : TowerMonoBehaviur
{
    [SerializeField] GameObject _effectObj;

    int _count = 0;

    public override void Init()
    {
        base.Init();

        GameObject effect = Instantiate(_effectObj, transform.position, Quaternion.identity, gameObject.transform);
        effect.transform.localScale = new Vector3(_area / 10 + 1, _area / 10 + 1, effect.transform.localScale.z);
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
        if (LevelManager.Instance._towerManager.instanceTowers.Count != _count)
        {
            foreach (var item in LevelManager.Instance._towerManager.instanceTowers)
            {
                if ((item.transform.position - transform.position).magnitude >= _area) continue;

                AttackTowe attackTowe = item.GetComponent<AttackTowe>();
                if (attackTowe != null)
                {
                    attackTowe._powerState = AttackTowe.PowerState.UP;
                }
            }

            _count = LevelManager.Instance._towerManager.instanceTowers.Count;
        }
    }
}
