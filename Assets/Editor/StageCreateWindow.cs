using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class StageCreateWindow : ScriptableWizard
{
    [Header("MapDates")]
    [SerializeField]
    GameObject gameRoot;

    [SerializeField]
    TextAsset mapDate;

    public TileBase overTile;
    public TileBase load;
    public TileBase[] wall = new TileBase[2];
    public TileBase setTowet;
    public TileBase start;
    public TileBase goal;
    public List<MAP_DATE> mapDates = new List<MAP_DATE>();
    public Material overTileMaterial;

    [Header("Enemys")]
    [SerializeField]
    TextAsset enemyDate;

    [MenuItem("Create/StageCreateWindow")]
    static void ShowWindow()
    {
        GetWindow<StageCreateWindow>("StageCreateWindow");
    }

    private void OnWizardCreate()
    {

    }
}
