using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseView : SerializedMonoBehaviour
{
	// ----------------//
	// --- 私有成员
	// ----------------//
	protected List<BaseChildView> AllChildViews
	{
		get
		{
			if (_AllChildViews == null)
			{
				_AllChildViews = new List<BaseChildView>(GetComponentsInChildren<BaseChildView>(true));
			}
			return _AllChildViews;
		}
	}
	private List<BaseChildView> _AllChildViews;

	protected abstract CameraActor.CameraWorkStates? GetCameraWorkStateInThisView { get; }
	// ----------------//
	// --- 公有成员
	// ----------------//
	public DisplayingStates DisplayingState
	{
		get => _isDisplaying;
		private set
		{
			_isDisplaying = value;
			if (!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
			}
			transform.GetChild(0).gameObject.SetActive(_isDisplaying == DisplayingStates.Displaying);
		}
	}
	private DisplayingStates _isDisplaying = DisplayingStates.Sleep;

	public string GetViewName { get => GetType().ToString(); }

	//public static BaseView Instance;

	// ----------------//
	// --- Unity消息
	// ----------------//
	protected virtual void Awake()
	{
		//Instance = this;
		Debug.Log(GetType() + "已初始化关闭");
		if (TryGetComponent<Canvas>(out Canvas canvas))
		{
			canvas.worldCamera = Camera.main;
		}
		DisplayingState = DisplayingStates.Closed;
	}

	protected virtual void Start()
	{ }

	// ----------------//
	// --- 私有方法
	// ----------------//


	// ----------------//
	// --- 公有方法
	// ----------------//
	public bool IsView<T>() where T : BaseView
	{
		return this is T;
	}

	public bool IsView(string typeName)
	{
		return GetType().ToString() == typeName;
	}

	//public string GetViewName { get => GetType().ToString(); }

	public virtual void SwitchView(BaseView openningView, BaseView exitingView, bool sleepView, BaseOpenViewData openData)
	{
		if (openningView != this && DisplayingState != DisplayingStates.Closed) // 关闭这个视图
		{
			if (sleepView && DisplayingState != DisplayingStates.Sleep)
			{
				SleepView(openData);
			}
			else
			{
				ExitView(openData);
			}
			foreach (var item in AllChildViews)
			{
				item.OnParentViewExit();
			}
		}
		else if (openningView == this) // 打开这个视图
		{
			if (GetCameraWorkStateInThisView.HasValue)
			{
				//Debug.LogError("视角模式" + (GetCameraWorkStateInThisView.Value == CameraActor.CameraWorkStates.Follow ? "跟随" : "自由移动"));
				CameraActor.Instance.SetCameraWorkState(GetCameraWorkStateInThisView.Value);
			}
			{
				//Debug.LogError("视角模式不切换");
			}
			if (DisplayingState == DisplayingStates.Sleep)
			{
				AwakeView(openData);
			}
			else
			{
				EnterView(openData);
			}
			foreach (var item in AllChildViews)
			{
				item.OnParentViewEnter();
			}
		}
	}

	//public virtual bool EnterCondition(BaseView baseView)
	//{
	//	return true ;
	//}

	///// <summary>
	///// 退出条件
	///// 返回true则退出视图
	///// 返回false则挂起
	///// </summary>
	///// <param name="willOpenView"></param>
	///// <returns></returns>
	//public virtual bool ExitCondition(BaseView willOpenView)
	//{
	//	return true;
	//}

	protected abstract void OnEnteringView(BaseOpenViewData openData);
	protected abstract void OnExitingView(BaseOpenViewData openData);
	protected virtual void OnAwakeView(BaseOpenViewData openData) { }
	protected virtual void OnSleepView(BaseOpenViewData openData) { }

	public void EnterView(BaseOpenViewData openData) 
	{
		Debug.Log("MVC 进入视图" + GetType());
		DisplayingState = DisplayingStates.Displaying;
		OnEnteringView(openData);
	}

	public void ExitView(BaseOpenViewData openData)
	{
		Debug.Log("MVC 退出视图" + GetType());
		DisplayingState = DisplayingStates.Closed;
		OnExitingView(openData);
	}

	/// <summary>
	/// 打开当前视图时，如果是挂起状态，则自动唤醒
	/// </summary>
	protected virtual void AwakeView(BaseOpenViewData openData)
	{
		DisplayingState = DisplayingStates.Displaying;
		Debug.Log("MVC 唤醒视图" + GetType());
		OnAwakeView(openData);
	}

	/// <summary>
	/// 暂时挂起
	/// </summary>
	public virtual void SleepView(BaseOpenViewData openData)
	{
		Debug.Log("MVC 睡眠视图" + GetType());
		DisplayingState = DisplayingStates.Sleep;
		OnSleepView(openData);
	}

	// ----------------//
	// --- 类型
	// ----------------//
	public enum SwitchStates
	{
		Exit, Enter
	}

	public enum MainViewTypes
	{
		Start, Create, Edit, Modeling, Simulate
	}

	public enum DisplayingStates
	{ 
		Displaying, Sleep, Closed
	}
}