using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MaterialField : BaseField<Material>
{
    public MaterialField(string label, VisualElement visualInput, string tooltip = "", string defaultPath = "") : base(label, visualInput)
    {
        base.label = label;
        base.labelElement.style.color = Color.white;
        base.tooltip = tooltip;
        base.style.backgroundColor = new Color(0.294f, 0.294f, 0.294f);
        base.value = AssetDatabase.LoadAssetAtPath(defaultPath, typeof(Material)) as Material;
    }
}