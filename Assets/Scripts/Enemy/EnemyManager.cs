using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Transform _enemyParentObj = null;

    [SerializeField] GameObject[] _enemyObj = null;

    [SerializeField] Vector3 localSize = Vector3.zero;

    public TextAsset _stageText;

    public float _speedRateUP = 2;
    public float _speedRateDown = 0.3f;

    public List<EnemyCon> instanceEnemys = new List<EnemyCon>();

    int _count;

    public string[,] _stageTexts;

    float _time;

    //敵のインスタンス
    private void EnemyInstance(int enemyNum, int spawnNumber, float rate)
    {
        GameObject obj = Instantiate(_enemyObj[enemyNum], LevelManager.Instance._mapDate.GetStart()[spawnNumber] + new Vector3(0.5f, 0.5f, 0) ,Quaternion.identity , _enemyParentObj);
        EnemyCon enemyCon = obj.GetComponent<EnemyCon>();
        enemyCon.EnemyAwake();
        enemyCon._speedRate = rate;
        instanceEnemys.Add(enemyCon);
        obj.transform.localScale = localSize;
    }

    public void EnemyManagerInit()
    {
        _stageTexts = LoadText.SetTexts(_stageText.text);

        _count = 1;
    }
    
    public void EnemySpeedChange(bool toDrag)
    {
        float speedRate = instanceEnemys[0]._speedRate;

        if (speedRate == _speedRateUP || speedRate == _speedRateDown)
        {
            foreach (var item in instanceEnemys)
            {
                item._speedRate = 1;
            }
        }
        else
        {
            if (toDrag)
            {
                foreach (var item in instanceEnemys)
                {
                    item._speedRate = _speedRateDown;
                }
            }
            else
            {
                foreach (var item in instanceEnemys)
                {
                    item._speedRate = _speedRateUP;
                }
            }
        }
    }
    public void EnemySpeedChange(float value)
    {
        foreach (var item in instanceEnemys)
        {
            item._speedRate = value;
        }
    }
    //エネミーを出す
    public void EnemySpawnUpdate(float rate)
    {
        if (_count >= _stageTexts.GetLength(0))
        {
            if (instanceEnemys.Count <= 0)
            {
                //ステージクリア
                LevelManager.Instance.StageClear();
            }
            return;
        }

        if (_time > float.Parse(_stageTexts[_count, 2]))
        {    
            if (_enemyObj.Length <= 0 || LevelManager.Instance._mapDate.GetStart().Count <= 0) return;

            EnemyInstance(int.Parse(_stageTexts[_count, 0]), int.Parse(_stageTexts[_count, 1]), rate);

            _count++;

            _time = 0;
        }
        _time += Time.deltaTime * rate;
    }
    //今現在インスタンスされている敵の取得
    public List<EnemyCon> GetEnemys()
    {
        return instanceEnemys;
    }
    //敵をリストから削除
    public void DestroyEnemy(EnemyCon desEnemy)
    {
        instanceEnemys.Remove(desEnemy);
    }
}