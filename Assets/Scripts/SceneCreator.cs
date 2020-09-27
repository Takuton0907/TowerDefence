using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCreator : EditorWindow
{
    //ビルドに設定するシーンが入ったディレクトリへのパス
    private const string STAGE_FILE_PATH = "Assets/Scenes/GameScenes/";

    [MenuItem("Create/StageScene")]
    private static void CreateSceneAsset()
    {
        EditorBuildSettingsScene scenePath = new EditorBuildSettingsScene(STAGE_FILE_PATH, true);

        Scene scene = SceneManager.GetSceneByPath(scenePath.path);

        Scene fsaf = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        fsaf.name = "Teeeset";
        EditorSceneManager.SaveScene(fsaf, scenePath.path + "Teeeset.unity");
        scenePath.path += "Teeeset.unity";
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        ArrayUtility.Add(ref scenes, scenePath);
        EditorBuildSettings.scenes = scenes;
    }

    private static string GetPath()
    {
        var instanceId = Selection.activeInstanceID;
        var path = AssetDatabase.GetAssetPath(instanceId);
        path = string.IsNullOrEmpty(path) ? "Assets" : path;

        if (Directory.Exists(path))
        {
            return path;
        }
        if (File.Exists(path))
        {
            var parent = Directory.GetParent(path);
            var fullName = parent.FullName;
            var unixFileName = fullName.Replace("\\", "/");
            return FileUtil.GetProjectRelativePath(unixFileName);
        }
        return string.Empty;
    }
}