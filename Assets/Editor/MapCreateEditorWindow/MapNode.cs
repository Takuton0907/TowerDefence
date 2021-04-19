using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapNode : Node
{
    public MapNode()
    {
        title = "Map Data";
        // 入力用のポートを作成
        var inputPortGameRoot = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(GameObject));
        inputPortGameRoot.portName = "GameRoot";
        inputContainer.Add(inputPortGameRoot);

        var inputPortMapData = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TextAsset));
        inputPortMapData.portName = "MapData";
        inputContainer.Add(inputPortMapData);

        var inputPortLoad = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Tile));
        inputPortLoad.portName = "LoadTile";
        inputContainer.Add(inputPortLoad);

        var inputPortWall = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Tile[]));
        inputPortWall.portName = "WallTile";
        inputContainer.Add(inputPortWall);

        var inputPortSetTower = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Tile));
        inputPortSetTower.portName = "SetTowerTile";
        inputContainer.Add(inputPortSetTower);

        var inputPortStart = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Tile));
        inputPortStart.portName = "StartTile";
        inputContainer.Add(inputPortStart);

        var inputPortGoal = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Tile));
        inputPortGoal.portName = "GoalTile";
        inputContainer.Add(inputPortGoal);

        var inputPortOverMaterial = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Tile));
        inputPortOverMaterial.portName = "OverTileMaterial";
        inputContainer.Add(inputPortOverMaterial);

        var inputPortEnemyData = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TextAsset));
        inputPortEnemyData.portName = "MapData";
        inputContainer.Add(inputPortEnemyData);
    }
}

public class TileNode : Node
{
    public TileNode()
    {
        title = "TileArray";

        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Tile));
        port.portName = "Value";
        outputContainer.Add(port);

        extensionContainer.Add(new FloatField());
        RefreshExpandedState();
    }


}
public class OutputNode : Node
{
    public OutputNode()
    {
        title = "Output";
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
        port.portName = "Value";
        inputContainer.Add(port);
    }
}

public class AddNode : Node
{
    public AddNode()
    {
        title = "Add";

        var inputPort1 = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float[]));
        inputPort1.portName = "A";
        inputContainer.Add(inputPort1);

        var inputPort2 = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
        inputPort2.portName = "B";
        inputContainer.Add(inputPort2);

        var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
        outputPort.portName = "Out";
        outputContainer.Add(outputPort);
    }
}

public class ValueNode : Node
{
    public ValueNode()
    {
        title = "Value";

        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
        port.portName = "Value";
        outputContainer.Add(port);

        extensionContainer.Add(new FloatField());
        RefreshExpandedState();
    }
}