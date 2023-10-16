using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GameConfig", menuName = "创建数据文件/创建GameConfig数据", order = 11)]
public class GameConfig : UniqueConfigBase
{
	// ------------------ //
	// --- 序列化
	// ------------------ // 
	public float OverlappingDetectionDistance => Physics2D.defaultContactOffset * -1.1f;

	// 为何这种赋值方式会导致ScriptableObject的一个错误 ???
	//public float OverlappingDetectionDistance = Physics2D.defaultContactOffset * -1.1f;
	[Header("章节/关卡加载配置")]
	[Multiline()]
	public string ChapterBundlesFolderPath;
	[Multiline()]
	public string ChapterTexsFolderName;
	[Multiline()]
	public string StandardTransportLevelCargoName;

	[Header("关卡场景的UI")]
	public GameObject LevelUI;

	[Header("建模符号设置视图预设")]
	public GameObject SymbolSettingCanvas;

	[Header("关卡场景UI MVC 的视图")]
	public GameObject View_Create;
	public GameObject View_Edit;
	public GameObject View_Start;
	public GameObject View_Modeling;
	public GameObject View_Simulate;
	public GameObject View_ScenePartEdit;
	public GameObject View_PartGroup;

	[Header("零件连接光标")]
	public GameObject ConnectCursor;
	[Header("吸附相关")]
	public GameObject SnapLineCursor;
	[Header("零件旋转光圈")]
	public GameObject PartRoateCircle;


	[Header("ES3 管理器")]
	public GameObject ES3Mgr;

	[Header("建模内容")]
	public GameObject SymbolPrefab;

	[Header("建模图父物体开始位置")]
	public Vector2 ModelingMapStartPosition;

	[Header("建模背景预制")]
	public GameObject ModelingBackGround;

	[Header("玩家载具设定")]
	public float TorqueWheel;
	public float SpeedWheel;
	// ------------------ //
	// --- 公有成员
	// ------------------ //
	public static GameConfig Instance { private set; get; }




	// ------------------ //
	// --- 私有成员
	// ------------------ //


	// ------------------ //
	// --- Unity消息
	// ------------------ //

	//private void OnValidate()
	//{
	//	Debug.Log($"配置文件 {typeof(GameConfig)} 刷新");
	//}

	//private void OnDestroy()
	//{
	//	Debug.LogError($"{typeof(GameConfig)} {this.GetInstanceID()} 删除");
	//}
	// ------------------ //
	// --- 公有方法
	// ------------------ //
	public override void InitInstance()
	{
		Instance = this;
	}


	// ------------------ //
	// --- 类型
	// ------------------ //
}
