using System;
using UnityEditor;
using UnityEngine;

public class InitializerTable : ScriptableObject
{
    public Tobj[] Objects;

    [MenuItem("Assets/Create/LocalEffectSetting")]
    static void CreateLocalEffectSetting()
    {
        InitializerTable localEffectSetting = CreateInstance<InitializerTable>();
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources");
        AssetDatabase.CreateAsset(localEffectSetting, path);
        AssetDatabase.Refresh();
    }
}


[Serializable]
public class Tobj
{
    public GameObject obj;
    public bool DontDestroy;
}
