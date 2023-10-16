using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 关卡视图的管理
/// </summary>
public class UIManager : MonoSinglton<UIManager>
{
	// ---------- //
	// -- 序列化
	// ---------- //
	[SerializeField]
	private TextMeshProUGUI _mainTitleText;

	[SerializeField]
	private RawImage imgBackGroundBox;

	// ---------- //
	// -- 私有属性
	// ---------- //
	private Dictionary<string, BaseView> ViewDic = new Dictionary<string, BaseView>();
	private Color colorBackGroundBoxDefault;
	private bool _logMessageEnable = false;

	// ---------- //
	// -- 公有属性
	// ---------- //
	public MainViewSimulate ViewPanel_SimulatePhysics { private set; get; }
	public MainViewCreate ViewPanel_PartCreate { private set; get; }
	public MainViewPlayerPartEditor ViewPanel_EditPart { private set; get; }
	public MainViewStart ViewPanel_Starter { private set; get; }
	public MainViewModeling ViewPanel_Modeling { private set; get; }
	public MainViewScenePartEditor ViewPanel_EditScenePart { private set; get; }
	public GlobalViewPartEditor ViewPanel_PartGroup { private set; get; }


	public BaseView PanelDisplaying
	{
		private set; get;
	}
	public BaseView PanelLastOpened
	{
		private set; get;
	}

	/// <summary>
	/// 用于预览场景传感器的相机
	/// </summary>
	private static Camera _renderTexCamera;
	public static Camera RenderTextureCamera
	{
		get
		{
			if (_renderTexCamera == null)
			{
				GameObject obj = new GameObject("Camer_RenderSensorTexture");
				_renderTexCamera = obj.AddComponent<Camera>();
				_renderTexCamera.orthographic = true;
			}
			return _renderTexCamera;
		}
	}

	// ---------------- //
	// -- Unity消息
	// ---------------- //
	private void Awake()
	{
		ViewPanel_SimulatePhysics = Instantiate(GameConfig.Instance.View_Simulate, transform).GetComponentInChildren<MainViewSimulate>(true);
		ViewPanel_PartCreate = Instantiate(GameConfig.Instance.View_Create, transform).GetComponentInChildren<MainViewCreate>(true);
		ViewPanel_EditPart = Instantiate(GameConfig.Instance.View_Edit, transform).GetComponentInChildren<MainViewPlayerPartEditor>(true);
		ViewPanel_Starter = Instantiate(GameConfig.Instance.View_Start, transform).GetComponentInChildren<MainViewStart>(true);
		ViewPanel_Modeling = Instantiate(GameConfig.Instance.View_Modeling, transform).GetComponentInChildren<MainViewModeling>(true);
		ViewPanel_EditScenePart = Instantiate(GameConfig.Instance.View_ScenePartEdit, transform).GetComponentInChildren<MainViewScenePartEditor>(true);
		ViewPanel_PartGroup = Instantiate(GameConfig.Instance.View_PartGroup, transform).GetComponentInChildren<GlobalViewPartEditor>(true);

		//colorBackGroundBoxDefault = imgBackGroundBox.color;

		ViewDic = new Dictionary<string, BaseView>()
		{
			[ViewPanel_Modeling.GetType().ToString()] = ViewPanel_Modeling,
			[ViewPanel_SimulatePhysics.GetType().ToString()] = ViewPanel_SimulatePhysics,
			[ViewPanel_PartCreate.GetType().ToString()] = ViewPanel_PartCreate,
			[ViewPanel_EditPart.GetType().ToString()] = ViewPanel_EditPart,
			[ViewPanel_Starter.GetType().ToString()] = ViewPanel_Starter,
			[ViewPanel_EditScenePart.GetType().ToString()] = ViewPanel_EditScenePart,
			[ViewPanel_PartGroup.GetType().ToString()] = ViewPanel_PartGroup,
		};

		//GameManager.Instance.EnterLevel
		//LevelProgressBase.Instance.OnStartSimulate.AddListener(InitUIs);
		ArchiveManager.Instance.OnArchiveApplied.AddListener(InitUIs);
	}

	private void Update()
	{
		if (Keyboard.current.backquoteKey.wasPressedThisFrame)
		{
			_logMessageEnable = !_logMessageEnable;
		}
	}


