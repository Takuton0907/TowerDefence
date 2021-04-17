using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class StageCreateEditor : EditorWindow
{
    [MenuItem("Window/StageEditor")]
    public static void ShowWindow()
    {
        StageCreateEditor StageEditor = CreateInstance<StageCreateEditor>();
        StageEditor.Show();
        StageEditor.titleContent = new GUIContent("Stage Editor");
    }

    public void OnEnable()
    {
        
    }
}
public class NodeElement : VisualElement
{
    public NodeElement( string name, Color color, Vector2 pos)
    {
        style.backgroundColor = new StyleColor(color);
        transform.position = pos;

        Add(new Label(name));
    }
}
