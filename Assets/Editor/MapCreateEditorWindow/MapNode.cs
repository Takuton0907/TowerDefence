using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

/// <summary> マップデータのノード </summary>
public class MapNode : Node
{
    public List<Port> _inputPorts = new List<Port>();

    public MapNode()
    {
        title = "Map Data";
        // 入力用のポートを作成

        var inputPortGameRoot = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(GameObject));
        inputPortGameRoot.portName = "GameRoot";
        inputContainer.Add(inputPortGameRoot);
        _inputPorts.Add(inputPortGameRoot);

        var inputPortMapData = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TextAsset));
        inputPortMapData.portName = "MapData";
        inputContainer.Add(inputPortMapData);
        _inputPorts.Add(inputPortMapData);

        var inputPortLoad = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortLoad.portName = "LoadTile";
        inputContainer.Add(inputPortLoad);
        _inputPorts.Add(inputPortLoad);

        var inputPortWall = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase[]));
        inputPortWall.portName = "WallTile";
        inputContainer.Add(inputPortWall);
        _inputPorts.Add(inputPortWall);

        var inputPortSetTower = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortSetTower.portName = "SetTowerTile";
        inputContainer.Add(inputPortSetTower);
        _inputPorts.Add(inputPortSetTower);

        var inputPortStart = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortStart.portName = "StartTile";
        inputContainer.Add(inputPortStart);
        _inputPorts.Add(inputPortStart);

        var inputPortGoal = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortGoal.portName = "GoalTile";
        inputContainer.Add(inputPortGoal);
        _inputPorts.Add(inputPortGoal);

        var inputPortOverMaterial = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Material));
        inputPortOverMaterial.portName = "OverTileMaterial";
        inputContainer.Add(inputPortOverMaterial);
        _inputPorts.Add(inputPortOverMaterial);

        var inputPortEnemyData = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TextAsset));
        inputPortEnemyData.portName = "EnemyData";
        inputContainer.Add(inputPortEnemyData);
        _inputPorts.Add(inputPortEnemyData);
    }
}
/// <summary> tilebaseを処理するノード </summary>
public class TileNode : Node
{
    public TileNode()
    {
        title = "Tile";
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(TileBase));
        port.portName = "Value";
        outputContainer.Add(port);

        var tileField = new TileField("Tile", defaultPath: "Assets/AssetStore/Texture/Backyard - Free/Separate Tiles/backyard_00.asset");
        port.userData = tileField.value;
        extensionContainer.Add(tileField);
        RefreshExpandedState();
    }
}
/// <summary> tilebaseの配列を処理するノード </summary>
public class TileArrayNode : Node
{
    List<TileField> _tileBases = new List<TileField>();

    public TileArrayNode()
    {
        title = "TileArray";
        //portの作成
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(TileBase[]));
        port.portName = "Value";
        port.userData = _tileBases;

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
/// <summary> Material処理するノード </summary>
public class MaterialNode : Node
{
    public MaterialNode()
    {
        title = "Material";
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Material));
        port.portName = "Value";
        outputContainer.Add(port);

        var field = new MaterialField("Material", defaultPath: "Assets/Map/Material/OverTileMaterial.mat");
        port.userData = field.value;
        extensionContainer.Add(field);
        RefreshExpandedState();
    }
}

public class TextAssetNode : Node
{
    public TextAssetNode()
    {
        title = "TextAsset";
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(TextAsset));
        port.portName = "Value";
        outputContainer.Add(port);

        var field = new TextAssetField("TextAsset");
        port.userData = field.value;
        extensionContainer.Add(field);
        RefreshExpandedState();
    }
}

public class GameObjectNode : Node
{
    public GameObjectNode()
    {
        title = "Object";
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(GameObject));
        port.portName = "Value";
        outputContainer.Add(port);

        var field = new ObjectField("GameObject");
        field.objectType = typeof(GameObject);
        port.userData = field.value;
        extensionContainer.Add(field);
        RefreshExpandedState();
    }
}

public class OutputNode : Node
{
    public OutputNode()
    {
        title = "Output";
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
        port.portName = "Value";
        inputContainer.Add(port);
    }
}

public class AddNode : Node
{
    public AddNode()
    {
        title = "Add";

        var inputPort1 = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
        inputPort1.portName = "A";
        inputContainer.Add(inputPort1);

        var inputPort2 = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
        inputPort2.portName = "B";
        inputContainer.Add(inputPort2);

        var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
        outputPort.portName = "Out";
        outputContainer.Add(outputPort);
    }
}

public class ValueNode : Node
{
    public ValueNode()
    {
        title = "Value";

        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
        port.portName = "Value";
        outputContainer.Add(port);

        extensionContainer.Add(new FloatField());
        RefreshExpandedState();
    }
}