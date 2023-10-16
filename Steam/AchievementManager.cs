using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Steamworks.Data;
using UnityEngine;

public class AchievementManager
{
	// ------------- //
	// -- 序列化
	// ------------- //

	// ------------- //
	// -- 私有成员
	// ------------- //
	private Dictionary<AchievementTypes, string> AchievementsDicts = new Dictionary<AchievementTypes, string>()
	{
		[AchievementTypes.Ach_StartGame] = "Ach_StartGame",
		[AchievementTypes.Ach_Aircraft] = "Ach_Aircraft",
		[AchievementTypes.Ach_NewPlayer] = "Ach_NewPlayer",
	};

	// ------------- //
	// -- 公有成员
	// ------------- //
	public static AchievementManager Instance = new AchievementManager();

	// ------------- //
	// -- 公有方法
	// ------------- //
	public void UnlockAchievement(AchievementTypes achievementName)
	{
		Debug.Assert(AchievementsDicts.ContainsKey(achievementName));
		Achievement achData = new Achievement(AchievementsDicts[achievementName]);
		achData.Trigger(true);
		Debug.Log($"解锁成就 {achievementName}");
	}

	public void SetStats(string name, int addNum = 1)
	{
		Steamworks.SteamUserStats.SetStat(name, addNum);
		Steamworks.SteamUserStats.StoreStats();
		Debug.Log($"修改统计 {name}={Steamworks.SteamUserStats.GetStatInt(name)}");
	}

	public void Test()
	{
		int finishLevelAmount = Steamworks.SteamUserStats.GetStatInt("FinishLevels");
		Debug.Log("完成关卡统计：" + finishLevelAmount);
	}

	// ------------- //
	// -- 私有方法
	// ------------- //
	private AchievementManager(){}

	// ------------- //
	// -- 类型
	// ------------- //
	public enum AchievementTypes
	{
		Ach_StartGame, Ach_Aircraft, Ach_NewPlayer
	}
}

