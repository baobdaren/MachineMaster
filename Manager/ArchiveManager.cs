using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class ArchiveManager
{
	// ------------------ //    
	// --- 序列化    
	// ------------------ //

	// ------------------ //    
	// --- 公有成员    
	// ------------------ //
	public static ArchiveManager Instance = new ArchiveManager();
	//public Archive CurrentArchive { get; private set; }

	private List<ISaveable> _allManagers = new List<ISaveable>()
	{
		PlayerPartManager.Instance,
		SensorManager.Instance,
		PartConnectionManager.Instance,
		ModelMapManager.Instance,
		PlayerManager.Instance,
		ScenePartManager.Instance,
	};

	public UnityEvent OnArchiveApplied = new UnityEvent();

	// ------------------ //
	// --- 私有成员    
	// ------------------ //

	// ------------------ //    
	// --- Unity消息    
	// ------------------ //

	// ------------------ //    
	// --- 公有方法   
	// ------------------ //
	private ArchiveManager()
	{ 
		GameManager.Instance.OnLevelLoaded.AddListener(OnEnterLevel_LoadArchive);
	}

	public void SaveLevelArchive()
	{
		Debug.Log("保存存档");
		//foreach (var item in PartManager.Instance.AllPartCtrls)
		//{
		//	item.UpdateDataFromAccesstor();
		//}
		//if (PlayerManager.Instance.Player.SaveablePosition.HasValue == false)
		//{
		//	return;
		//}
		Archive archive = new Archive();
		//PlayerPartManager.Instance.SaveArchive(archive);
		//SensorManager.Instance.SaveArchive(archive);
		//PartConnectionManager.Instance.SaveArchive(archive);
		//ModelMapManager.Instance.SaveArchive(archive);
		//PlayerManager.Instance.SaveArchive(archive);
		//archive.playerPos = GameManager.Instance.pla
		foreach (ISaveable iteMgrs in _allManagers)
		{
			iteMgrs.SaveIntoArchive(archive);
		}
		ES3.Save("Archive", archive, GetSavePath());
		return;
    }

	public static string CombinArchiveName(string chapterName, string levelName)
	{
		return $"[{chapterName}][{levelName}]";
	}

	public static bool HasArchive(string chapterName, string levelName)
	{
		//ES3.RenameFile(ES3Settings.defaultSettings)
		//return ES3.KeyExists(CombinArchiveName(chapterName, levelName, ArchiveName));
		return ES3.FileExists(GetSavePath(chapterName, levelName));
	}

	public static string GetSavePath(string chapterName = null, string levelName = null)
	{
		if (chapterName == null) chapterName = GameManager.Instance.SelectedChapterName;
		if (levelName == null) levelName = GameManager.Instance.SelectedLevelName;
		// ES3Settings.defaultSettings.FullPath 是默认保存文件的路径,这里取文件夹
		return Path.Combine(Path.GetDirectoryName(ES3Settings.defaultSettings.FullPath), Path.GetFileName(chapterName), levelName);
	}
	// ------------------ //   
	// --- 私有方法
	// ------------------ //
	private void OnEnterLevel_LoadArchive(string chapterName, string levelName)
	{
		if (HasArchive(chapterName, levelName))
		{
			Debug.Log($"正在加载{GetSavePath(chapterName, levelName)}的存档");
			//ApplyArchive(ES3.Load<Archive>(GetSavePath(chapterName, levelName)));
			ApplyArchiveToManagers(ES3.Load<Archive>("Archive", GetSavePath(chapterName, levelName)));

			//foreach (var item in PartManager.Instance.AllPartCtrls)
			//{
			//	Debug.LogError($"ID {item.GetHashCode()}");
			//}
			//foreach (var item in PartConnectionManager.Instance.AllConnection)
			//{
			//	Debug.LogError($"conn {item.MasterPart.GetHashCode()} - {item.TargetPart.GetHashCode()}");
			//}
		}
		OnArchiveApplied?.Invoke();
	}

	private void ApplyArchiveToManagers(Archive archive)
	{
		Debug.Log("----加载存档----");
		// 应用存档
		foreach (var item in _allManagers)
		{
			item.LoadArchive(archive);
		}
	}

	// ------------------ //   
	// --- 类型
	// ------------------ //
	[Serializable]
	public class Archive
	{
		public List<PlayerPartCtrlData> playerPartList;
		public List<ScenePartCtrlData> scenePartList;
		public List<PartConnectionCoreData> partConnectionsArchive;

		public Dictionary<int, (List<ModelingExpressionNode>, List<SymbolConnectArchiveData>)> partModelingMapsArchive;

		public List<(int, int, bool)> partCollisionArchive;

		public SensorManager sensorMgrArchive;
		public SymbolConnector symbolConnectMgrArchive;

		public Archive() { }
	}

	/// <summary>
	/// 由于引用关系在ES3里不能保存
	/// 使用特殊的数据结构来保存连接
	/// 这时一个node到另外一个node的一个连接
	/// </summary>
	[Serializable]
	public struct SymbolConnectArchiveData
	{
		public int outputIndex;
		public int outputIO;
		public int inputIndex;
		public int inputIO;
	}
}
