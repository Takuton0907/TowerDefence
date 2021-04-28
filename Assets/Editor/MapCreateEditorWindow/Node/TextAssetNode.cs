using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary> TextAsset処理するノード </summary>
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
