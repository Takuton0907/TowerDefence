using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEditor;

public class StagePostoro : AssetPostprocessor
{
	const string STAGE_PATH = "";

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string str in importedAssets)
        {
            string ext = Path.GetExtension(str);
            string folderPath = str.Substring(0, str.Length - AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(str).name.Length - ext.Length - "/".Length);
            Debug.Log(folderPath);


        }
        foreach (string str in deletedAssets)
        {
            Debug.Log("Deleted Asset: " + str);
        }

        for (int i = 0; i < movedAssets.Length; i++)
        {
            Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
        }
    }


	//オーディオファイルが入ってるディレクトリへのパス

	//=================================================================================
	//変更の監視
	//=================================================================================

#if !UNITY_CLOUD_BUILD
	//入力されたassetsのパスの中に、指定したパスが含まれるものが一つでもあるか
	private static bool ExistsPathInAssets(List<string[]> assetPathsList, string targetPath)
	{
		return assetPathsList
			.Any(assetPaths => assetPaths
				.Any(assetPath => assetPath
					.Contains(targetPath)));
	}

#endif

	//=================================================================================
	//スクリプト作成
	//=================================================================================

	[MenuItem("Tools/AudioManager/Create BGM&SE Path")]
	private static void CreateAudioPath()
	{
		CreateBGMPath();
	}

	[MenuItem("Tools/AudioManager/Create Path")]
	private static void CreateBGMPath()
	{
		Create(STAGE_PATH);
	}

	private static void Create(string directoryPath)
	{
		string directoryName = Path.GetFileName(directoryPath);
		var audioPathDict = new Dictionary<string, string>();

		foreach (var audioClip in Resources.LoadAll<AudioClip>(directoryName))
		{
			//アセットへのパスを取得
			var assetPath = AssetDatabase.GetAssetPath(audioClip);

			//Resources以下のパス(拡張子なし)に変換
			var targetIndex = assetPath.LastIndexOf("Resources", StringComparison.Ordinal) + "Resources".Length + 1;
			var resourcesPath = assetPath.Substring(targetIndex);
			resourcesPath = resourcesPath.Replace(Path.GetExtension(resourcesPath), "");

			var audioName = audioClip.name;
			if (audioPathDict.ContainsKey(audioName))
			{
				Debug.LogError(audioName + " is duplicate!\n1 : " + resourcesPath + "\n2 : " + audioPathDict[audioName]);
			}
			audioPathDict[audioName] = resourcesPath;
		}

		//このスクリプトがある所へのパス取得し、定数クラスを書き出す場所を決定
		string selfFileName = "AudioPathCreator.cs";
		string selfPath = Directory.GetFiles("Assets", "*", System.IO.SearchOption.AllDirectories)
			.FirstOrDefault(path => System.IO.Path.GetFileName(path) == selfFileName);

		string exportPath = selfPath.Replace(selfFileName, "").Replace("Editor", "Scripts");

		//定数クラス作成
		ConstantsClassCreator.Create(directoryName + "Path", directoryName + "ファイルへのパスを定数で管理するクラス", audioPathDict, exportPath);
	}
}