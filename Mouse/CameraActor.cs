using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraActor : MonoBehaviour
{
	[SerializeField]
	private CinemachineVirtualCamera _mainVirualCamera;

	[SerializeField]
	private CinemachineConfiner2D _mainVirtualCameraConfiner;

	[SerializeField]
	private Light2D editingLight;
	//[SerializeField]
	//private Camera _secondCamera;
	// ----------------//
	// --- 私有成员
	// ----------------//
	private static DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> _changedOperation;
	// 透视方式下使用z轴
	//private readonly float[] _editSize = new float[] { 40, 160 };
	//private readonly float[] _modelSize = new float[] { 10, 25 };
	// 正交方式下使用fileView大小
	public static readonly float[] OtherSizeLimit = new float[] { 1, 7 };

	public int GetNum { get { return _num; } }
	private int _num;

	private RectTransform CanvasRect
	{
		get
		{
			if (_canvasRect == null)
			{
				_canvasRect = UIManager.Instance.GetComponentInChildren<RectTransform>(true);
			}
			return _canvasRect;
		}
	}
	private RectTransform _canvasRect;
	private CinemachineVirtualCamera MainVirtualCamera { get => _mainVirualCamera; }
	private VirtualCameraZone _currenVirtualCameraZone;
	// ----------------//
	// --- 公有成员
	// ----------------//
	public static CameraActor Instance { private set; get; }
	public static event Action<float> OnCameraViewSizeChanged;
	public Transform CurrentFollowTarget
	{
		get 
		{
			return CurrentVirtualCamera.Follow;
		}
		set
		{
			//if (value == null)
			//{
			//	MainVirtualCamera.Follow = PlayerManager.Instance.Player.transform;
			//	CurrentVirtualCamera.Follow = MainVirtualCamera.Follow; // 目标回归到主角载具上时,可能在距依然再非主虚拟相机范围内
			//	CurrentVirtualCamera.enabled = true; // 上一个目标如果不再该虚拟相机范围内,则该相机会被disable,这里重新激活
			//}
			//else
			//{
			MainVirtualCamera.Follow = value.transform;
			CurrentVirtualCamera.Follow = value.transform;
			if (CurrentVirtualCameraZone)
			{
				CurrentVirtualCameraZone.enabled = false;
				CurrentVirtualCameraZone.enabled = true;
			}
			//}
		}
	}
	public CameraWorkStates CurrentWorkingStatue
	{
		get => MainVirtualCamera.Follow != null ? CameraWorkStates.Follow : CameraWorkStates.FreeMove;
	}
	public CinemachineVirtualCamera CurrentVirtualCamera
	{
		get 
		{
			if (CurrentVirtualCameraZone == null)
			{
				return MainVirtualCamera;
			}
			return CurrentVirtualCameraZone.VirtualCamera;
		}
		//set
		//{
		//	//if (_ == value)
		//	//{
		//	//	return;
		//	//}
		//	//value.VirtualCamera.Follow = _currentVirtualCamera.VirtualCamera.Follow;
		//	Transform following = CurrentVirtualCamera.Follow;
		//	_currenVirtualZone = value;
		//	_currentVirtualCamera.Follow = following;
		//	//_mainVirualCamera.enabled = (_newVirtualCamera == null);
		//}
	}
	public VirtualCameraZone CurrentVirtualCameraZone
	{
		get => _currenVirtualCameraZone;
		set
		{
			value.VirtualCamera.Follow = CurrentFollowTarget;
			_currenVirtualCameraZone = value;
		}
	}
	public Camera MainCamera { get; private set; }

	public float CameraViewSize
	{
		// 透视
		get => CameraActor.Instance.MainCamera.orthographicSize;
		set => CameraActor.Instance.MainCamera.orthographicSize = value;
		// 正交
		//get => CameraActor.Instance.MainCamera.fieldOfView;
		//set => CameraActor.Instance.MainCamera.fieldOfView =value;
	}
	public float MinCameraViewSize { get => OtherSizeLimit[0]; }
	public float MaxCameraViewSize { get => OtherSizeLimit[1]; }
	public const float Duration = 0.5f;
	public Vector2? mouseDownPos = null;

	/// <summary>
	/// 记录当前帧鼠标点击的位置
	/// </summary>
	private int _mousePosUpdateTime = -1;
	private Vector3? _curFramMouseWorldPos;
	public Vector3 MouseWorldPos
	{
		get
		{
			if (_mousePosUpdateTime != Time.frameCount || !_curFramMouseWorldPos.HasValue)
			{
				_mousePosUpdateTime = Time.frameCount;
				_curFramMouseWorldPos = ScreenTo2DWorldPosition(Mouse.current.position.ReadValue());
			}
			return _curFramMouseWorldPos.Value;
		}
	}

	private bool? _mouseOverUI;
	private int _mouseOverUIUpdateTime = -1;
	public bool MouseOverUI
	{
		get
		{
			if (_mouseOverUIUpdateTime != Time.frameCount)
			{
				_mouseOverUIUpdateTime -= Time.frameCount;
				_mouseOverUI = EventSystem.current.IsPointerOverGameObject();
			}
			return _mouseOverUI.Value;
		}
	}

	public CinemachineBrain MainCameraCinemachinBrain
	{
		get
		{
			if (_cinemachineBrain == null)
			{
				_cinemachineBrain = GetComponent<CinemachineBrain>();
			}
			return _cinemachineBrain;
		}
	}
	private CinemachineBrain _cinemachineBrain;

	public PlayableDirector MainCameraDirector
	{
		get
		{
			if (_mainCameraDirector == null)
			{
				_mainCameraDirector = GetComponent<PlayableDirector>();
			}
			return _mainCameraDirector;
		}
	}
	private PlayableDirector _mainCameraDirector;

	public CinemachineConfiner2D mainVirtualCameraConfiner { get => _mainVirtualCameraConfiner; }
	// ----------------//
	// --- Unity 消息
	// ----------------//
	private void Awake()
	{
		Instance = this;
		MainCamera = GetComponentInChildren<Camera>();
		CurrentFollowTarget = PlayerManager.Instance.Player.transform;
		editingLight.gameObject.SetActive(false);
		ResetCamera();
		Debug.Assert(CurrentFollowTarget, "Awake时应该存在跟踪对象");
	}

	void LateUpdate()
	{
		//改为移动虚拟相机，而不是相机
		UpdateCamera();
	}


	// ----------------//
	// --- 公有方法
	// ----------------//
	public void SetMoveTo(Vector2 targetPos, float dur = Duration, Action completeCallBack = null)
	{
		CameraWorkStates lastWorkingType = CurrentWorkingStatue;
		//// 暂时锁定相机控制
		//CurrentWorkingStatue = CameraMoveTypes.FreeMove;
		var op = MainVirtualCamera.transform.DOMove(new Vector3(targetPos.x, targetPos.y, MainVirtualCamera.transform.position.z), dur);
		// 移动完成后恢复设置工作模式
		op.onComplete = new TweenCallback(() =>
		{
			SetWorkingModel(lastWorkingType);
			completeCallBack?.Invoke();
		});
	}

	public void SetFocusTo(float to, float dur = Duration)
	{
		_changedOperation = DOTween.To(
			() => (float)CameraActor.Instance.CameraViewSize,
			(float v) => CameraActor.Instance.CameraViewSize = v,
			Mathf.Clamp(to, MinCameraViewSize, MaxCameraViewSize),
			dur
		);
	}

	public void SetCameraWorkState(CameraWorkStates switchTo)
	{
		switch (switchTo)
		{
			case CameraWorkStates.FreeMove:
				//mainVirtualCameraConfiner.m_BoundingShape2D = LevelProgressBase.Instance.CurrentEditZone.EditArea.PolygonTrigger;
				MainVirtualCamera.Priority = int.MaxValue;
				MainVirtualCamera.Follow = null;
				break;
			case CameraWorkStates.Follow:
				MainVirtualCamera.Priority = 0;
				MainVirtualCamera.Follow = PlayerManager.Instance.Player.transform;
				//mainVirtualCameraConfiner.m_BoundingShape2D = MainVirualCameraCaheData.confinerCache;
				break;
		}
	}

	/// <summary>
	/// 设置遮罩,只显示某些层级
	/// </summary>
	/// <param name="onlyPart"></param>
	public void SetCameraDisplay(CameraCullingTypes onlyPart)
	{
		SetMainCameraCullingMask(onlyPart);
		//editingLight.gameObject.SetActive(onlyPart == CameraCullingTypes.ToEditPart);
	}

	public void ResetCamera()
	{
		//CameraActor.Instance.CurrentWorkingStatue = CameraMoveTypes.FreeMove;
		CameraActor.Instance.MainCamera.transform.position = new Vector3(0, 0, -10);
		CameraActor.Instance.MainCamera.orthographic = true;
		CameraViewSize = MinCameraViewSize;
		//CameraActor.Instance.MainCamera.orthographicSize = MaxCameraSize;
	}

	/// <summary>
	/// 将屏幕坐标转换为世界坐标中z轴为0的平面上
	/// </summary>
	/// <param name="mousePos"></param>
	/// <returns></returns>
	public static Vector3 ScreenTo2DWorldPosition(Vector2 mousePos)
	{
		float distance = Mathf.Abs(CameraActor.Instance.MainCamera.transform.position.z);
		return CameraActor.Instance.MainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distance));
	}

	public bool MouseClicked(MouseBtn mouseBtn, bool falseWhenOverUI = true)
	{
		if (falseWhenOverUI && MouseOverUI)
		{
			return false;
		}
		return MouseBtnClicked(mouseBtn);
	}

	public bool MouseClickedWolrdPos(CameraActor.MouseBtn btn, out Vector2 pos,  bool ignoreWhenClickUI )
	{
		if (ignoreWhenClickUI && MouseOverUI)
		{
			pos = Vector2.zero;
			return false;
		}
		pos = ScreenTo2DWorldPosition(Mouse.current.position.ReadValue());
		return true;
	}

	public Vector2 WorldToScreenPos(Vector3 worldPos)
	{
		Vector3 screenPos = CameraActor.Instance.MainCamera.WorldToScreenPoint(worldPos);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, screenPos, CameraActor.Instance.MainCamera, out Vector2 loaclPoint);
		return loaclPoint;
	}


	// ----------------//
	// --- 私有方法
	// ----------------//
	private void SetWorkingModel(CameraWorkStates workingType)
	{
		if (workingType == CameraWorkStates.FreeMove)
		{
			SetCameraSize(true);
		}
	}
	private void UpdateCamera()
	{
		if (CurrentWorkingStatue == CameraWorkStates.FreeMove)
		{
			UpdateFreeMove();
		}
		UpdateFreeScale();
		//switch (CurrentWorkingStatue)
		//{
		//	case CameraWorkingStates.FreeMove: // 编辑状态下自由移动相机
		//								   //UpdateCameraSize(); // 做好不改变尺寸
		//								   // 边缘移动相机
		//		UpdateCameraPosition();
		//		break;
		//	case CameraWorkingStates.Follow:
		//		if (MainVirtualCamera.Follow == null)
		//		{
		//			MainVirtualCamera.Follow = GameObject.FindGameObjectWithTag("Player").transform;
		//		}
		//		break;
		//}
	}

	/// <summary>
	/// 鼠标中键滚动缩放相机
	/// </summary>
	/// <param name="force"></param>
	private void SetCameraSize(bool force = false)
	{
		// 除以120后分为1，2，3档位（速度渐快）
		// 负号变换：前推为拉近
		float wheel = Mouse.current.scroll.ReadValue().y / -120;
		bool anykeyDown = Keyboard.current.anyKey.isPressed;
		if (!anykeyDown && (wheel != 0 || force))
		{
			//wheel *= 1 + (FirstAsset.Instance.CameraAction.CameraViewSize - MinCameraViewSize) / (MaxCameraViewSize - MinCameraViewSize);
			// 滚轮滚动10次为一次完整的缩放
			SetFocusTo(CameraActor.Instance.CameraViewSize + wheel * ((MaxCameraViewSize - MinCameraViewSize) / 9), 0.2f);
		}
	}
	private void UpdateFreeScale()
	{
		float middleBtnScroller = -Mouse.current.scroll.ReadValue().y;
		if (middleBtnScroller != 0)
		{ 
			float scale = (OtherSizeLimit[1] - MainVirtualCamera.m_Lens.OrthographicSize) * 0.1f;
			scale = Mathf.Clamp(scale, (OtherSizeLimit[1] - OtherSizeLimit[0]) * 0.05f, (OtherSizeLimit[1] - OtherSizeLimit[0]) * 0.1f) * (middleBtnScroller > 0 ? 1 : -1);
			float target = Mathf.Clamp(MainVirtualCamera.m_Lens.OrthographicSize + scale, OtherSizeLimit[0], OtherSizeLimit[1]);
			//MainVirtualCamera.m_Lens.OrthographicSize.
			DOTween.To(
				new DG.Tweening.Core.DOGetter<float> ( () => { return MainVirtualCamera.m_Lens.OrthographicSize; }),
				new DG.Tweening.Core.DOSetter<float> ((float value) => { MainVirtualCamera.m_Lens.OrthographicSize = value; }),
				target,
				0.2f
				);
			//_secondCamera.orthographicSize = MainVirtualCamera.m_Lens.OrthographicSize;
		}
	}

	private void UpdateFreeMove()
	{
		#region 靠边移动相机方案
		//Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
		//int dectedRange = 4;
		//Vector2 moveDir = Vector2.zero;
		//if (mouseScreenPos.x < dectedRange)
		//{
		//	moveDir += Vector2.left;
		//}
		//else if (mouseScreenPos.x > Screen.width - dectedRange)
		//{
		//	moveDir += Vector2.right;
		//}

		//if (mouseScreenPos.y < dectedRange)
		//{
		//	moveDir += Vector2.up;
		//}
		//else if (mouseScreenPos.y > Screen.height - dectedRange)
		//{
		//	moveDir += Vector2.down;
		//}
		//if (moveDir == Vector2.zero) return;
		//float ratio = CameraActor.Instance.MainCamera.orthographicSize * GameManager.Instance.MouseScrollSpeed * Time.deltaTime;
		//CameraActor.Instance.MainCamera.transform.position += (Vector3)moveDir * ratio;
		#endregion

		#region 鼠标右键移动相机方案
		if (Mouse.current == null)
		{
			return;
		}

		if (Mouse.current.rightButton.wasPressedThisFrame)
		{
			mouseDownPos = Mouse.current.position.ReadValue();
		}
		else if (Mouse.current.rightButton.wasReleasedThisFrame)
		{
			mouseDownPos = Vector2.down;
		}
		if (mouseDownPos != Vector2.down && Mouse.current.rightButton.isPressed)
		{
			float ratio = GameManager.Instance.MouseScrollSpeed * (Time.deltaTime != 0 ? Time.deltaTime : 0.0163f);
			Vector3 moveDir = Mouse.current.position.ReadValue() - mouseDownPos.Value;
			MainVirtualCamera.transform.position += moveDir * ratio;
			// 暂时不考虑范围限制
			//if (!mainVirtualCameraConfiner.m_BoundingShape2D.OverlapPoint(MainVirtualCamera.transform.position))
			//{
			//	MainVirtualCamera.transform.position = mainVirtualCameraConfiner.m_BoundingShape2D.ClosestPoint(MainVirtualCamera.transform.position);
			//	MainVirtualCamera.transform.position += new Vector3(0, 0, -10);
			//}
		}

		float middleBtnOffset = Mouse.current.middleButton.ReadValue();
		if (middleBtnOffset != 0)
		{
			MainVirtualCamera.m_Lens.OrthographicSize -= middleBtnOffset;
		}
		#endregion

		#region 鼠标中键拖拽移动 

		#endregion
	}

	private bool MouseBtnClicked(MouseBtn mouseBtn)
	{
		if (mouseBtn == MouseBtn.Right && Mouse.current.rightButton.wasPressedThisFrame)
		{
			return true;
		}
		if (mouseBtn == MouseBtn.Middle && Mouse.current.middleButton.wasPressedThisFrame)
		{
			return true;
		}
		if (mouseBtn == MouseBtn.Left && Mouse.current.leftButton.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}


	private int GetLayerBitByName(string layerName)
	{
		int result = LayerMask.NameToLayer(layerName);
		Debug.Assert(result != -1);
		return 1 << result;
	}

	private void SetMainCameraCullingMask(CameraCullingTypes displayToEditpart)
	{
		if (displayToEditpart == CameraCullingTypes.ToEditPart)
		{
			int Layers = GetLayerBitByName("EditPart");
			Layers |= GetLayerBitByName("ScenePart");
			Layers |= GetLayerBitByName("UI");
			Layers |= GetLayerBitByName("Cursor");
			Layers |= GetLayerBitByName("Camera");
			Layers |= GetLayerBitByName("Bearing");
			MainCamera.cullingMask = Layers;
		}
		else
		{
			MainCamera.cullingMask = -1;
		}
	}
	//----------------//
	// --- 类型
	// ----------------//
	public enum CameraCullingTypes
	{
		ToEditPart, All
	};

	public enum CameraWorkStates
	{
		FreeMove, Follow
	}

	public enum MouseBtn
	{
		Left, Middle, Right
	}
}
