﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> タイトルシーンの管理 </summary>
public class TitleManager : SingletonMonoBehaviour<TitleManager>
{
    [SerializeField] MapDataObject _titleMapdate;

    [SerializeField] TextAsset _spawnText;

    [SerializeField] GameObject[] _enemyObj = null;

    [SerializeField] Vector3 localSize = Vector3.zero;

    [SerializeField] Transform _parentObj;

    string[,] _texts;

    int _count = 1;

    float _time;

    void Start()
    {
        _texts = LoadText.SetTexts(_spawnText.text);
    }

    void Update()
    {
        EnemySpawnUpdate();
    }

    private void EnemyInstance(int enemyNum, int spawnNumber)
    {
        GameObject obj = Instantiate(_enemyObj[enemyNum], _titleMapdate.GetStart()[spawnNumber] + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, _parentObj);
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
        foreach (var item in _titleMapdate.mapDatas)
        {
            item.states = TILE_STATAS.Opne;
        }
    }
    //敵が進む道を検索
    public List<GRID_DATA> Sarch(Vector3 posi)
    {
        TileOpen();

        posi = Vector3Int.FloorToInt(posi);
        posi = new Vector3(posi.x, posi.y, 0);

        int dateIndex = 0;
        for (int i = 0; i < _titleMapdate.mapDatas.Count; i++)
        {
            if (_titleMapdate.mapDatas[i].posi == posi)
            {
                dateIndex = i;
                break;
            }
        }

        var openList = new List<int>();

        return Aster(dateIndex, openList);
    }
    //A*の実装
    private List<GRID_DATA> Aster(int index, List<int> list)
    {
        List<int> goalIndexs = _titleMapdate.GetGoalIndex();

        int startIndex = index;

        for (int i = 0; i < 1000; i++)
        {
            if (_titleMapdate.mapDatas[index].tileBaseNum == (int)TILE.GOAL)
            {
                break;
            }

            //次に動けるTileの取得
            var mapIndexs = _titleMapdate.GetNextTilesIndex(index);

            //通れる道以外の削除
            foreach (var item in mapIndexs)
            {
                if (_titleMapdate.mapDatas[item].states == TILE_STATAS.Opne)
                {
                    if (_titleMapdate.mapDatas[item].tileBaseNum == (int)TILE.LOAD || _titleMapdate.mapDatas[item].tileBaseNum == (int)TILE.GOAL || _titleMapdate.mapDatas[item].tileBaseNum == (int)TILE.START)
                    {
                        _titleMapdate.mapDatas[item].parentData = _titleMapdate.mapDatas[index];
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

                _titleMapdate.mapDatas[item].C = _titleMapdate.GetCost(index, minGoalValue);
                _titleMapdate.mapDatas[item].H = _titleMapdate.GetCost(startIndex, index);
                _titleMapdate.mapDatas[item].S = _titleMapdate.mapDatas[item].C + _titleMapdate.mapDatas[item].H;
            }
            //スタートした場所は検索済みなのでCloaseに
            _titleMapdate.mapDatas[index].states = TILE_STATAS.Close;

            int minValue = int.MaxValue;

            foreach (var item in list)
            {
                if (_titleMapdate.mapDatas[item].states == TILE_STATAS.Close) continue;
                if (_titleMapdate.mapDatas[item].S < minValue)
                {
                    minValue = _titleMapdate.mapDatas[item].S;
                    index = item;
                }
            }
        }

        var vs = new List<GRID_DATA>();
        GRID_DATA date = _titleMapdate.mapDatas[index];
        while (date.tileBaseNum != (int)TILE.START)
        {
            vs.Add(date);
            date = date.parentData;
        }

        return vs;
    }
}
