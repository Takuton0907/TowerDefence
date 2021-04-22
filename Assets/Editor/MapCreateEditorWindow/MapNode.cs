using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

/// <summary> マップデータのノード </summary>
public class MapNode : NodeBase
{
    MapData _mapData = new MapData();

    public MapNode()
    {
        title = "Map Data";
        // 入力用のポートを作成

        var inputPortGameRoot = CreatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(GameObject));
        inputPortGameRoot.portName = "GameRoot";
        inputPortGameRoot.userData = _mapData._gameRoot;
        inputContainer.Add(inputPortGameRoot);

        var inputPortMapData = CreatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TextAsset));
        inputPortMapData.portName = "MapData";
        inputPortMapData.userData = _mapData._mapData;
        inputContainer.Add(inputPortMapData);

        var inputPortLoad = CreatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortLoad.portName = "LoadTile";
        inputPortLoad.userData = _mapData._load;
        inputContainer.Add(inputPortLoad);

        var inputPortWall = CreatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase[]));
        inputPortWall.portName = "WallTile";
        inputPortWall.userData = _mapData._wall;
        inputContainer.Add(inputPortWall);

        var inputPortSetTower = CreatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortSetTower.portName = "SetTowerTile";
        inputPortSetTower.userData = _mapData._setTower;
        inputContainer.Add(inputPortSetTower);

        var inputPortStart = CreatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortStart.portName = "StartTile";
        inputPortStart.userData = _mapData._start;
        inputContainer.Add(inputPortStart);

        var inputPortGoal = CreatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortGoal.portName = "GoalTile";
        inputPortGoal.userData = _mapData._goal;
        inputContainer.Add(inputPortGoal);

        var inputPortOverMaterial = CreatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Material));
        inputPortOverMaterial.portName = "OverTileMaterial";
        inputPortOverMaterial.userData = _mapData._overTileMaterial;
        inputContainer.Add(inputPortOverMaterial);

        var inputPortEnemyData = CreatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TextAsset));
        inputPortEnemyData.portName = "MapData";
        inputPortEnemyData.userData = _mapData._enemyData;
        inputContainer.Add(inputPortEnemyData);
    }
}
/// <summary> tilebaseを処理するノード </summary>
public class TileNode : NodeBase
{
    TileField _tileField = default;

    public TileNode()
    {
        title = "Tile";
        var port = CreatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(TileBase));
        port.portName = "Value";
        port.userData = _tileField;
        outputContainer.Add(port);
        
        var tileField = new TileField("Tile", defaultPath: "Assets/AssetStore/Texture/Backyard - Free/Separate Tiles/backyard_00.asset");
        extensionContainer.Add(tileField);
        _tileField = tileField;
        RefreshExpandedState();
    }
}
/// <summary> tilebaseの配列を処理するノード </summary>
public class TileArrayNode : NodeBase
{
    List<TileField> _tileBases = new List<TileField>();

    public TileArrayNode()
    {
        title = "TileArray";
        //portの作成
        var port = CreatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(TileBase[]));
        port.portName = "Value";
        port.userData = _tileBases;
        Debug.Log(port.edgeConnector);

        outputContainer.Add(port);

        //dataの設定とリストへの追加
        var tileField = new TileField("Tile", defaultPath: "Assets/AssetStore/Texture/Backyard - Free/Separate Tiles/backyard_00.asset");
        extensionContainer.Add(tileField);
        _tileBases.Add(tileField);
        RefreshExpandedState();

        var box = new Box();

        //Addボタンを作成
        var addbutton = new Button();
        addbutton.clickable.clicked += () => {
            //新たなTileFieldを作成
            tileField = new TileField("Tile", defaultPath: "Assets/AssetStore/Texture/Backyard - Free/Separate Tiles/backyard_00.asset");
            int index = extensionContainer.IndexOf(box);
            extensionContainer.Insert(index, tileField);
            _tileBases.Add(tileField);
        };
        addbutton.text = "Add";

        //removeボタンの作成
        var removeButton = new Button();
        removeButton.clickable.clicked += () =>
        {
            //一番下のデータを削除
            int index = extensionContainer.IndexOf(box) - 1;
            if (index <= 0)
            {
                return;
            }
            extensionContainer.RemoveAt(index);
            _tileBases.RemoveAt(index);
        };
        removeButton.text = "Remove";

        // Boxの子としてボタンを追加
        box.Add(addbutton);
        box.Add(removeButton);
        // ルートの子としてBoxを追加
        extensionContainer.Add(box);
    }
}
public class OutputNode : NodeBase
{
    public OutputNode()
    {
        title = "Output";
        var port = CreatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
        port.portName = "Value";
        inputContainer.Add(port);
    }
}

public class AddNode : NodeBase
{
    public AddNode()
    {
        title = "Add";

        var inputPort1 = CreatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float[]));
        inputPort1.portName = "A";
        inputContainer.Add(inputPort1);

        var inputPort2 = CreatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
        inputPort2.portName = "B";
        inputContainer.Add(inputPort2);

        var outputPort = CreatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
        outputPort.portName = "Out";
        outputContainer.Add(outputPort);
    }
}

public class ValueNode : NodeBase
{
    public ValueNode()
    {
        title = "Value";

        var port = CreatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
        port.portName = "Value";
        outputContainer.Add(port);

        extensionContainer.Add(new FloatField());
        RefreshExpandedState();
    }
}