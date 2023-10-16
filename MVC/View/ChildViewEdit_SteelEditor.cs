using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.U2D;

/// <summary>
/// 对于框架(钢材组成的结构)的编辑，采用单独的设计界面，钢材是一个特殊又基础的零件
/// </summary>
public class ChildViewEdit_SteelEditor : BaseChildView
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	private NiceButton BeginEditSteelBtn;
	[SerializeField]
	private NiceButton DeleteIconBtn;
	// ----------------//
	// --- 公有成员
	// ----------------//

	// ----------------//
	// --- 私有成员
	// ----------------//
	/// <summary>
	/// 点击开始编辑后为True
	/// </summary>
	public bool IsEditing { get; private set; } = false;
	[ReadOnly]
	public bool IsMoveingCorner = false;
	/// <summary>
	/// 左击拐角图标后，以该图标为插入点
	/// 删除任意一个拐点后清空
	/// </summary>
	private SteelAccessor CurSteelEditAccesstor
	{
		get
		{
			if (CurSteelCtrl == null) return null;
			return CurSteelCtrl.MyEditPartAccesstor as SteelAccessor;
		}
	}
	private PlayerPartCtrl CurSteelCtrl
	{
		get
		{
			if (!ModelPlayerPartEditor.Instance.IsEditing || (ModelPlayerPartEditor.Instance.GetConnectMain is ScenePartCtrl) || ModelPlayerPartEditor.Instance.EditingPlayerPartCtrl.MyPartType != PartTypes.Steel) return null;
			return ModelPlayerPartEditor.Instance.EditingPlayerPartCtrl;
		}
	}
	private List<(Vector3, Quaternion)> _curSteelCorners = new List<(Vector3, Quaternion)>(11);
	private int _curSteelCornerUpdatedFrame = -1;
	private List<(Vector3, Quaternion)> CurSteelCorners
	{
		get
		{
			if (_curSteelCorners.Count == 0 || _curSteelCornerUpdatedFrame != Time.frameCount)
			{
				PlayerPartSuperFactory.GetFactor<SteelFactory>().GetWorldSpaceCroners_NonAlloc(CurSteelCtrl, CurSteelEditAccesstor, ref _curSteelCorners);
			}
			return _curSteelCorners;
		}
	}

	private List<NiceButton> _cornerNodeBtnList;
	private List<NiceDrag> _dragMoveNode;
	//private List<NiceButton> _btnInsertNode;
	//private int? InsertAtIndex { set; get; }
	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		_cornerNodeBtnList = new List<NiceButton>(10);
		_dragMoveNode = new List<NiceDrag>(10);
		// 添加删除按钮和拖拽按钮
		for (int i = 0; i < _cornerNodeBtnList.Capacity; i++)
		{
			if (i == 0) _cornerNodeBtnList.Add(DeleteIconBtn);
			else _cornerNodeBtnList.Add(Instantiate(DeleteIconBtn.gameObject, DeleteIconBtn.transform.parent).GetComponent<NiceButton>());
			_cornerNodeBtnList[i].gameObject.SetActive(false);
			NiceButton curBtn = _cornerNodeBtnList[i];
			_dragMoveNode.Add(curBtn.GetComponent<NiceDrag>());
			_cornerNodeBtnList[i].OnRightClick.AddListener(() =>
			{
				OnRightClickNode_DeleteNode(curBtn);
			});
		}
		foreach (NiceDrag item in _dragMoveNode)
		{
			NiceDrag curDrag = item;
			item.Drag.AddListener(() => { OnDragCorners(curDrag); });
		}
		BeginEditSteelBtn.onClick.AddListener(OnClicked_TryBeginEditSteelNodes);
	}

	public override void OnParentViewEnter()
	{
		base.OnParentViewEnter();
		// 只有编辑Steel时才显示
		if (/*ModelEdit.Instance.IsEditingPlayerPart && */ModelPlayerPartEditor.Instance.EditingPlayerPartCtrl.MyPartType != PartTypes.Steel)
		{
			BeginEditSteelBtn.gameObject.SetActive(false);
			return;
		}
		BeginEditSteelBtn.gameObject.SetActive(true);
		ResetNodeBtns();
	}

	public override void OnParentViewExit()
	{
		base.OnParentViewExit();
		FinishEditSteel();
	}
	// ----------------//
	// --- 公有方法
	// ----------------//

	// ----------------//
	// --- 私有方法
	// ----------------//
	private void OnClicked_TryBeginEditSteelNodes()
	{
		if (IsEditing) 
			FinishEditSteel(); 
		else 
			BeginEditSteel();
	}

	private void FinishEditSteel()
	{
		BeginEditSteelBtn.gameObject.SetActive(false);
		IsEditing = false;
		BeginEditSteelBtn.Text = "开始编辑";
		ResetNodeBtns();
		StopAllCoroutines();
		ModelPlayerPartEditor.Instance.IsEditingSteel = true;
	}

	private void BeginEditSteel()
	{
		if (CurSteelCtrl == null)
		{
			BeginEditSteelBtn.gameObject.SetActive(false);
			return;
		}
		if (ModelPlayerPartEditor.Instance.IsConnecting)
		{
			GameMessage.Instance.PrintMessageAtMousePos("正在进行连接");
			return;
		}
		IsEditing = true;
		BeginEditSteelBtn.Text = "结束编辑";
		ResetNodeBtns();
		StartCoroutine(Cor_UpdateIconPosition());
		ModelPlayerPartEditor.Instance.IsEditingSteel = true;
	}
	/// <summary>
	/// 左击
	/// </summary>
	/// <param name="clickedBtn"></param>
	private void OnRightClickNode_DeleteNode(NiceButton clickedBtn)
	{
		GameMessage.Instance.PrintMessageAtMousePos("删除点" + _cornerNodeBtnList.IndexOf(clickedBtn));
		PlayerPartSuperFactory.GetFactor<SteelFactory>().RemoveCorner(CurSteelCtrl, _cornerNodeBtnList.IndexOf(clickedBtn), CurSteelEditAccesstor);
	}

	private void OnDragCorners(NiceDrag dragedCmpnt)
	{
		int dragCornerIndex = _dragMoveNode.IndexOf(dragedCmpnt);
		var mouseWorldPos = CurSteelEditAccesstor.transform.InverseTransformPoint(CameraActor.Instance.MouseWorldPos);
		PlayerPartSuperFactory.GetFactor<SteelFactory>().UpdateCorner(CurSteelCtrl, dragCornerIndex, mouseWorldPos);
		//_curSteelEditAccesstor.InsertCorners(InsertAtIndex.Value, mouseWorldPos);
		//SteelFactory.Intsance.InsertCorner(_curSteelCtrl, mouseWorldPos, Quaternion.identity, _curSteelEditAccesstor, InsertAtIndex.Value);
	}

	private void ResetNodeBtns()
	{
		foreach (var item in _cornerNodeBtnList)
		{
			item.gameObject.SetActive(false);
		}
	}

	private int TryGetInsertableIndex(Vector3 inputPos)
	{
		const float minDisToEndPoint = 0.03f;
		float? minInsertDis = null;
		//insertPos = Vector3.zero;
		int? minInsertPointIndex = null;
		for (int i = 1; i < CurSteelCorners.Count; i++)
		{
			Vector3 p2ToP1 = CurSteelCorners[i - 1].Item1 - CurSteelCorners[i].Item1;
			Vector3 p2ToInput = inputPos - CurSteelCorners[i].Item1;
			Vector3 p1ToInput = inputPos - CurSteelCorners[i - 1].Item1;
			if (Vector3.Dot(p2ToInput, p2ToP1) <= 0 || Vector3.Dot(p1ToInput, -p2ToP1) <= 0) continue; // 判断如果处在端点两边则返回
			if (p2ToInput.sqrMagnitude < minDisToEndPoint || p1ToInput.sqrMagnitude < minDisToEndPoint) continue;
			Vector3 helpVec = Vector3.Cross(p2ToP1, Vector3.Cross(p2ToP1, p2ToInput)).normalized;
			float dis = Mathf.Abs(Vector3.Dot(p2ToInput, helpVec));
			if (!minInsertDis.HasValue || minInsertDis.Value > dis)
			{
				minInsertDis = dis;
				minInsertPointIndex = i - 1;
				//insertPos = inputPos + -helpVec * dis;
			}
		}
		if (!minInsertDis.HasValue || minInsertDis.Value > 0.05f)
		{
			return -1;
		}
		//Debug.DrawLine(inputPos, insertPos);
		return minInsertPointIndex.HasValue ? minInsertPointIndex.Value : -1;
	}

	private IEnumerator Cor_UpdateIconPosition()
	{
		RectTransform rect = UIManager.Instance.GetComponent<RectTransform>();
		while (enabled && CurSteelEditAccesstor)
		{
			//worldSpaceCornerPoss = SteelFactory.Intsance.GetCorners_WorldSpace(_curSteelCtrl, _curSteelEditAccesstor);
			// 显示拐点按钮
			for (int i = 0; i < _cornerNodeBtnList.Count; i++)
			{
				if (i > CurSteelCorners.Count)
				{
					_cornerNodeBtnList[i].gameObject.SetActive(false);
					continue;
				}
				_cornerNodeBtnList[i].gameObject.SetActive(true);
				Vector2 screenPos;

				if (i == CurSteelCorners.Count)
				{
					screenPos = _cornerNodeBtnList[i].transform.position + (_cornerNodeBtnList[i].transform.localPosition - _cornerNodeBtnList[i - 1].transform.localPosition).normalized * 0.1f;
				}
				else
				{
					screenPos = (Vector2)CameraActor.Instance.MainCamera.WorldToScreenPoint(CurSteelCorners[i].Item1);
				}
				//RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPos, CameraActor.Instance.MainCamera, out Vector2 loaclPoint);
				//_btnDeleteNode[i].transform.localPosition = loaclPoint;
				_cornerNodeBtnList[i].transform.position = screenPos ;
			}
			yield return null;
		}
		Debug.Log("退出");
	}
	// ----------------//
	// --- 类型
	// ----------------//
}
