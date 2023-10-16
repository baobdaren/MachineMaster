using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Events;

/// <summary>
/// 关卡流程的基类
/// </summary>
public abstract class LevelProgressBase : SerializedMonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public static LevelProgressBase Instance
	{
		get 
		{
			if (_instance == null) _instance = FindObjectOfType<LevelProgressBase>();
			return _instance;
		}
	}
	private static LevelProgressBase _instance;
	//public UnityEvent<bool> OnLevelFinish = new UnityEvent<bool>();
	public UnityEvent OnStartSimulate = new UnityEvent();
	/// <summary>
	/// 当可以设计时，子类择机调用
	/// </summary>
	//public UnityEvent OnEnableEdit = new UnityEvent();

	/// <summary>
	/// 当不再可以设计时，字类择机调用
	/// </summary>
	//public UnityEvent OnDisableEdit = new UnityEvent();

	public EditZone CurrentEditZone { protected set; get; }
	public List<Collider2D> CollidersLevelEditZoneOverlaped { get; } = new List<Collider2D>();

	// ----------------//
	// --- 私有成员
	// ----------------//
	public Dictionary<Hash128, ScenePart> AllSceneParts
	{
		get
		{
			if (_allScenePartList == null)
			{
				var allParts = new List<ScenePart>(GameObject.FindObjectsOfType<ScenePart>(true));
				_allScenePartList = new Dictionary<Hash128, ScenePart>(allParts.Count);
				foreach (var item in allParts)
				{
					_allScenePartList.Add(item.GetPartHashID, item);
				}
			}
			return _allScenePartList;
		}
	}
	private Dictionary<Hash128, ScenePart> _allScenePartList;

	// ----------------//
	// --- Unity消息
	// ----------------//
	protected virtual void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		var foundZones = FindObjectsOfType<EditZone>();
		Debug.Assert(foundZones.Length >= 1, "至少需要一个编辑区域");
		CurrentEditZone = foundZones[0];
		Physics2D.OverlapCollider(CurrentEditZone.EditArea, new ContactFilter2D() { useTriggers = false }, CollidersLevelEditZoneOverlaped);
	}

	protected virtual void Start()
	{
	}

	// ----------------//
	// --- 公有方法
	// ----------------//
	public abstract void SetStartPos(Vector2 playerPos);

	public ScenePart GetScenepart(Hash128 id)
	{
		if(AllSceneParts.TryGetValue(id, out ScenePart part))
		{ 
			return part; 
		}
		else
		{
			Debug.Assert(false, $"没有找到存档中场景零件的匹配场景零件 {id.ToString()}");
			return null;
		}
	}

	public void StartPlay() 
	{
		OnStartSimulate?.Invoke();
	}



	/// <summary>
	/// 返回不符合运行条件的零件
	/// </summary>
	/// <returns></returns>
	//public abstract bool CanRun(out List<PartCtrl> result, out string msg, out Vector2? pos);
	//public abstract bool CanRun(out string msg, out Vector2? pos);
	public abstract bool CanRun(out Vector2[] result);
	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//
}
