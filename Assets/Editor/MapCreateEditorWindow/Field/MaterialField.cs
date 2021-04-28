using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

/// <summary>  Materialを管理するField </summary>
public class MaterialField : ObjectField
{
    public MaterialField(string label, string tooltip = "", string defaultPath = "") : base()
    {
        objectType = typeof(Material);
        base.label = label;
        base.tooltip = tooltip;
        base.value = AssetDatabase.LoadAssetAtPath(defaultPath, typeof(Material)) as Material;
    }
}