using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// マップに必要な要素をまとめるクラス
/// </summary>
public class MapData
{
    public GameObject gameRoot { set; get; }
    public TextAsset mapData { set; get; }
    public TileBase load { set; get; }
    public List<TileBase> wall { set; get; } = new List<TileBase>();
    public TileBase setTower { set; get; }
    public TileBase start { set; get; }
    public TileBase goal { set; get; }
    public Material overTileMaterial { set; get; }
    public TextAsset enemyData { set; get; }
}
/// <summary>
/// mapのType分け
/// </summary>
public enum DataType
{
    GameRoot,
    MapDataText,
    LoadTile,
    WallTile,
    SetTowerTile,
    StartTile,
    GoalTile,
    Material,
    EnemyDataText,
}