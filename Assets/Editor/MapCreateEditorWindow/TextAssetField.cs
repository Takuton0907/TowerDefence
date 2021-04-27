using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class TextAssetField : ObjectField
{
    public TextAssetField(string label, string tooltip = "", string defaultPath = "") : base()
    {
        objectType = typeof(TextAsset);

        base.label = label;
        base.tooltip = tooltip;
        base.value = AssetDatabase.LoadAssetAtPath(defaultPath, typeof(TextAsset)) as TextAsset;
    }
}
