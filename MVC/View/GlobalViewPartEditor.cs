using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary> 
/// 通过零件层级，来控制显示
/// </summary>
public class GlobalViewPartEditor : GlobalView
{
	// ------------- //
	// -- 序列化
	// ------------- //
	[ChildGameObjectsOnly]
	[SerializeField]
	private Toggle[] _toggleList;
	[SerializeField]
	private TextMeshProUGUI _textCurrentLayer;

	// ------------- //
	// --  私有属性
	// ------------- //
	private ControllerPartGroup Ctrl => ControllerPartGroup.Instance;
	private ModelPartEditor Model => ModelPartEditor.Instance;

	protected override List<string> FollowViewNames { get; } = new List<string>() { nameof(MainViewCreate), nameof(MainViewPlayerPartEditor), nameof(MainViewScenePartEditor) };

	// 场景零件是否需要修改尺寸 颜色等,决定是否独立一个视图来编辑场景零件
	protected override CameraActor.CameraWorkStates? GetCameraWorkStateInThisView => null;

	public int CurrentLayer
	{
		get => _layer;
		set
		{
			int newValue = value % 3;// 0 1 2
			bool changed = _layer != newValue;
			_layer = newValue;
			_textCurrentLayer.text = _layer.ToString();
			if (changed) Ctrl.OnSelectLayer?.Invoke(_layer);
		}
	}
	private int _layer;
	// ------------- //
	// --  公有属性
	// ------------- //
	public static GlobalViewPartEditor Instance 
	{
		get
		{
			if(_instance == null) _instance = FindObjectOfType<GlobalViewPartEditor>();
			return _instance;
		}
	}
	private static GlobalViewPartEditor _instance;
	// ------------- //
	// --  Unity消息
	// ------------- //
	protected override void Awake()
	{
		base.Awake();

		Debug.Assert(_toggleList.Length == 3);
		Debug.Assert(_toggleList[0].group != null);
		Debug.Assert(_toggleList[0].group == _toggleList[1].group);
		Debug.Assert(_toggleList[1].group == _toggleList[2].group);
		Debug.Assert(_toggleList[2].group == _toggleList[1].group);
		//for (int i = 0; i < 3; i++)
		//{
		//	GameObject clone = GameObject.Instantiate(PartGroupItem).gameObject;
		//	PartLayerGroupItem cloneItemGroup = clone.GetComponent<PartLayerGroupItem>();
		//}
		_toggleList[0].group.allowSwitchOff = true;
		_toggleList[0].onValueChanged.AddListener((bool on) => { if (on) CurrentLayer = 0; });
		_toggleList[1].onValueChanged.AddListener((bool on) => { if (on) CurrentLayer = 1; });
		_toggleList[2].onValueChanged.AddListener((bool on) => { if (on) CurrentLayer = 2; });

		Ctrl.OnSelectLayer.AddListener(OnLayerChanged);
		//_setLayerButton.onClick.AddListener(OnClicked_SetLayer);
	}

	protected override void Start()
	{
		base.Start();

		_toggleList[0].SetIsOnWithoutNotify(false);
		_toggleList[1].SetIsOnWithoutNotify(false);
		_toggleList[2].SetIsOnWithoutNotify(false);
		_toggleList[0].SetIsOnWithoutNotify(true);
		_toggleList[0].group.allowSwitchOff = false;
	}

	private void Update()
	{
		if (Keyboard.current.tabKey.wasPressedThisFrame)
		{
			int nextLayer = (CurrentLayer + 1) % 3;
			_toggleList[nextLayer].isOn = true;
		}
	}

	//void Start()
	//   {
	// //      toggleOrigin.SetActive(true);
	// //      toggleOrigin.GetComponent<Toggle>().isOn = true;
	//	//while (toggleOrigin.transform.parent.childCount < GameLayerManager.PartLayerCount)
	//	//{
	// //          GameObject.Instantiate(toggleOrigin, toggleOrigin.transform.parent);
	// //      }
	// //      Debug.LogError("Start 层级过滤");
	// //      for (int i = 0; i <  GameLayerManager.PartLayerCount; i++)
	// //      {
	// //          int layerIndex = i + GameLayerManager.DefaultPartLayer;
	// //          GameObject toggle = toggleOrigin.transform.parent.GetChild(i).gameObject;
	// //          //toggle.transform.Find("Background").GetComponent<Image>().color = GameLayerManager.Instance.GetPartColor(layerIndex);
	// //          toggle.GetComponent<Toggle>().onValueChanged.AddListener(new UnityEngine.Events.UnityAction<bool>((bool isOn) => 
	// //          { 
	// //              OnToggleValueChanged_Layer(isOn, layerIndex); 
	// //          }));
	// //          // toggleGroup.RegisterToggle(toggleObject.GetComponent<Toggle>());
	// //          _toggls.Add(toggle.GetComponent<Toggle>());
	// //      }
	//   }


	// ------------- //
	// -- 私有方法
	// ------------- //
	//private void OnClicked_SetLayer()
	//{
	//	if (EditingPart != null) EditingPart.Layer = _layer;
	//	CurrentLayer = CurrentLayer;
	//}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="addToLayerIndex"></param>
	private void OnLayerChanged(int selectedLayer)
	{
		Ctrl.SetEditingPartLayer(selectedLayer);
		//SetPartMaterial(selectedLayer);
		PartMaterialSetter.Instance.UpdatePartMaterials();
		//UIManager.Instance.ViewPanel_EditPart.ConnectView.ForceUpdate_ConnectCursor();
	}
	//private void OnToggleValueChanged_Layer(bool isOn, int selectNum)
	//   {
	//	foreach (PlayerPartCtrl item in PlayerPartManager.Instance.AllPlayerPartCtrls)
	//       {
	//		if (item.Layer == selectNum)
	//		{
	//               item.MyEditPartAccesstor.gameObject.SetActive(isOn);
	//		}
	//       }
	//	// 层级修改后刷新连接状态
	//	if (UIManager.Instance.ViewPanel_EditPart.IsDisplaying)
	//	{
	//           UIManager.Instance.ViewPanel_EditPart.ConnectView.ForceUpdate_ConnectCursor();
	//	}
	//   }

