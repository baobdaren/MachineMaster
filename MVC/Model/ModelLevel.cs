using System;
using System.Collections.Generic;

public class ModelLevel:ResetAbleInstance<ModelLevel>
{
	// 单例

	// 所有关卡的字典
	private readonly Dictionary<int, Level> Levels = new Dictionary<int, Level>();


	/// <summary>
	/// 初始化关卡
	/// 从json数据表格读取，并保存到Levels
	/// </summary>
	public ModelLevel ( )
	{
		//var bundle = AssetLoader.BundleLoad(BundlePaths.Paths_Level.LEVEL_BUNDLENAME);
		//if (bundle == null) {
		//    Debug.LogError("关卡配置的json文件读取失败，路径=" + BundlePaths.Paths_Level.LEVEL_BUNDLENAME);
		//    return;
		//}
		//TextAsset levelText = bundle.LoadAsset(BundlePaths.Paths_Level.LEVEL_TEXTNAME) as TextAsset;
		//LevelDataArray levelsJData = JsonUtility.FromJson<LevelDataArray>(levelText.text);
		//foreach (LevelJsonData item in levelsJData.arr) {
		//    Levels.Add(item.ID, new Level(item));
		//}
		//Debug.LogError("读取关卡完毕，关卡数量=" + Levels.Count);
	}
	public struct Level
	{
		public Level ( LevelJsonData data )
		{
			name = data.Name;
			nextLevels = new List<int>(data.NextLevelIDs);
			unlockScore = data.UnlockScore;
			Unlocked = false;
			score = 0;
			lastLevels = new List<int>();
		}
		public readonly string name;
		public readonly List<int> nextLevels;
		public readonly int unlockScore;

		public int score;
		public List<int> lastLevels;
		public bool Unlocked
		{
			get; private set;
		}
		public bool CanUnlockNext => score >= unlockScore;
	}

	[Serializable]
	public class LevelJsonData
	{
		public int ID;
		public string Name;
		public int[] NextLevelIDs;
		public int UnlockScore;
	}

	[Serializable]
	public class LevelDataArray
	{
		public LevelJsonData[] arr;
	}
}
