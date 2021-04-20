using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapData : MonoBehaviour
{
    public GameObject _gameRoot { set; get; }
    public TextAsset _mapData { set; get; }
    public TileBase _load { set; get; }
    public TileBase[] _wall { set; get; }
    public TileBase _setTower { set; get; }
    public TileBase _start { set; get; }
    public TileBase _goal { set; get; }
    public Material _overTileMaterial { set; get; }
    public TextAsset _enemyData { set; get; }
}