	/// <summary>
	/// 更新所有零件的材质
	/// </summary>
	/// <returns></returns>
	private IEnumerator ProgressUpdateConflictPartMaterials()
	{
		List<(BasePartCtrl, BasePartCtrl)> conflictList = new List<(BasePartCtrl, BasePartCtrl)>();
		while (true)
		{
			bool draging = PartManager.Instance.IsDraging;
			// 暂时拖拽时冲突检测只针对拖拽1个零件的情况
			//if(PartManager.Instance.FollowingParts.Count != 1) { yield return null; continue; }
			conflictList = PartManager.Instance.GetConflictData(PartManager.Instance.DragingPart);
			// 拖拽中 显示与拖拽零件冲突的所有零件
			if (draging)
			{
				ClearAllPartTexError();
				//Debug.LogError($"正在拖拽 {conflictList.Count}组冲突零件");
				foreach ((BasePartCtrl, BasePartCtrl) itemConflictPartPair in conflictList)
				{
					PartMaterialSetter.Instance.SetMaterial_ErrorTex(itemConflictPartPair.Item2, true);
					PartMaterialSetter.Instance.SetMaterial_ErrorTex(itemConflictPartPair.Item1, true);
				}
				yield return new WaitForSecondsRealtime(0.1f);
			}
			else // 未拖拽 成对间隔显示冲突零件
			{
				//Debug.LogError($"没有拖拽 {conflictList.Count}组零件");
				foreach ((BasePartCtrl, BasePartCtrl) itemConflictPartPair in conflictList)
				{
					//PartMaterialSettter.Instance.SetMaterial_ErrorTex(PartManager.Instance.DragingPart, true);
					PartMaterialSetter.Instance.SetMaterial_ErrorTex(itemConflictPartPair.Item1, true);
					PartMaterialSetter.Instance.SetMaterial_ErrorTex(itemConflictPartPair.Item2, true);
					float timeStart = Time.realtimeSinceStartup;
					float waitTime = Mathf.Clamp(3f / conflictList.Count, 0.5f, 1f);
					yield return new WaitUntil(() => { return Time.realtimeSinceStartup - timeStart > waitTime || PartManager.Instance.IsDraging == true; });
					if (PartManager.Instance.IsDraging) break;
					ClearAllPartTexError();
					yield return new WaitForSecondsRealtime(Mathf.Clamp(3f / conflictList.Count, 0.1f, 0.5f));
				}
				ClearAllPartTexError();
			}
			yield return 0;
		}
		Debug.LogError("退出刷新冲突材质");
	}

	//private void SetPartMaterial(int highlightLayer)
	//{
	//	//if (highlightLayer == 3) // 3表示关闭纯色显示方式
	//	//{
	//	//	for (int i = 0; i < PartManager.Instance.AllPartCount; i++)
	//	//	{
	//	//		BasePartCtrl part = PartManager.Instance.GetPart(i);
	//	//		PartMaterialSetter.Instance.SetMaterial_Layer_Clear(part);
	//	//	}
	//	//}
	//	//else
	//	//{
	//	//	for (int i = 0; i < PartManager.Instance.AllPartCount; i++)
	//	//	{
	//	//		BasePartCtrl part = PartManager.Instance.GetPart(i);
	//	//		int layer = (part is ScenePartCtrl) ? (part as ScenePartCtrl).PartLayer : (part as PlayerPartCtrl).Layer;
	//	//		PartMaterialSetter.Instance.SetMaterial_PureColor(part, layer);
	//	//		//PartMaterialSettter.Instance.SetMaterial_PureColor(part, layer, ModelEdit.Instance.EditingPlayerPartCtrl == part);
	//	//	}
	//	//}
	//}

	private void ClearAllPartTexError()
	{
		for (int i = 0; i < PartManager.Instance.AllPartCount; i++)
		{
			PartMaterialSetter.Instance.SetMaterial_ErrorTex(PartManager.Instance.GetPart(i), false);
		}
	}

	protected override void OnEnteringView(BaseOpenViewData datas)
	{
		Model.IsEditing = true;
		//SetPartMaterial(CurrentLayer);
		CameraActor.Instance.SetCameraDisplay(CameraActor.CameraCullingTypes.ToEditPart);
		// 从建模界面返回时
		//if (ModelEdit.Instance.IsEditing)
		//{
		//EditingPart = (BasePartCtrl)ModelEdit.Instance.EditingPlayerPartCtrl ?? (BasePartCtrl)ModelScenePart.Instance.EditingScenePart;
		//}
		StartCoroutine(ProgressUpdateConflictPartMaterials());
		PartMaterialSetter.Instance.UpdatePartMaterials();
	}

	protected override void AwakeView(BaseOpenViewData datas)
	{
		base.AwakeView(datas);
	}

	protected override void OnExitingView(BaseOpenViewData datas)
	{
		Model.IsEditing = false;
		// 退出时恢复???
		StopAllCoroutines(); // 暂停错误材质设置携程，再清空错误材质设置
		//Debug.LogError("恢复错误纹理材质设置");
		//SetPartMaterial(3);
		CameraActor.Instance.SetCameraDisplay(CameraActor.CameraCullingTypes.All);
		PartConnectionManager.Instance.UpdateEditBearings(null);
	}
	// ------------- //
	// -- 公有方法
	// ------------- //
}
