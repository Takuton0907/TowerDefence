using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;
using UnityEditor;
using System;
using System.Linq;

public class StageCreateEditorWindow : EditorWindow
{
    MapCreateGraphView graphView;

    [MenuItem("Window/StageEditor")]
    public static void Open()
    {
        GetWindow<StageCreateEditorWindow>(ObjectNames.NicifyVariableName(nameof(StageCreateEditorWindow)));
    }

    void OnEnable()
    {
        graphView = new MapCreateGraphView(this);
        rootVisualElement.Add(graphView);
        rootVisualElement.style.backgroundColor = Color.gray;

        rootVisualElement.Add(new RunElement("Save", new Color(0.29f, 0.29f, 0.29f), new Vector2(0, 10), OnClickRun));
    }
    //saveの実行
    private void OnClickRun()
    {
        MapNode mapNode = graphView._mapNode;
        var inputChilds = mapNode.inputContainer.contentContainer.Children();
       
        foreach (var input in mapNode._inputPorts)
        {
            //extensionContainerに設定してある値の参照の取得
            var extensionChildren = input.connections.Select(connect => connect.output?.node.extensionContainer.Children());

            foreach (var child in extensionChildren)
            {
                foreach (var item in child)
                {
                    if (item is TileField field)
                    {
                        //値の使用
                        Debug.Log(field.value);
                    }
                }
            }
        }
        

        //string[] names = AssetDatabase.FindAssets("t:Folder", new[] { "Assets/Resources/Stages" });

        //string folderPath = AssetDatabase.CreateFolder("Assets/Resources/Stages", names.Length.ToString("00") + mapData.name);

        //string path = AssetDatabase.GUIDToAssetPath(folderPath);

        //path += "/";

        //MapDataObject date = MapDataCreator.CreateMapData(mapData, path);

        //date.load = load;
        //date.wall = wall;
        //date.setTowet = setTower;
        //date.overTile = overTile;
        //date.start = start;
        //date.goal = goal;

        //date.overTileMaterial = overTileMaterial;

        //Debug.Log(date.load);

        //MapDataCreator.CreatePrefab(date, path);

        //AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(enemyData), path + enemyData.name + ".csv");

        //Debug.Log($"以下のフォルダに作成し\n{enemyData.name}も移動しました\n{path}");
    }
}


public class RunElement : VisualElement
{
    public RunElement(string name, Color color, Vector2 pos, Action clickAction)
    {
        style.backgroundColor = new StyleColor(color);
        style.position = Position.Absolute;
        style.height = 50;
        style.width = 100;
        transform.position = pos;

        Add(new Label(name));
        Button runBytton = new Button();
        runBytton.text = "Save";
        runBytton.clickable.clicked += clickAction;
        Add(runBytton);
    }
}
