using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTowe : TowerMonoBehaviur
{
    EnemyManager enemyManager;
    [SerializeField] int _attackPowe = 10;
    [SerializeField] float _attackInterval = 2;

    EnemyCon _attackEnemy;

    private void Awake()
    {
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        if (enemyManager == null) return;
        StartCoroutine(Attack(enemyManager.instanceEnemys, _attackInterval, _attackPowe));
    }

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
                int minCount = int.MaxValue;
                for (int i = 0; i < enemy.Count; i++)
                {
                    if (enemy[i]._root.Count <= minCount)
                    {
                        minCount = enemy[i]._root.Count;
                        count = i;
                    }
                }

                _attackEnemy = enemy[count];
            }


            enemy[count].Damage(damage);
            if (enemy[count].HP <= 0)
            {
                enemy.RemoveAt(count);
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
