using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetsManager
{
	// ------------------ //
	// --- 公有成员
	// ------------------ //
	public static bool IsDone { get; private set; } = false;
	public static PartPrefabConfig DefaultPartPrefab { private set; get; }
	public static Dictionary<string, Texture2D> ChapterLabelToCoverTexture { private set; get; }

	// ------------------ //
	// --- 私有成员
	// ------------------ //

	// ------------------ //
	// --- 公有方法
	// ------------------ //
	public static IEnumerator InitAssets(Action<bool> initedCallBack = null)
	{
		float startTime = Time.realtimeSinceStartup;
		yield return Addressables.InitializeAsync();
		Debug.Log("初始化资源管理系统");

		yield return ProgressLoadChapterLabelsAndCoverTexture();
		Debug.Log("加载章节名称和纹理加载");
		yield return ProgressLoadSO();
		//Debug.Log("默认零件预 加载成功");

		Debug.Log("加载配置文件对象");
		Debug.Log($"加载完成 耗时{Time.realtimeSinceStartup - startTime}s");
		Application.quitting += Application_quitting;
		IsDone = true;
		initedCallBack?.Invoke(IsDone);
	}

	// ------------------ //
	// --- 私有方法
	// ------------------ //
	private AssetsManager() { }

	private static void Application_quitting()
	{
		Debug.Log("注意释放资源");
	}

	/// <summary>
	/// 有的关卡具有自己独特的零件预设
	/// </summary>
	/// <param name="chapterName"></param>
	/// <returns></returns>
	private static IEnumerator ProgressLoadSO()
	{
		//var HandlerLoadLocations = Addressables.LoadResourceLocationsAsync("SO", Addressables.MergeMode.Union);
		//yield return HandlerLoadLocations;
		//Debug.Log("SO 文件共" + HandlerLoadLocations.Result.Count);

		var handlerLoadAssets = Addressables.LoadAssetsAsync(new List<string> { "SO" }, new Action<UnityEngine.Object>(
			(soObject) => 
			{
				Debug.LogWarning($"加载SO文件:{soObject.GetType()}");
				if (soObject is UniqueConfigBase)
				{
					(soObject as UniqueConfigBase).InitInstance();
				}
				if (soObject is PartPrefabConfig)
				{
					DefaultPartPrefab = soObject as PartPrefabConfig;
					Debug.LogWarning("默认零件预制件已加载");
				}
			}
		), Addressables.MergeMode.UseFirst);
		yield return handlerLoadAssets;
	}

	/// <summary>
	/// 所有章节的主题纹理都在同一个addressable的group里 ChapterTextures
	/// 同一个章节的关卡场景,在同一个label下
	/// </summary>
	/// <returns>返回一个字典（章节名称:章节界面纹理）</returns>
	private static IEnumerator ProgressLoadChapterLabelsAndCoverTexture() 
	{
		ChapterLabelToCoverTexture = new Dictionary<string, Texture2D>();
		var handlerLoadChapterTexture = Addressables.LoadAssetsAsync(
			new List<string> { "ChapterTexture" },
			new Action<Texture2D>(
				(Texture2D loadedObject) => 
				{ 
					ChapterLabelToCoverTexture.Add(loadedObject.name, loadedObject); 
				}),
			Addressables.MergeMode.None
		);
		yield return handlerLoadChapterTexture;
		Debug.LogWarning($"章节封面纹理共计:{ChapterLabelToCoverTexture.Count}个");
		foreach (var item in ChapterLabelToCoverTexture)
		{
			Debug.Log("章节封面纹理:" + Path.GetFileNameWithoutExtension(item.Key));
		}


		//var loadChapterTextureHandler = Addressables.LoadResourceLocationsAsync("ChapterTexture");
		//yield return loadChapterTextureHandler;
		//ChapterLabelToCoverTexture = new Dictionary<string, Texture2D>(loadChapterTextureHandler.Result.Count);
		//Debug.LogWarning($"章节封面纹理共计:{ChapterLabelToCoverTexture.Count}个");
		//foreach (var item in loadChapterTextureHandler.Result)
		//{
		//	var textureLoadHandler = Addressables.LoadAssetAsync<Texture2D>(item);
		//	yield return textureLoadHandler;
		//	ChapterLabelToCoverTexture.Add(Path.GetFileNameWithoutExtension(item.PrimaryKey), textureLoadHandler.Result);
		//	Debug.Log("章节封面纹理:" + Path.GetFileNameWithoutExtension(item.PrimaryKey));
		//	yield return 0;
		//}
	}
}
