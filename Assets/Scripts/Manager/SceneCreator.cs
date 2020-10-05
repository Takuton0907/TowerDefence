using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
public static class SceneCreator
{
    //ビルドに設定するシーンが入ったディレクトリへのパス
    private const string STAGE_FILE_PATH = "Assets/Scenes/GameScenes/";

    public static Scene SceneAddBuildSetting(string name)
    {
        EditorBuildSettingsScene scenePath = new EditorBuildSettingsScene(STAGE_FILE_PATH, true);

        Scene scene = SceneManager.GetSceneByPath(scenePath.path);

        Scene fsaf = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        fsaf.name = name;
        EditorSceneManager.SaveScene(fsaf, scenePath.path + name +".unity");
        scenePath.path += name + ".unity";
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        ArrayUtility.Add(ref scenes, scenePath);
        EditorBuildSettings.scenes = scenes;

        return scene;
    }
}
#endif