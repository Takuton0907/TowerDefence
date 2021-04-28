using UnityEditor.Experimental.GraphView;
using UnityEngine.Tilemaps;

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
