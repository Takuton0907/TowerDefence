using System.Collections.Generic;
using UnityEngine;

/// <summary> タワーの管理をする </summary>
public class TowerManager : MonoBehaviour
{
    public List<TowerBase> instanceTowers = new List<TowerBase>();

    public float _speedRate = 1;

    public DragObj[] _instanseDragObj = new DragObj[4];

    //Towerをセット
    public void SetTower(TowerBase tower)
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
    public void DestroyTower(TowerBase tower)
    {
        instanceTowers.Remove(tower);
    }
}
