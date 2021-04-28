using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

/// <summary> GameObject処理するノード </summary>
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
