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
    string GAMEROOT_PATH = "Assets/Plefab/GameRoot.prefab";

    [SerializeField]
    TextAsset mapDate;
    string MAP_DATE_PATH = "Assets/Resources/Stages/00MapDates/testMap.csv";

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

    List<MAP_DATE> mapDates = new List<MAP_DATE>();
    public Material overTileMaterial;
    string OVERTILE_MATERIAL_PATH = "Assets/Map/Material/OverTileMaterial.mat";

    [Header("Enemys")]
    [SerializeField]
    TextAsset enemyDate;
    string ENEMY_DATE_PATH = "Assets/Resources/Stages/01testMap/Stage1.csv";

    private void Awake()
    {
        //初期化
        gameRoot = AssetDatabase.LoadAssetAtPath(GAMEROOT_PATH, typeof(GameObject)) as GameObject;
        mapDate = AssetDatabase.LoadAssetAtPath(MAP_DATE_PATH, typeof(TextAsset)) as TextAsset;
        overTileMaterial = AssetDatabase.LoadAssetAtPath(OVERTILE_MATERIAL_PATH, typeof(Material)) as Material;
        enemyDate = AssetDatabase.LoadAssetAtPath(ENEMY_DATE_PATH, typeof(TextAsset)) as TextAsset;

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

        string folderPath = AssetDatabase.CreateFolder("Assets/Resources/Stages",names.Length.ToString("00") + "TestFolder");
        
        string path = AssetDatabase.GUIDToAssetPath(folderPath);

        path += "/";

        MapDate date = MapDateCreator.CreateMapDate(mapDate, path);

        MapDateCreator.CreatePrefab(date, path);

        AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(enemyDate), path + enemyDate.name + ".csv");

        Debug.Log($"以下のフォルダに作成し\n{enemyDate.name}も移動しました\n{path}");
    }
}
