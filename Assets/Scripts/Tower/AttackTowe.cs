using System.Collections.Generic;
using UnityEngine;

public class AttackTowe : TowerMonoBehaviur
{
    public enum PowerState
    {
        NOMAL,
        UP,
        DOWN,
    }

    public PowerState _powerState = PowerState.NOMAL;

    [SerializeField] int _attackPowe = 10;
    [SerializeField] int _attackBuff = 2;
    [SerializeField] int _attackDeBuff= -2;
    [SerializeField] float _attackInterval = 2;

    EnemyCon _attackEnemy;

    [SerializeField] GameObject _attackaAnimObj;

    [SerializeField] GameObject _buffAnimObj;

    float _time = 0;

    int count = 0;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(this.transform.position, this._area);
    }
#endif

    //攻撃
    public override void Action(List<EnemyCon> enemy, float speed)
    {
        _time += Time.deltaTime * speed;

        if (_anims.Count >= 0)
        {
            foreach (var item in _anims)
            {
                item.AnimUpdate(speed);
                item.Deth();
            }
        }

        if (_time < _attackInterval) return;

        _time = 0;

        if (enemy.Count <= 0) return;

        if (_attackEnemy == null || (_attackEnemy.transform.position - transform.position).magnitude >= _area)
        {
            count = 0;
            int maxCount = 0;
            List<int> enemyIndexs = new List<int>();

            for (int i = 0; i < enemy.Count; i++)
            {
                if ((enemy[i].transform.position - transform.position).magnitude <= _area)
                {
                    enemyIndexs.Add(i);
                }
            }

            if (enemyIndexs.Count <= 0) return;

            foreach (var item in enemyIndexs)
            {
                if (enemy[item].count >= maxCount)
                {
                    maxCount = enemy[item].count;
                    count = item;
                }
            }

            _attackEnemy = enemy[count];
        }

        GameObject animObj = Instantiate(_attackaAnimObj, transform.position, Quaternion.identity, gameObject.transform);
        TowerAnimBase towerAnimBase = animObj.GetComponent<TowerAnimBase>();
        towerAnimBase.SetAnimDirection(enemy[count].transform.position, this);
        _anims.Add(towerAnimBase);

        switch (_powerState)
        {
            case PowerState.NOMAL:
                if (_buffAnimObj.activeSelf == true)
                {
                    _buffAnimObj.SetActive(false);
                }
                enemy[count].Damage(_attackPowe);
                break;
            case PowerState.UP:
                if (_buffAnimObj.activeSelf == false)
                {
                    _buffAnimObj.SetActive(true);
                }
                enemy[count].Damage(_attackPowe + _attackBuff);
                break;
            case PowerState.DOWN:
                enemy[count].Damage(_attackPowe + _attackDeBuff);
                break;
        }

        Vector3 distance = (enemy[count].gameObject.transform.position - transform.position).normalized;

        _animator.SetFloat("Hori", distance.x);
        _animator.SetFloat("Var", distance.y);

        if (enemy[count].HP <= 0)
        {
            enemy.RemoveAt(count);
        }
    }
}
