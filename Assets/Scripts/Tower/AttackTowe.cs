﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTowe : TowerMonoBehaviur
{
    [SerializeField] int _attackPowe = 10;
    [SerializeField] float _attackInterval = 2;
    [SerializeField] float _attckArea = 2;

    EnemyCon _attackEnemy;

    private void Awake()
    {
        StartCoroutine(Attack(EnemyManager.Instance.instanceEnemys, _attackInterval, _attackPowe));
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
                int maxCount = 0;
                List<int> enemyIndexs = new List<int>();

                yield return null;
                for (int i = 0; i < enemy.Count; i++)
                {
                    if ((enemy[i].transform.position - transform.position).magnitude <= _attckArea)
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
            if (enemy[count].HP <= 0)
            {
                enemy.RemoveAt(count);
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
