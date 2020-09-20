using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class AttackTowe : TowerMonoBehaviur
{
    [SerializeField] int _attackPowe = 10;
    [SerializeField] float _attackInterval = 2;

    EnemyCon _attackEnemy;
    Animator _animator;

    [Header("AnimationUse")]
    bool _animatoinUse = true;

    [SerializeField] GameObject _attackaAnimObj;

    List<TowerAnimBase> _anims = new List<TowerAnimBase>();

    float _time = 0;

    int count = 0;

    private void Awake()
    {
        base.Init();

        _animator = GetComponent<Animator>();
    }
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
            }
        }

        if (_time < _attackInterval) return;

        _time = 0;

        if (enemy.Count <= 0) return;

        if (_attackEnemy == null)
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

        GameObject animObj = Instantiate(_attackaAnimObj, transform.position, Quaternion.identity);
        TowerAnimBase towerAnimBase = animObj.GetComponent<TowerAnimBase>();
        towerAnimBase.SetAnimDirection(enemy[count].transform.position);
        _anims.Add(towerAnimBase);

        enemy[count].Damage(_attackPowe);

        Vector3 distance = (enemy[count].gameObject.transform.position - transform.position).normalized;

        _animator.SetFloat("Hori", distance.x);
        _animator.SetFloat("Var", distance.y);

        if (enemy[count].HP <= 0)
        {
            enemy.RemoveAt(count);
        }
    }
}
