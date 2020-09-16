using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTowe : TowerMonoBehaviur
{
    [SerializeField] int _attackPowe = 10;
    [SerializeField] float _attackInterval = 2;

    EnemyCon _attackEnemy;
    Animator _animator;

    [Header("AnimationUse")]
    bool _animatoinUse = true;
    Animation _animation;

    LineRenderer lineRenderer;

    private void Awake()
    {
        base.Init();

        _animator = GetComponent<Animator>();

        if (_animatoinUse) _animation = GetComponent<Animation>();
        else lineRenderer = GetComponent<LineRenderer>();

        StartCoroutine(Attack(LevelManager.Instance._enemyManager.instanceEnemys, _attackInterval, _attackPowe));
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(this.transform.position, this._area);
    }
#endif
    //攻撃
    IEnumerator Attack(List<EnemyCon> enemy, float interval, int damage)
    {
        int count = 0;
        while (true)
        {
            yield return null;
            if (enemy.Count <= 0) continue;

            if (_attackEnemy == null)
            {
                count = 0;
                int maxCount = 0;
                List<int> enemyIndexs = new List<int>();

                yield return null;
                for (int i = 0; i < enemy.Count; i++)
                {
                    if ((enemy[i].transform.position - transform.position).magnitude <= _area)
                    {
                        enemyIndexs.Add(i);
                    }
                }

                if (enemyIndexs.Count <= 0) continue;

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


            enemy[count].Damage(damage);

            if (_animation != null) _animation.Play();
            //else lineRenderer.


            Vector3 distance = (enemy[count].gameObject.transform.position - transform.position).normalized;

            _animator.SetFloat("Hori", distance.x);
            _animator.SetFloat("Var", distance.y);

            if (enemy[count].HP <= 0)
            {
                enemy.RemoveAt(count);
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
