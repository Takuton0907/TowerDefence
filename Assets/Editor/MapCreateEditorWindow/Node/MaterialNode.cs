using UnityEditor.Experimental.GraphView;
using UnityEngine;

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
