using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PartManager
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public static PartManager Instance = new PartManager();
	/// <summary>
	/// 玩家零件和场景零件总数
	/// </summary>
	public int AllPartCount => PlayerPartManager.Instance.AllPlayerPartCtrls.Count + 
		(LevelProgressBase.Instance.CurrentEditZone != null ? LevelProgressBase.Instance.CurrentEditZone.AllSceneParts.Count : 0);

	public List<PlayerPartCtrl> AllPlayerPart { get => PlayerPartManager.Instance.AllPlayerPartCtrls; }
	public List<ScenePartCtrl> AllScenePart { get => LevelProgressBase.Instance.CurrentEditZone.AllSceneParts; }

	public BasePartCtrl DragingPart { get; private set; } 
	public Dictionary<BasePartCtrl,Vector2> FollowingParts { get; private set; } = new (8);
	public Dictionary<BasePartConnection, Vector2> FollowingConnections { get; private set; } = new (4);
	public List<BasePartConnection> PartConnections { get; private set; } = new List<BasePartConnection>(8);
	public bool IsDraging => DragingPart!=null;

	public BasePartCtrl EditingPart 
	{
		private set
		{
			_editingPart = value;
		}
		get
		{
			if(_editingPart != null )
			{
				if(AllPlayerPart.Contains(_editingPart) ||  AllScenePart.Contains(_editingPart) ) 
				{
					return _editingPart;
				}
			}
			return null;
		}
	}
	public UnityEvent<BasePartCtrl> OnStartEditPart = new UnityEvent<BasePartCtrl>();
	public UnityEvent OnFinishiEditPart = new UnityEvent();

	// ----------------//
	// --- 私有成员
	// ----------------//
	/// <summary>
	/// 正在查找拖拽零件
	/// 为true时，说明已经在筛选了
	/// </summary>
	bool isSelectingDragPart = false;
	private BasePartCtrl _editingPart;
	private List<BasePartCtrl> OverlapedPartsCache = new List<BasePartCtrl>(8);

	// ----------------//
	// --- 公有方法
	// ----------------//
	/// <summary>
	/// 获取零件，包括场景零件
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public BasePartCtrl GetPart(int index)
	{
		if (index < PlayerPartManager.Instance.AllPlayerPartCtrls.Count)
		{
			return PlayerPartManager.Instance.AllPlayerPartCtrls[index];
		}
		else
		{
			return LevelProgressBase.Instance.CurrentEditZone.AllSceneParts[index - PlayerPartManager.Instance.AllPlayerPartCtrls.Count];
		}
	}

	/// <summary>
	/// 测试零件之间是否有冲突的碰撞器
	/// </summary>
	/// <param name="conflictPart"></param>
	/// <returns></returns>
	public bool GetConflictPart(out List<(BasePartCtrl, BasePartCtrl)> ConflictParts)
	{
		bool recordActiveState = ParentsManager.Instance.ParentOfEditParts.gameObject.activeSelf;
		ParentsManager.Instance.ParentOfEditParts.gameObject.SetActive(true);
		ConflictParts = new List<(BasePartCtrl, BasePartCtrl)>();
		for (int partIndex = 0; partIndex < AllPartCount; partIndex++)
		{
			for (int partIndex2 = partIndex + 1; partIndex2 < AllPartCount; partIndex2++)
			{
				BasePartCtrl partA = GetPart(partIndex);
				BasePartCtrl partB = GetPart(partIndex2);
				if (partA == partB) continue;
				if (partA.OverlapOther(partB))
				{
					ConflictParts.Add((partA, partB));
				}
			}
		}
		ParentsManager.Instance.ParentOfEditParts.gameObject.SetActive(recordActiveState);
		return ConflictParts.Count > 0;
	}

	public List<(BasePartCtrl, BasePartCtrl)> GetConflictData(BasePartCtrl includePart = null)
	{
		bool recordActiveState = ParentsManager.Instance.ParentOfEditParts.gameObject.activeSelf;
		ParentsManager.Instance.ParentOfEditParts.gameObject.SetActive(true);
		List<(BasePartCtrl, BasePartCtrl)> conflictParts = new List<(BasePartCtrl, BasePartCtrl)>();
		for (int partIndex = 0; partIndex < AllPartCount; partIndex++)
		{
			for (int partIndex2 = partIndex + 1; partIndex2 < AllPartCount; partIndex2++)
			{
				BasePartCtrl partA = GetPart(partIndex);
				BasePartCtrl partB = GetPart(partIndex2);
				if (includePart != null && includePart != partA && includePart != partB) continue;
				if (partA == partB) continue;
				if (partA.OverlapOther(partB))
				{
					conflictParts.Add((partA, partB));
				}
			}
		}
		ParentsManager.Instance.ParentOfEditParts.gameObject.SetActive(recordActiveState);
		return conflictParts;
	}

	public void RegistPartDoubleClickListener(BasePartCtrl registPart)
	{
		if (registPart.PartDoubleClickCmpnt == null) return;
		registPart.PartDoubleClickCmpnt.OnDoubleClick.AddListener(() => {
			PartSelector.Instance.SelectClickedPart(registPart);
		});
	}

	public void RegistPartDragingListener(BasePartCtrl registPart)
	{
		if (registPart.PartDragCmpnt == null) return;
		registPart.PartDragCmpnt.OnDragStart.AddListener((UnityAction)(() => {
			BasePartCtrl selectedPart;
			if (DragingPart == null)
			{ 
				selectedPart = SelectDragingPart(registPart);
				registPart.PartDragCmpnt.CancleDrag();
				DragingPart = selectedPart;
				selectedPart.PartDragCmpnt.ForceDrag(true, true);
				return; // 这个零件不是选择后的零件
			}
			else
			{
				Debug.Assert(DragingPart ==  registPart);
				selectedPart = registPart;
			}

			bool dragable = UpdateFollowingPartAndConnection(selectedPart);
			if (dragable == false)
			{
				FollowingConnections.Clear();
				FollowingParts.Clear();
				selectedPart.PartDragCmpnt.CancleDrag();
			}
			else
			{
				foreach (var item in FollowingParts.Keys)
				{
					item.PartDragCmpnt.IsSnapableTarget = false;
				}
			}
		}));

		registPart.PartDragCmpnt.OnDraging.AddListener((Vector2 v2) =>
		{
			foreach (KeyValuePair<BasePartCtrl, Vector2> itemFollowingPart in FollowingParts)
			{
				itemFollowingPart.Key.Position = itemFollowingPart.Value + DragingPart.Position;
			}
			foreach (KeyValuePair<BasePartConnection, Vector2> itemFollowingConn in FollowingConnections)
			{
				itemFollowingConn.Key.AnchorPosition = itemFollowingConn.Value + DragingPart.Position;
			}
			PartMaterialSetter.Instance.UpdatePartMaterials();
		});

		registPart.PartDragCmpnt.OnDragEnd.AddListener(() => {
			DragingPart = null;
			foreach (var item in FollowingParts.Keys)
			{
				item.PartDragCmpnt.IsSnapableTarget = true;
			}
			//OnEndDragPlayerPart?.Invoke(playerPart); 
		});
	}

	private void OnEditingNewPart(BasePartCtrl editPart)
	{
		EditingPart = editPart;
		if (EditingPart != null)
		{
			OnStartEditPart?.Invoke(EditingPart);
		}
		else
		{
			OnFinishiEditPart?.Invoke();
		}
		PartConnectionManager.Instance.UpdateEditBearings(EditingPart);
		PartMaterialSetter.Instance.UpdatePartMaterials();
	}

	private void OnFinishEditPart()
	{
		EditingPart = null;
		PartConnectionManager.Instance.UpdateEditBearings(EditingPart);
	}

	private void ResetData()
	{
		//OnStartDragPlayerPart.RemoveAllListeners();
		//OnEndDragPlayerPart.RemoveAllListeners();
		ControllerPlayerPartEditor.Instance.OnStartEditPlayerPart.RemoveListener(OnEditingNewPart);
		ControllerPlayerPartEditor.Instance.OnFinishEditPlayerPart.RemoveListener(OnFinishEditPart);
		ControllerScenePartEditor.Instance.OnStartEditScenePart.RemoveListener(OnEditingNewPart);
		ControllerScenePartEditor.Instance.OnFinishEditScenePart.RemoveListener(OnFinishEditPart);
		Instance = new PartManager();
	}


	public void GetMouseOverlapedParts(List<BasePartCtrl> result)
	{
		result.Clear();
		Vector3 mouseWolrdPos = CameraActor.Instance.MouseWorldPos;
		for (int i = 0; i < PartManager.Instance.AllPartCount; i++)
		{
			BasePartCtrl part = PartManager.Instance.GetPart(i);
			if (part.OverlapPoint(mouseWolrdPos)) result.Add(part);
		}
	}
	// ----------------//
	// --- 私有方法
	// ----------------//
	private PartManager ()
	{
		ControllerPlayerPartEditor.Instance.OnStartEditPlayerPart.AddListener(OnEditingNewPart);
		ControllerPlayerPartEditor.Instance.OnFinishEditPlayerPart.AddListener(OnFinishEditPart);
		ControllerScenePartEditor.Instance.OnStartEditScenePart.AddListener(OnEditingNewPart);
		ControllerScenePartEditor.Instance.OnFinishEditScenePart.AddListener(OnFinishEditPart);
	}

	/// <summary>
	/// 拖拽直接或间接连接的零件
	/// 已删除
	/// </summary>
	/// <param name="dirctlyDragpart"></param>
	//public void DragJointlyParts(BasePartCtrl dirctlyDragpart)
	//{
	//	isSelectingDragPart = true;
	//	// 找出应该拖拽的零件
	//	BasePartCtrl result = SelectDragingPart(dirctlyDragpart);
	//	//DragingParts.Add(result);
	//	// 虽然这里重复了一些零件
	//	// 但是拖拽通知还是对的，但是被强制的零件没有移动
	//	var conntingParts = GetConnectedPart(result);
	//	foreach (var itemConntingPart in conntingParts)
	//	{
	//		if (!DragingPart.Contains(itemConntingPart))
	//		{
	//			itemConntingPart.PartDragCmpnt.ForceDrag(true, true);
	//		}
	//	}
	//	isSelectingDragPart = false;
	//	//OnStartDragPlayerPart?.Invoke(playerPart); 
	//}

	/// <summary>
	/// 查找与该零件有直接连接和间接连接的所有零件
	/// </summary>
	/// <param name="main"></param>
	/// <returns></returns>
	private bool UpdateFollowingPartAndConnection(BasePartCtrl main)
	{
		FollowingConnections.Clear();
		FollowingParts.Clear();
		FollowingParts.Add(main, Vector2.zero);
		int justAddedCount;
		do
		{
			justAddedCount = 0;
			foreach (var itemAddedPart in FollowingParts)
			{
				foreach (var itemAllConn in PartConnectionManager.Instance.AllEditConnection)
				{
					if (itemAllConn.ConnectedParts.ContainsKey(itemAddedPart.Key))
					{
						if(FollowingConnections.ContainsKey(itemAllConn) == false)
						{
							FollowingConnections.Add(itemAllConn, itemAllConn.AnchorPosition - main.Position);
						}
						foreach (var itemAddingPart in itemAllConn.ConnectedParts)
						{
							BasePartCtrl addingPart = itemAddingPart.Key as BasePartCtrl;
							if(addingPart.IsPlayerPart == false)
							{
								if ((addingPart as ScenePartCtrl).EnableDrag == false)
								{
									return false;
								}
							}
							if (!FollowingParts.ContainsKey(addingPart))
							{
								FollowingParts.Add(addingPart, addingPart.Position - main.Position);
								justAddedCount++;
							}
						}
						if (justAddedCount > 0) break;
					}
				}
				if (justAddedCount > 0) break;
			}
		} while (justAddedCount > 0);
		return true;
	}

	/// <summary>
	/// 选择出真正该被拖拽的零件
	/// 依据拖拽组件直接接收到的拖拽事件，正在编辑的零件和拖拽位置，查找优先级高（正在编辑且鼠标覆盖）的零件拖拽
	/// </summary>
	/// <param name="dragingPart"></param>
	/// <returns></returns>
	private BasePartCtrl SelectDragingPart(BasePartCtrl dragingPart)
	{
		dragingPart.PartDragCmpnt.CancleDrag();
		BasePartCtrl selectPart = dragingPart;
		// 当没有编辑零件时无需挑选
		// 当编辑零件就是拖拽零件时无需挑选
		if (EditingPart != null && dragingPart != EditingPart)
		{
			OverlapedPartsCache.Clear();
			GetMouseOverlapedParts(OverlapedPartsCache);
			if (OverlapedPartsCache.Contains(EditingPart))
			{
				selectPart = EditingPart;
			}
		}

		return selectPart;
	}

	// ----------------//
	// --- 类型
	// ----------------//
}
