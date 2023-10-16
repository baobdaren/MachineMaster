using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePersistentData 
{
    // ------------------ //    
    // --- 公有成员    
    // ------------------ //
    public static GamePersistentData Instance = new GamePersistentData();

    public float VolumeData
    {
        get => PlayerPrefs.HasKey(VolumeKey) ? PlayerPrefs.GetFloat(VolumeKey) : 0.8f;
        set => PlayerPrefs.SetFloat(VolumeKey, value);
    }

    public float ScrollerSpeedData
    {
        get => PlayerPrefs.HasKey(ScrollSpeedKey) ? PlayerPrefs.GetFloat(ScrollSpeedKey) : 0.5f;
        set => PlayerPrefs.SetFloat(ScrollSpeedKey, value);
    }

    public bool VerticalHoldData
    {
        get => PlayerPrefs.HasKey(VerticalHoldKey) ? (PlayerPrefs.GetInt(VerticalHoldKey)==1) : false;
        set => PlayerPrefs.SetInt(VerticalHoldKey, value?1:0);
    }


    // ------------------ //   
    // --- 私有成员    
    // ------------------ //
    private const string VolumeKey = "Volume";
    private const string ScrollSpeedKey = "ScrollSpeed";
    private const string VerticalHoldKey = "VerticalHold";


    // ------------------ //    
    // --- 公有方法   
    // ------------------ //

    ///// <summary>
    ///// 读取关卡进度
    ///// 数据格式
    ///// {章节前缀}{章节名}{关卡前缀}{关卡名}：分数
    ///// </summary>
    ///// <returns></returns>
    //public int GetLevelScore(string chapterName, string levelName)
    //{
    //    string levelKey = ConcatLevelKey(chapterName, levelName);
    //    if (PlayerPrefs.HasKey(levelKey))
    //    {
    //        return PlayerPrefs.GetInt(levelKey);
    //    }
    //    return 0;
    //}


    //public void SaveLevelScore(string chapterName, string LevelName, int score)
    //{
    //    PlayerPrefs.SetInt(ConcatLevelKey(chapterName, LevelName), score);
    //    PlayerPrefs.Save();
    //}

    /// <summary>
    /// 保存游戏相关数据
    /// </summary>
    public static void SaveAllData()
    {
        PlayerPrefs.SetFloat(VolumeKey, GameManager.Instance.Volume);
        PlayerPrefs.SetFloat(ScrollSpeedKey, GameManager.Instance.MouseScrollSpeed);
        PlayerPrefs.Save();
    }


    // ------------------ //   
    // --- 私有方法
    // ------------------ //
    private GamePersistentData() 
    {
		Application.quitting += Application_quitting;
    }

	private void Application_quitting()
	{
        SaveAllData();
    }

    private static string ConcatLevelKey(string chapterName, string levelName)
    {
        return $"_chapter_{chapterName}_level_{levelName}";
    }


	// ------------------ //   
	// --- 类型
	// ------------------ //
}
