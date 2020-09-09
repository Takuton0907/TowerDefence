using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    [SerializeField] Transform _enemyParentObj = null;

    [SerializeField] GameObject[] _enemyObj = null;

    [SerializeField] Vector3 localSize = Vector3.zero;

    [SerializeField] TextAsset _stageText;

    public List<EnemyCon> instanceEnemys = new List<EnemyCon>();

    //敵のインスタンス
    private void EnemyInstance(int enemyNum, int spawnNumber)
    {
        GameObject obj = Instantiate(_enemyObj[enemyNum], LevelManager.Instance._mapDate.GetStart()[spawnNumber] + new Vector3(0.5f, 0.5f, 0) ,Quaternion.identity , _enemyParentObj);
        EnemyCon enemyCon = obj.GetComponent<EnemyCon>();
        enemyCon.EnemyAwake();
        instanceEnemys.Add(enemyCon);
        obj.transform.localScale = localSize;
    }
    //エネミーを出す
    public IEnumerator EnemySpawn()
    {
        string[,] stageTexts = LoadText.SetTexts(_stageText.text);

        int count = 0;
        while (true)
        {
            count++;
            if (count >= stageTexts.GetLength(0))
            {
                break;
            }
            yield return null;
            if (_enemyObj.Length <= 0 || LevelManager.Instance._mapDate.GetStart().Count <= 0) continue;
            EnemyInstance(int.Parse(stageTexts[count, 0]), int.Parse(stageTexts[count, 1]));
            yield return new WaitForSeconds(int.Parse(stageTexts[count, 2]));
            //Debug.Log(count);
        }

        yield return new WaitWhile(() => instanceEnemys.Count > 0);

        //ステージクリア
        LevelManager.Instance.StageClear();
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