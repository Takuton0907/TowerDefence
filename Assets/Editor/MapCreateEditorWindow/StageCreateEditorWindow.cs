using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class StageCreateEditorWindow : EditorWindow
{
    [MenuItem("Window/StageEditor")]
    public static void Open()
    {
        GetWindow<StageCreateEditorWindow>(ObjectNames.NicifyVariableName(nameof(StageCreateEditorWindow)));
    }

    void OnEnable()
    {
        var graphView = new MapCreateGraphView(this);
        rootVisualElement.Add(graphView);
    }
}