	// ---------- //
	// -- 私有方法
	// ---------- //
	private void InitUIs()
	{
		ModelSimulate.ResetData();
		ModelPlayerPartEditor.ResetData();
		ModelModeling.ResetData();
		ModelLevel.ResetData();
		ModelScenePart.ResetData();

		Debug.Log("初始化UI 显示");
		OpenView<MainViewStart>(BaseOpenViewData.Empty);
		StartCoroutine(PrintConsole());
	}
	private IEnumerator PrintConsole()
	{
		while (true)
		{
			StringBuilder sb = new StringBuilder();
			if (this && PlayerPartManager.Instance.AllPlayerPartCtrls != null)
			{
				sb.Clear();
				BasePartCtrl editingPart = ((BasePartCtrl)ModelPlayerPartEditor.Instance.EditingPlayerPartCtrl) ?? (ModelScenePart.Instance.EditingScenePart);
				if (editingPart != null)
				{
					sb.Append($"{Time.frameCount} 正在编辑：{editingPart.MyPartType} Hash={editingPart.GetHashCode()}");
					sb.Append($"{System.Environment.NewLine}当前 零件层级：{editingPart.Layer}");
					if(editingPart.IsPlayerPart) 
					{
						sb.Append($"{System.Environment.NewLine}当前 零件尺寸：{(editingPart as PlayerPartCtrl).Size}");
					}
				}
				else
				{
					sb.Append("未开始编辑");
				}
				if(PartManager.Instance.DragingPart != null) 
				{
					sb.Append($"{System.Environment.NewLine}正在拖拽 {PartManager.Instance.DragingPart.MyPartType}");
				}
				else
				{
					sb.Append($"{System.Environment.NewLine}未开始拖拽");
				}
				yield return null;
				sb.Append($"{System.Environment.NewLine}统计 刚体数量：{PlayerPartManager.Instance.AllColliders.Count}");
				yield return null;
				sb.Append($"{System.Environment.NewLine}统计 零件数量：{PlayerPartManager.Instance.AllPlayerPartCtrls.Count}");
				yield return null;
				sb.Append($"{System.Environment.NewLine}统计 连接数量：{PartConnectionManager.Instance.AllEditConnection.Count}");
				yield return null;
				sb.Append($"{System.Environment.NewLine}统计 场景零件数量：{LevelProgressBase.Instance.AllSceneParts.Count}");
				yield return null;
				sb.Append($"{System.Environment.NewLine}统计 可吸附对象数量：{PartSnapManager.Instance.AllSnapableObjects.Count}");
				yield return null;
				_mainTitleText.text = sb.ToString();
			}
			_mainTitleText.enabled = _logMessageEnable;
			yield return new WaitUntil(() => _logMessageEnable);
			_mainTitleText.enabled = _logMessageEnable;
		}
	}

	//-------------//
	//-- 公有方法
	//-------------//
	/// <summary>
	/// 切换到视图
	/// </summary>
	public void OpenView<T>(BaseOpenViewData datas, bool sleepView = false) where T : BaseView
	{
		var viewSwitchTo = ViewDic[typeof(T).ToString()];
		var exitingView = PanelDisplaying;
		PanelDisplaying = viewSwitchTo;
		foreach (KeyValuePair<string, BaseView> item in ViewDic)
		{
			if(item.Value != PanelDisplaying)
			{
				item.Value.SwitchView(viewSwitchTo, exitingView, sleepView, datas);
			}
		}
		PanelDisplaying.SwitchView(viewSwitchTo, exitingView, sleepView, datas);
	}

	/// <summary>
	/// 隐藏当前的视图
	/// </summary>
	public void Hide(BaseOpenViewData datas)
	{
		if (PanelDisplaying)
		{
			PanelDisplaying.SleepView(datas);
		}
	}

	public void OnSwipe_MoveCamera(Vector2 gs)
	{
		//if (Input.GetMouseButtonDown(2))
		if (Mouse.current.rightButton.isPressed)
		{
			CameraActor.Instance.MainCamera.transform.position += (Vector3)gs * CameraActor.Instance.MainCamera.orthographicSize * -1 / 400;
		}
	}

	public void SetActiveUIInteract(bool interactable)
	{
		GetComponent<GraphicRaycaster>().enabled = interactable;
		imgBackGroundBox.color = interactable ? colorBackGroundBoxDefault : Color.gray;
	}
}
