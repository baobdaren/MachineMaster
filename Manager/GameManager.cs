using Steamworks;
using Steamworks.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;


/// <summary>
/// 管理游戏进程，管理游戏数据，跨场景功能
/// </summary>
public class GameManager
{
    public static readonly GameManager Instance = new GameManager();
    // ------------------ //    
    // --- 序列化    
    // ------------------ //

    // ------------------ //    
    // --- 公有成员    
    // ------------------ //
    /// <summary>
    /// 进入关卡场景事件
    /// </summary>
    public UnityEvent<string, string> OnLevelLoaded = new UnityEvent<string, string>();
    public bool IsPlayingLevel { get; private set; }
    public string SelectedChapterName { get; private set; }
    public string SelectedLevelName { get; private set; }
    public bool AutoEnterReplay { get; private set; }

    public GameObject SelectLevelUI { get; private set; }
    public float MouseScrollSpeed { get; set; }
    public bool VerticalHold { get; set; }
    public float Volume 
    {
        get
        {
            return _volume;
        }
        set
        {
            _volume = value;
            AudioListener.volume = _volume;
        }
    }
    //public Vector2 RenterLevelPos { get => LevelProgressBase.Instance. }
    // ------------------ //   
    // --- 私有成员    
    // ------------------ //
    /// <summary>
    /// 音量保存-程序退出时音量始终为1，所以独立保存一下，用于保存设置时使用
    /// </summary>
    private float _volume;

    // ------------------ //    
    // --- Unity消息    
    // ------------------ //

    // ------------------ //    
    // --- 公有方法   
    // ------------------ //
    public GameManager()
    {
        Volume = GamePersistentData.Instance.VolumeData;
        MouseScrollSpeed = GamePersistentData.Instance.ScrollerSpeedData;
        VerticalHold = GamePersistentData.Instance.VerticalHoldData;
        try
        {
            Steamworks.SteamClient.Init(2330880);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Steam 初始化错误：" + ex.Message);
            Application.Quit();
        }
        Debug.Log("steam初始化成功" + Steamworks.SteamClient.IsValid);

        // // Steamworks.SteamUserStats.SetStat("FinishLevel", 1);
        // Achievement achData = new Achievement("Ach_StartGame"); 
        
        
        // achData.Trigger(true);
        
        // // Debug.Log(Steamworks.SteamUserStats.StoreStats());
        // // SteamUserStats.ResetAll(true);
        
        // foreach (Achievement item in SteamUserStats.Achievements)
        // {
        //     Debug.LogError(item.Description);
        // }



        Application.quitting += Application_quitting;
    }

    public void SelectChapter(string chapterName)
    {
        SelectedChapterName = chapterName;
    }

    /// <summary>
    /// 进入关卡场景
    /// </summary>
    public void EnterLevel(string enterLevelName)
    {
        SelectedLevelName = enterLevelName;
        EnterLevelScene(enterLevelName);
        //MoveFirstAssetToActiveScene(SceneManager.GetActiveScene());
        //RecordSelectLevelSceneInfo();
    }

    public void ReplayCurrentLevel(bool saveArchive)
    {
        //PartManager.Instance.re
        On_LevelFinish(saveArchive, true);
    }


	public void On_LevelFinish(bool saveArchive, bool replay)
	{
        IsPlayingLevel = false;
        if (saveArchive)
        {
            ArchiveManager.Instance.SaveLevelArchive();
            // level完成后，恢复关卡选择界面时自动进入这个章节

            AchievementManager.Instance.SetStats("FinishLevels");
        }
        EnterLevelSelectScene(SelectedChapterName, SelectedLevelName, replay);
	}

    /// <summary>
    /// 进入关卡选择界面
    /// </summary>
    /// <param name="openChapterItem">自动选择到该章节</param>
    /// <param name="openLevelItem">记录选择的关卡</param>
    /// <param name="replay">自动进入关卡</param>
	public void EnterLevelSelectScene(string openChapterItem = null, string openLevelItem = null, bool replay = false)
    {
        SelectedChapterName = openChapterItem;
        SelectedLevelName = openLevelItem;
        AutoEnterReplay = replay;
        // 当前场景为开始游戏场景或者关卡场景
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("LevelSelect");
        //GameObject.Instantiate(GameConfig.Instance.ES3Mgr);
        SceneManager.UnloadSceneAsync(currentScene);
    }


    // ------------------ //   
    // --- 私有方法
    // ------------------ //
	private void Application_quitting()
	{
        //Debug.LogError("清空存档");
        //ES3.DeleteDirectory(@"C:\Users\GODBO\AppData\LocalLow\xiaobaostudio\Mechanic Master");
        Steamworks.SteamClient.Shutdown();
	}

    /// <summary>
    /// 创建场景内容
    /// </summary>
    private void EnterLevelScene(string sceneName, Vector2? enterPos = null)
    {
        AchievementManager.Instance.UnlockAchievement(AchievementManager.AchievementTypes.Ach_NewPlayer);

        //Scene selectScene = SceneManager.GetActiveScene();
#if LOAD_ASSETS_MODE_RES
        sceneName = (SelectedChapterName + "/" + sceneName + ".unity");
        Debug.Log(sceneName);
#endif
        string sceneNameLoadTo = SelectedChapterName + "/" + sceneName + ".unity";
		AsyncOperationHandle<SceneInstance>  handlerLoadScene = Addressables.LoadSceneAsync(sceneNameLoadTo);
        handlerLoadScene.Completed += (AsyncOperationHandle<SceneInstance> obj) =>
        {
            obj.Result.ActivateAsync();
            Debug.Log("加载关卡场景完成");
            OnLevelLoaded?.Invoke(SelectedChapterName, SelectedLevelName);
            //if (enterPos.HasValue)
            //{
            //    LevelProgressBase.Instance.SetStartPos(enterPos.Value);
            //}
            //LevelProgressBase.Instance.OnLevelFinish.AddListener((bool saveArchive) => { On_LevelFinish(saveArchive, false); });
        };
        IsPlayingLevel = true;
        return;
		AsyncOperation loadSceneOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        loadSceneOp.completed += (AsyncOperation op) =>
        {
            //GameObject.Instantiate(GameConfig.Instance.LevelUI);
            OnLevelLoaded?.Invoke(SelectedChapterName, SelectedLevelName);
			//if (enterPos.HasValue)
			//{
   //             LevelProgressBase.Instance.SetStartPos(enterPos.Value);
			//}
            //LevelProgressBase.Instance.OnLevelFinish.AddListener((bool saveArchive) => { On_LevelFinish(saveArchive, false); });
        };
    }
}
