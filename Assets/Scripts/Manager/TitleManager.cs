using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : SingletonMonoBehaviour<TitleManager>
{
    [SerializeField] MapDate _titleMapdate;

    [SerializeField] TextAsset _spawnText;

    [SerializeField] GameObject[] _enemyObj = null;

    [SerializeField] Vector3 localSize = Vector3.zero;

    string[,] _texts;

    int _count = 1;

    float _time;

    // Start is called before the first frame update
    void Start()
    {
        _texts = LoadText.SetTexts(_spawnText.text);
    }

    // Update is called once per frame
    void Update()
    {
        EnemySpawnUpdate();
    }

    private void EnemyInstance(int enemyNum, int spawnNumber)
    {
        GameObject obj = Instantiate(_enemyObj[enemyNum], _titleMapdate.GetStart()[spawnNumber] + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
        obj.transform.localScale = localSize;
    }

    private void EnemySpawnUpdate()
    {
        if (_count >= _texts.GetLength(0))
        {
            _count = 1;
        }

        if (_time > float.Parse(_texts[_count, 2]))
        {
            if (_enemyObj.Length <= 0 || _titleMapdate.GetStart().Count <= 0) return;

            EnemyInstance(int.Parse(_texts[_count, 0]), int.Parse(_texts[_count, 1]));

            _count++;

            _time = 0;
        }
        _time += Time.deltaTime;
    }

    //すべてのタイルのSATASをOpenにする
    private void TileOpen()
    {
        foreach (var item in _titleMapdate.mapDates)
        {
            item.states = TILE_STATAS.Opne;
        }
    }
    //敵が進む道を検索
    public List<MAP_DATE> Sarch(Vector3 posi)
    {
        TileOpen();

        posi = Vector3Int.FloorToInt(posi);
        posi = new Vector3(posi.x, posi.y, 0);

        int dateIndex = 0;
        for (int i = 0; i < _titleMapdate.mapDates.Count; i++)
        {
            if (_titleMapdate.mapDates[i].posi == posi)
            {
                dateIndex = i;
                break;
            }
        }

        var openList = new List<int>();

        return Aster(dateIndex, openList);
    }
    //A*の実装
    private List<MAP_DATE> Aster(int index, List<int> list)
    {
        List<int> goalIndexs = _titleMapdate.GetGoalIndex();

        int startIndex = index;

        for (int i = 0; i < 1000; i++)
        {
            if (_titleMapdate.mapDates[index].tileBaseNum == (int)TILE.GOAL)
            {
                break;
            }

            //次に動けるTileの取得
            var mapIndexs = _titleMapdate.GetNextTilesIndex(index);

            //通れる道以外の削除
            foreach (var item in mapIndexs)
            {
                if (_titleMapdate.mapDates[item].states == TILE_STATAS.Opne)
                {
                    if (_titleMapdate.mapDates[item].tileBaseNum == (int)TILE.LOAD || _titleMapdate.mapDates[item].tileBaseNum == (int)TILE.GOAL || _titleMapdate.mapDates[item].tileBaseNum == (int)TILE.START)
                    {
                        _titleMapdate.mapDates[item].parentDate = _titleMapdate.mapDates[index];
                        list.Add(item);
                    }
                }
            }
            //距離の計算
            foreach (var item in mapIndexs)
            {
                int minGoalValue = int.MaxValue;
                foreach (var ind in goalIndexs)
                    if (minGoalValue > _titleMapdate.GetCost(index, ind))
                        minGoalValue = _titleMapdate.GetCost(index, ind);

                _titleMapdate.mapDates[item].C = _titleMapdate.GetCost(index, minGoalValue);
                _titleMapdate.mapDates[item].H = _titleMapdate.GetCost(startIndex, index);
                _titleMapdate.mapDates[item].S = _titleMapdate.mapDates[item].C + _titleMapdate.mapDates[item].H;
            }
            //スタートした場所は検索済みなのでCloaseに
            _titleMapdate.mapDates[index].states = TILE_STATAS.Close;

            int minValue = int.MaxValue;

            foreach (var item in list)
            {
                if (_titleMapdate.mapDates[item].states == TILE_STATAS.Close) continue;
                if (_titleMapdate.mapDates[item].S < minValue)
                {
                    minValue = _titleMapdate.mapDates[item].S;
                    index = item;
                }
            }
        }

        var vs = new List<MAP_DATE>();
        MAP_DATE date = _titleMapdate.mapDates[index];
        while (date.tileBaseNum != (int)TILE.START)
        {
            vs.Add(date);
            date = date.parentDate;
        }

        return vs;
    }
}
