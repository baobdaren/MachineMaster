using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// 运行关卡的界面
/// 主要负责运行零件
/// </summary>
public class MainViewSimulate : BaseView
{
	// ---------- //
	// --- 序列化
	// ---------- //
	//[SerializeField]
	//Button buttonBack;
	[SerializeField]
	Button buttonSimulate;
	[SerializeField]
	Button buttonReplay;
	[SerializeField]
	Slider sliderSimulateCreateProgress;
	// ---------- //
	// --- 私有属性
	// ---------- //
	public ControllerSimulate Ctrl => ControllerSimulate.Instance;
	private readonly List<GameObject> _errorLineList = new List<GameObject>();
	private Transform _conflictLineParent
	{
		get
		{
			if (_conflictParentGameObject == null)
			{
				_conflictParentGameObject = new GameObject("Parent Of Conflict Line").transform;
			}
			return _conflictParentGameObject;
		}
	}
	private Transform _conflictParentGameObject;
	protected override CameraActor.CameraWorkStates? GetCameraWorkStateInThisView => CameraActor.CameraWorkStates.Follow;

	// ---------- //
	// --- 公有属性
	// ---------- //
	public static bool IsRunInPhysicsModel
	{
		get => ParentsManager.Instance.ParentOfPhysicsParts && ParentsManager.Instance.ParentOfPhysicsParts.activeSelf;
	}
	public int SimulateCreateProgress
	{
		set
		{
			if (value < 0 || value > 100) { sliderSimulateCreateProgress.gameObject.SetActive(false); return; }
			sliderSimulateCreateProgress.gameObject.SetActive(true);
			sliderSimulateCreateProgress.value = value;
		}
	}


	// ---------- //
	// --- Unity 消息
	// ---------- //

	protected override void Awake()
	{
		base.Awake();
		//buttonSimulate.onClick.AddListener(() => { StartCoroutine(OnClicked_Simulate()); });
		buttonReplay.onClick.AddListener(OnClicked_ReplayLevel);
		//buttonBack.onClick.AddListener(OnClicked_BackToStartView);

		sliderSimulateCreateProgress.wholeNumbers = true;
		sliderSimulateCreateProgress.maxValue = 0;
		sliderSimulateCreateProgress.minValue = 0;
		sliderSimulateCreateProgress.gameObject.SetActive(false);
	}

	// ---------- //
	// --- 私有方法
	// ---------- //
	protected override void OnEnteringView(BaseOpenViewData datas)
	{
		ControllerSimulate.Instance.CreateCommanderUI();
		buttonSimulate.interactable = true;
		buttonReplay.interactable = false;
		//buttonBack.interactable = true;
		TimeManager.Instance.IsEditing = false;
		BeginSimulate();
	}

	protected override void OnExitingView(BaseOpenViewData datas)
	{
	}

	/// <summary>
	/// 运行
	/// </summary>
	private void BeginSimulate()
	{
		TimeManager.Instance.IsEditing = false;
		buttonSimulate.interactable = false;
		buttonReplay.interactable = true;
		//buttonBack.interactable = false;
		sliderSimulateCreateProgress.gameObject.SetActive(true);
		StartCoroutine(Ctrl.ProgressCreateSimulation(SimulateProgressSetter, stepCount => sliderSimulateCreateProgress.maxValue = stepCount));
		sliderSimulateCreateProgress.gameObject.SetActive(false);
		CameraActor.Instance.SetCameraWorkState(CameraActor.CameraWorkStates.Follow);
	}

	private void OnClicked_ReplayLevel()
	{
		GameManager.Instance.ReplayCurrentLevel(true);
	}

	//private void DisplayErrorLine(List<List<Vector3>> conflictList, List<Vector3[]> beyoundBoundPos)
	//{
	//	while (_errorLineList.Count < conflictList.Count + beyoundBoundPos.Count)
	//	{
	//		_errorLineList.Add(CreatePartErrorLine());
	//	}
	//	for (int i = 0; i < _errorLineList.Count; i++)
	//	{
	//		_errorLineList[i].SetActive(i < conflictList.Count + beyoundBoundPos.Count);
	//	}
	//	for (int i = 0; i < conflictList.Count; i++)	
	//	{
	//		if (i < conflictList.Count)
	//		{
	//			_errorLineList[i].gameObject.SetActive(true);
	//			conflictList[i].Sort((a, b) => { return a.x < b.x ? -1 : 1; });
	//			_errorLineList[i].GetComponent<LineRenderer>().SetPositions(conflictList[i].ToArray());
	//		}
	//	}
	//	for (int i = conflictList.Count; i < conflictList.Count + beyoundBoundPos.Count; i++)
	//	{
	//		int beyoundBoundPosIndex = i - conflictList.Count;
	//		Array.Sort(beyoundBoundPos[beyoundBoundPosIndex], (a, b) => { return a.x < b.x ? -1 : 1; });
	//		_errorLineList[i].GetComponent<LineRenderer>().SetPositions(beyoundBoundPos[beyoundBoundPosIndex]);
	//	}
	//}

	private GameObject CreatePartErrorLine()
	{
		GameObject conflictLine = new GameObject("Conflict Line" + _errorLineList.Count);
		conflictLine.transform.parent = _conflictLineParent;
		LineRenderer lineCmpnt = conflictLine.AddComponent<LineRenderer>();
		lineCmpnt.startWidth = 0.5f;
		lineCmpnt.positionCount = 4;
		lineCmpnt.numCornerVertices = 6;
		lineCmpnt.material = MaterialConfig.Instance.RedErrorLine;
		lineCmpnt.sortingLayerID = SortingLayer.NameToID("ConnectCursor");
		return conflictLine;
	}

	private void SimulateProgressSetter()
	{
		sliderSimulateCreateProgress.value++;
	}
	// ---------- //
	// --- 公有方法
	// ---------- //
}
