using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public List<TowerMonoBehaviur> instanceTowers = new List<TowerMonoBehaviur>();

    public float _speedRate = 1;

    public DragObj[] _instanseDragObj = new DragObj[4];

    //Towerをセット
    public void SetTower(TowerMonoBehaviur tower)
    {
        instanceTowers.Add(tower);
    }

    //エネミーを出す
    public void TowerUpdate(float rate)
    {
        foreach (var item in instanceTowers)
        {
            item.Action(LevelManager.Instance._enemyManager.instanceEnemys, rate);
            item.Action(rate);
        }
    }

    //towerをリストから削除
    public void DestroyTower(TowerMonoBehaviur tower)
    {
        instanceTowers.Remove(tower);
    }
}
