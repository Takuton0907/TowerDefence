using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

/// <summary> stageを生成するEditor拡張 </summary>
public class StageCreateWindow : ScriptableWizard
{
    [Header("MapDates")]
    [SerializeField]
    GameObject gameRoot = null;
    string GAMEROOT_PATH = "Assets/Plefab/Managers/GameRoot.prefab";

    [SerializeField]
    TextAsset mapData = null;

    TileBase overTile;
    public TileBase load;
    public TileBase[] wall = new TileBase[2];
    public TileBase setTower;
    public TileBase start;
    public TileBase goal;

    string WALL_TILEBASE_PATH = "Assets/AssetStore/Texture/Backyard - Free/Separate Tiles/backyard_00.asset";
    string LOAD_TILEBASE_PATH = "Assets/AssetStore/Texture/Backyard - Free/Separate Tiles/backyard_50.asset";
    string SETTOWER_TILEBASE_PATH = "Assets/AssetStore/Texture/Backyard - Free/Separate Tiles/backyard_50.asset";
    string START_TILEBASE_PATH = "Assets/AssetStore/Texture/Backyard - Free/Separate Tiles/backyard_79.asset";
    string GOAL_TILEBASE_PATH = "Assets/AssetStore/Texture/Backyard - Free/Separate Tiles/backyard_78.asset";

    List<GRID_DATA> mapDatas = new List<GRID_DATA>();
    public Material overTileMaterial;
    string OVERTILE_MATERIAL_PATH = "Assets/Map/Material/OverTileMaterial.mat";

    [Header("Enemys")]
    [SerializeField]
    TextAsset enemyData = null;

    private void Awake()
    {
        //初期化
        gameRoot = AssetDatabase.LoadAssetAtPath(GAMEROOT_PATH, typeof(GameObject)) as GameObject;
        overTileMaterial = AssetDatabase.LoadAssetAtPath(OVERTILE_MATERIAL_PATH, typeof(Material)) as Material;

        load = AssetDatabase.LoadAssetAtPath(LOAD_TILEBASE_PATH, typeof(TileBase)) as TileBase;
        TileBase tileBase = AssetDatabase.LoadAssetAtPath(WALL_TILEBASE_PATH, typeof(TileBase)) as TileBase;
        for (int i = 0; i < wall.Length; i++)
        {
            wall[i] = tileBase;
        }
        tileBase = AssetDatabase.LoadAssetAtPath(SETTOWER_TILEBASE_PATH, typeof(TileBase)) as TileBase;
        setTower = tileBase;
        overTile = tileBase;
        start = AssetDatabase.LoadAssetAtPath(START_TILEBASE_PATH, typeof(TileBase)) as TileBase;
        goal = AssetDatabase.LoadAssetAtPath(GOAL_TILEBASE_PATH, typeof(TileBase)) as TileBase;
    }

    [MenuItem("Create/StageCreateWindow")]
    static void ShowWindow()
    {
        GetWindow<StageCreateWindow>("StageCreateWindow");
    }

    private void OnWizardCreate()
    {
        string[] names = AssetDatabase.FindAssets("t:Folder", new[] { "Assets/Resources/Stages" });

        string folderPath = AssetDatabase.CreateFolder("Assets/Resources/Stages",names.Length.ToString("00") + mapData.name);
        
        string path = AssetDatabase.GUIDToAssetPath(folderPath);

        path += "/";

        MapDataObject date = MapDataCreator.CreateMapData(mapData, path);

        date.load = load;
        date.wall = wall;
        date.setTowet = setTower;
        date.overTile = overTile;
        date.start = start;
        date.goal = goal;

        date.overTileMaterial = overTileMaterial;

        Debug.Log(date.load);

        MapDataCreator.CreatePrefab(date, path);

        AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(enemyData), path + enemyData.name + ".csv");

        Debug.Log($"以下のフォルダに作成し\n{enemyData.name}も移動しました\n{path}");
    }
}
