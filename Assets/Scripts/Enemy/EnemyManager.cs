using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Transform _enemyParentObj = null;
    [SerializeField] GameObject[] _enemyObj = null;

    [SerializeField] Vector3 localSize = Vector3.zero;
    //Tilemap startMap;

    public List<EnemyCon> instanceEnemys = new List<EnemyCon>();

    private void Start()
    {
        StartCoroutine(EnemySpawn(1));
    }

    private void EnemyInstance(int enemyNum, int spawnNumber)
    {
        GameObject obj = Instantiate(_enemyObj[enemyNum], LevelManager.Instance._mapDate.GetStart() + new Vector3(0.5f, 0.5f, 0) ,Quaternion.identity , _enemyParentObj);
        instanceEnemys.Add(obj.GetComponent<EnemyCon>());
        obj.transform.localScale = localSize;
    }

    IEnumerator EnemySpawn(float interval)
    {
        while (true)
        {
            yield return null;
            if (_enemyObj.Length <= 0 || LevelManager.Instance._mapDate.GetStart() == new Vector3(1000, 1000, 1000)) continue;
            EnemyInstance(0, 0);
            yield return new WaitForSeconds(interval);
        }
    }

    public List<EnemyCon> GetEnemys()
    {
        return instanceEnemys;
    }
}