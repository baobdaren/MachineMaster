using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class VehicleLevelProgress : LevelProgressBase
{
	// ------------------ //    
	// --- 序列化
	// ------------------ //
	[SerializeField]
	private AbsTargetTrigger PlayerDest;

	// ------------------ //    
	// --- 公有成员    
	// ------------------ //
	public class OneArgEvent : UnityEvent<int> { }
	[HideInInspector]
	public OneArgEvent OnSucceed = new OneArgEvent();
	// ------------------ //   
	// --- 私有成员    
	// ------------------ //

	// ------------------ //    
	// --- Unity消息    
	// ------------------ //
	//protected override void Awake()
	//{
	//	base.Awake();
	//	CurrentEditZone.OnPlayerEnter.AddListener(
	//		() => 
	//		{
	//			Debug.Log("进入 编辑空间");
	//			//CurrentEditZone = CurrentEditZone;
	//			OnEnableEdit?.Invoke(); 
	//		});
	//	CurrentEditZone.OnPlayerExit.AddListener(
	//		() => 
	//		{ 
	//			Debug.Log("退出 编辑控件");
	//			//CurrentEditZone = null;
	//			OnDisableEdit?.Invoke(); 
	//		});
	//}

	// ------------------ //    
	// --- 公有方法   
	// ------------------ //
	public override void SetStartPos(Vector2 playerPos)
	{
		FindObjectOfType<VehiclePlayer>().transform.position = playerPos;

		//GameObject.Instantiate(GameConfig.Instance.ES3Mgr);
		//DestiniationArea.Init(new List<Collider2D>(TaskCargo.GetComponentsInChildren<Collider2D>()), OnTaskSucced);
		//SetActiveCargo(true);
		//MainCameraAction.MoveTo(TaskCargo.transform.position);
		//MainCameraAction.LookTo(MainCameraAction.MaxCameraSize);
	}

	public void OnTaskSucced()
	{
		Debug.Log("关卡条件达成");
		OnSucceed?.Invoke(10000 - Time.frameCount);
	}

	public override bool CanRun(out Vector2[] result)
	{
		// 可能在编辑时车辆移动了
		if (CurrentEditZone == null)
		{
			result = null;
			return false;
		}
		bool has = CurrentEditZone.HasOverBoundPoints(out result);
		return !has;
	}

	// ------------------ //   
	// --- 私有方法
	// ------------------ //
}
