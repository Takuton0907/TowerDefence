using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mapcreate : MonoBehaviour
{
    [SerializeField] Tilemap _tilemap;
    [SerializeField] TileBase _tileBase;

    [SerializeField] MapDateObject mapDate;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(mapDate.mapDates.Count);
        //TileMapCon.SetMap(_tilemap,new Vector2(10, 15), _tileBase, _tileBase);
        TileMapCon.SetMap(ref _tilemap, mapDate);
    }
}
