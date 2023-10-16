using System;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Text;
using Unity.Collections;
using static BasePartConnection;
using UnityEngine.Localization.SmartFormat.Utilities;

public class PartConnectionManager:Loadable<PartConnectionManager>
{
	public PartConnectionManager() { }
	// --------------- //
	// --  私有成员
	// --------------- //

	// --------------- //
	// --  公有成员
	// --------------- //
	public List<BasePartConnection> AllEditConnection { private set; get; } = new List<BasePartConnection>();

	// --------------- //
	// --  公有方法
	// --------------- //
	/// <summary>
	/// 快速创建固接在轴承的两个零件
	/// </summary>
	/// <param name="isFixedOrHinge"></param>
	/// <returns></returns>
	// public bool TryCreateFixedToBearing(IConnectableCtrl part1, IConnectableCtrl part2, Vector2 anchorPos, StringBuilder sb)
	// {
	// 	BasePartConnection conn;
	// 	if (part1 == null || part2 == null)
	// 	{
	// 		sb.Append("必须同时选中两个零件");
	// 		return false;
	// 	}

	// 	//IConnectableCtrl part1 = Model.EditingPlayerPartCtrl;
	// 	//Vector2 anchorPos = Model.CurrentConnectCursorPos.Value;
	// 	if (!part1.TryGetOverlapPointEditColliderIndex(anchorPos, out int firstPartConnIndex))
	// 	{
	// 		sb.Append("-找不到“主物体”可铰接的碰撞器");
	// 		return false;
	// 	}
	// 	//if (Model.WillConnectTarget == null)
	// 	//{
	// 	//	sb.Append("没有可铰接的目标零件");
	// 	//	return false;
	// 	//}
	// 	//IConnectableCtrl part2 = Model.WillConnectTarget;
	// 	if (!part2.TryGetOverlapPointEditColliderIndex(anchorPos, out int secondPartConnIndex))
	// 	{
	// 		sb.Append("-找不到“铰接对象”可铰接的碰撞器");
	// 		return false;
	// 	}
	// 	conn = PartConnectionManager.Instance.CreateFixedConnection(part1, firstPartConnIndex, part2, secondPartConnIndex, anchorPos);
	// 	//Model.IsDirty_BearingColor = true;
	// 	if (conn != null)
	// 	{
	// 		return true;
	// 	}
	// 	else
	// 	{
	// 		Debug.LogError("铰接对象创建失败！！！");
	// 		return false;
	// 	}
	// }

	public bool TryCreateHingeConnection(IConnectableCtrl part, bool isFixed, Vector2 bearingPlacePos, StringBuilder sb)
	{
		BasePartConnection conn = PartConnectionManager.Instance.CreateConection(bearingPlacePos);
		if (!part.TryGetOverlapPointEditColliderIndex(bearingPlacePos, out int overlapColliderIndex))
		{
			return false;
		}
		conn.ConnectedParts.Add(part, new BasePartConnection.ConnectionDatas(overlapColliderIndex, isFixed));
		return conn != null;
	}

	/// <summary>
	/// 将主零件连接到轴承
	/// </summary>
	/// <param name="part"></param>
	/// <param name="bearingGO"></param>
	/// <returns></returns>
	public bool TryConnectPartToBearing(IConnectableCtrl part, bool isFixed, EditBearing bearingGO)
	{
		// 轴承图标所代表的连接
		BasePartConnection findResult = AllEditConnection.Find(a => a.EditBearing == bearingGO);
		if (findResult == null)
		{
			return false;
		}
		// 主零件必须覆盖轴承位置
		if (!part.TryGetOverlapPointEditColliderIndex(bearingGO.transform.position, out int overlapColliderIndex))
		{
			return false;
		}
		// 轴承没有主零件一说，可连接列表的零件均可删除。
		// 不过可以设计成属于主零件，默认第一个连接对象
		// 或者修改conn数据结构，增加一个字段为连接的主零件
		PartHingeConnection hingeConnection = findResult as PartHingeConnection;
		// 添加当前零件到该轴承连接
		var addingData = new ConnectionDatas(overlapColliderIndex, isFixed);
		if (hingeConnection.ConnectedParts.ContainsKey(part)) hingeConnection.ConnectedParts.Remove(part);
		hingeConnection.ConnectedParts.Add(part, addingData);
		return true;
	}


	/// <summary>
	/// 创建物理连接
	/// 连接物理碰撞器不依赖物理零件激活
	/// </summary>
	public System.Collections.IEnumerator CreatePhysicsConnections(float waitSeconds, Action progressAddSeeter)
	{
		Transform bearingParent = ParentsManager.Instance.ParentOfPhysicsConnection.transform;
		bearingParent.gameObject.SetActive(true);
		foreach (var itemEditConnection in AllEditConnection)
		{
			GameObject bearing = GameObject.Instantiate(PartConfig.Instance.PhysicsConnection, bearingParent);
			bearing.transform.position = itemEditConnection.AnchorPosition;
			Collider2D bearingCollider = bearing.GetComponentInChildren<Collider2D>(true);
			//Debug.Assert(itemEditConnection.ConnectedParts.Count >= 2);
			PhysicsBearing physicsBearing = bearing.GetComponent<PhysicsBearing>();
			itemEditConnection.BearingPhysics = physicsBearing;
			physicsBearing.SetDisplay(false);

			foreach (var itemConnectedPart in itemEditConnection.ConnectedParts)
			{
				// 尚未覆盖到零件的连接不予实现物理连接
				if (!itemConnectedPart.Key.OverlapPoint(itemEditConnection.AnchorPosition)) 
				{
					Debug.LogError($"点 {itemEditConnection.AnchorPosition} 无法被 {(itemConnectedPart.Key as PlayerPartCtrl).MyEditPartAccesstor.gameObject.GetHashCode()} 覆盖");
					break; 
				}
				ConnectPhysicsCollider(bearingCollider, itemConnectedPart.Key.GetPhysicsCollider(itemConnectedPart.Value.ConnectColliderIndex), itemEditConnection.AnchorPosition, itemConnectedPart.Value.IsFixed);
				SetIgnorePart(itemConnectedPart.Key, itemEditConnection.ConnectedParts, bearingCollider);
			}
			progressAddSeeter();
			yield return new WaitForSeconds(waitSeconds);
		}
	}
	public bool FindConnectableColliders ( Vector2 containPos,PlayerPartCtrl self, out List<Collider2D> findResult)
	{
		findResult = new List<Collider2D>();
		for (int i = 0; i < PlayerPartManager.Instance.AllColliders.Count; i++)
		{
			if (PlayerPartManager.Instance.ColliderToPartCtrl[PlayerPartManager.Instance.AllColliders[i]] != self)
			{
				if (PlayerPartManager.Instance.AllColliders[i].OverlapPoint(containPos))
				{
					findResult.Add(PlayerPartManager.Instance.AllColliders[i]);
				}
			}
		}

		foreach (PlayerPartCtrl item in PlayerPartManager.Instance.AllPlayerPartCtrls)
		{
			if (item != self)
			{
				if(item.MyEditPartAccesstor.GetConnectableCollider(containPos, out Collider2D connectableCollider))
				{
					findResult.Add(connectableCollider);
				}
			}
		}
		return findResult.Count > 0;
	}

	public bool OverlapPart ( Vector2 containPos, PlayerPartCtrl ctrlData, out Collider2D result)
	{
		var partColliderList = ctrlData.MyEditPartAccesstor.AllConectableColliders;
		for (int i = 0; i < partColliderList.Count; i++)
		{
			bool recordActive = partColliderList[i].gameObject.activeSelf;
			bool recordEnable = partColliderList[i].enabled;
			partColliderList[i].gameObject.SetActive(true);
			partColliderList[i].enabled = true;
			if (partColliderList[i].OverlapPoint(containPos))
			{
				result = partColliderList[i];
				return true;
			}
			partColliderList[i].gameObject.SetActive(recordActive);
			partColliderList[i].enabled = recordEnable;
		}
		result = null;
		return false;
	}

	/// <summary>
	/// 创建一个连接
	/// </summary>
	/// <param name="master"></param>
	/// <param name="masterColliderIndex"></param>
	/// <param name="target"></param>
	/// <param name="targetColliderIndex"></param>
	/// <param name="isFixed"></param>
	/// <param name="connectWorldPos"></param>
	/// <returns></returns>
	// public PartFixedConnection CreateFixedConnection(IConnectableCtrl master, int masterColliderIndex, IConnectableCtrl target, int targetColliderIndex, Vector2 connectWorldPos)
	// {
	// 	PartFixedConnection partConnection = new PartFixedConnection(connectWorldPos, master, masterColliderIndex, target, targetColliderIndex);
	// 	AllEditConnection.Add(partConnection);
	// 	return partConnection;
	// }

	/// <summary>
	/// 创建一个连接
	/// </summary>
	/// <param name="anchorPos"></param>
	/// <returns></returns>
	public PartHingeConnection CreateConection(Vector2 anchorPos)
	{ 

		PartHingeConnection partConnection = new PartHingeConnection(anchorPos);
		AllEditConnection.Add(partConnection);
		return partConnection;
	}

	/// <summary>
	/// 连接这个零件到轴承
	/// </summary>
	/// <param name="bearingObject"></param>
	/// <param name="part"></param>
	/// <param name="connectPartColliderIndex"></param>
	public void ConnectPartToBearing(GameObject bearingObject, IConnectableCtrl part, int connectPartColliderIndex, bool isFixedJoint)
	{
		PartHingeConnection hingeConnection = AllEditConnection.Find(conn => conn.EditBearing == bearingObject) as PartHingeConnection;
		Debug.Assert(hingeConnection != null);
		if (hingeConnection.ConnectedParts.ContainsKey(part))
		{
			Debug.LogError("该零件已经连接到此轴承，重复连接");
			hingeConnection.ConnectedParts.Remove(part);
		}
		if (!hingeConnection.ConnectedParts.ContainsKey(part))
		{
			hingeConnection.ConnectedParts.Add(part, new ConnectionDatas(connectPartColliderIndex, isFixedJoint));
		}
		else
		{
			hingeConnection.ConnectedParts[part] = new ConnectionDatas(connectPartColliderIndex, isFixedJoint);
		}
	}

	public void DisconnectPartFromBearing(EditBearing bearingObject, IConnectableCtrl part)
	{ 
		PartHingeConnection hingeConnection = AllEditConnection.Find(conn => conn.EditBearing == bearingObject) as PartHingeConnection;
		if (hingeConnection.ConnectedParts.ContainsKey(part) == false) return;
		hingeConnection.ConnectedParts.Remove(part );
		//UpdateEditBearings(part);
	}

	/// <summary>
	/// 删除该连接
	/// 删除连接在这个轴承上的所有
	/// </summary>
	/// <param name="scrw"></param>
	public void DeleteConnection(BasePartConnection deleteTarget)
	{
		DeleteConnection(deleteTarget.EditBearing);
	}

	public void DeleteConnection(EditBearing scrw)
	{
		//PartConnection findResult = AllConnection.Find((PartConnection item) => { return item.BearingEditIcon == scrw; });
		//AllConnection.Remove(findResult);
		BasePartConnection findResult = AllEditConnection.Find(itemConn => itemConn.EditBearing == scrw);
		findResult.ConnectedParts.Clear();
		AllEditConnection.Remove(findResult);
		scrw.gameObject.SetActive(false);
		GameObject.Destroy(scrw.gameObject);
	}

	/// <summary>
	/// 清除所有和该零件有关的数据
	/// 用于零件删除或清空该零件连接
	/// </summary>
	/// <param name="part"></param>
	public void ClearPartConnection(BasePartCtrl part)
	{
		//List<BasePartConnection> removeList = AllConnection.FindAll(item => { return item.MasterPart == part || item.TargetPart == part; });
		foreach (BasePartConnection itemConntion in AllEditConnection)
		{
			itemConntion.ConnectedParts.Remove(part);
		}
		// 删除没有连接目标的轴承
		List<BasePartConnection> deleteTargets = AllEditConnection.FindAll(itemConn => itemConn.ConnectedParts.Count <= 1);
		while (deleteTargets.Count > 0)
		{
			DeleteConnection(deleteTargets[0]);
			deleteTargets.RemoveAt(0);
		}
		// 这里可能会留下一些连接的数据，这些连接只连接到一个零件
	}

	public void SetActiveEditBearings(bool active)
	{
		foreach (var item in AllEditConnection)
		{
			item.EditBearing.gameObject.SetActive(active);
		}
	}

	//public void UpdateBearingColor(IConnectableCtrl editingPart)
	//{
	//	foreach (var itemConn in AllEditConnection)
	//	{
	//		if (!itemConn.IsFixedJoint
	//			&& editingPart.OverlapPoint(itemConn.AnchorPosition) // 这里原文是场景零件调用
	//			&& !itemConn.ConnectedParts.ContainsKey(editingPart)) //该轴承覆盖当前零件，而且没有连接到该零件
	//		{
	//			itemConn.Bearing.SetDisplay(true, true);
	//		}
	//		else
	//		{
	//			itemConn.Bearing.SetDisplay(true, false);
	//		}
	//	}
	//}

	/// <summary>
	/// 设置轴承图标的状态
	/// </summary>
	/// <param name="forceUpdate"></param>
	public void UpdateEditBearings(IConnectableCtrl mainPart)
	{
		//if(mainPart != null)
		//	Debug.LogError(mainPart.GetHashCode().ToString() + (mainPart as BasePartCtrl).MyPartType);
		//if (forceUpdate == false && Model.DirtyFlag_BearingColor == false) return;
		//// 查找轴承
		//if (Model.IsFindingBearing == false) Model.DirtyFlag_BearingColor = false; // ???

		foreach (var itemConn in AllEditConnection)
		{
			//if (editingPart.OverlapPoint(itemConn.AnchorPosition) && !itemConn.ConnectedParts.ContainsKey(editingPart)) //该轴承覆盖当前零件，而且没有连接到该零件
			//{
			//	itemConn.EditBearing.SetFunction(true, !itemConn.IsFixedJoint);
			//}
			//else
			//{
			//	itemConn.EditBearing.SetFunction(true, false);
			//}
			//itemConn.EditBearing.gameObject.SetActive(itemConn.ConnectedParts.ContainsKey(editingPart));

			bool hasMainPart = mainPart != null;
			bool overlapedMainPart = hasMainPart && mainPart.OverlapPoint(itemConn.AnchorPosition);
			bool hasConnectedMainPart = hasMainPart && itemConn.ConnectedParts.ContainsKey(mainPart);
			//bool connectable = hasMainPart && !hasConnectedMainPart /*&& !isFixedConnection*/ && overlapedMainPart;
			bool fixedConnectable = overlapedMainPart && (!itemConn.ConnectedParts.ContainsKey(mainPart) || !itemConn.ConnectedParts[mainPart].IsFixed);
			bool shaftConnectable = overlapedMainPart && (!itemConn.ConnectedParts.ContainsKey(mainPart) || itemConn.ConnectedParts[mainPart].IsFixed);

			bool deleteable = true; // ??? 建议在非编辑当前零件时，不可删除与当前零件无关的连接
			bool disconnectable = hasMainPart && hasConnectedMainPart /*&& !isFixedConnection*/;
			itemConn.EditBearing.SetButtonsDisplay(deleteable, shaftConnectable, fixedConnectable, disconnectable);
		}
	}


	// --------------- //
	// --  私有方法
	// --------------- //
	// 加载时场景对象有交接出错
	// 场景零件的item2为-1
	protected override void OnLoadedArchive(ArchiveManager.Archive archive)
	{
        OnResetData();
        foreach (PartConnectionCoreData itemConnectionData in archive.partConnectionsArchive)
		{
			BasePartConnection connection;
			//if (itemConnectionData.IsFixed)
			//{
			//	IConnectableCtrl part1 = GetPartCtrlByStrID(itemConnectionData.IDsConnectedPart[0]);
			//	IConnectableCtrl part2 = GetPartCtrlByStrID(itemConnectionData.IDsConnectedPart[1]);
			//	int part1ConnectIndex = itemConnectionData.IndexConnectedPartCollider[0];
			//	int part2ConnectIndex = itemConnectionData.IndexConnectedPartCollider[1];
			//	connection = new PartFixedConnection(itemConnectionData.AnchorPosition, part1, part1ConnectIndex, part2, part2ConnectIndex);
			//}
			//else
			//{
			connection = new PartHingeConnection(itemConnectionData.AnchorPosition);
			for (int i = 0; i < itemConnectionData.IDsConnectedPart.Length; i++)
			{
				string itemConnectedPartIDs = itemConnectionData.IDsConnectedPart[i];
				IConnectableCtrl part = GetPartCtrlByStrID(itemConnectedPartIDs);
				var addValue = new ConnectionDatas(itemConnectionData.IndexConnectedPartCollider[i], itemConnectionData.FlagConneectedPartIsFixed[i]);
				if (!connection.ConnectedParts.ContainsKey(part))
				{
					connection.ConnectedParts.Add(part, addValue);
				}
				else
				{
					connection.ConnectedParts[part] = addValue;
				}
			}
			//}
			AllEditConnection.Add(connection);
			//PlayerPartCtrl part1;
			//IConnectableCtrl part2;
			//part1 = PlayerPartManager.Instance.AllPlayerPartCtrls[int.Parse(itemConnectionData.part1ID)];
			//if (int.TryParse(itemConnectionData.part2ID, System.Globalization.NumberStyles.Integer, null, out int part1IDNum))
			//{
			//	part2 = PlayerPartManager.Instance.AllPlayerPartCtrls[part1IDNum];
			//}
			//else
			//{
			//	part2 = new ScenePartCtrl(LevelProgressBase.Instance.GetScenepart(Hash128.Parse(itemConnectionData.part2ID))) as IConnectableCtrl;
			//}
			//CreateConnection(part1, itemConnectionData.part1ColliderIndex, part2, itemConnectionData.part2ColliderIndex, itemConnectionData.isFixedJoint, itemConnectionData.anchorPos);
			//AllConnection.Add(conn);
		}
		//ModelEdit.Instance.DirtyFlag_BearingColor = true;
		UpdateEditBearings(null);
		//foreach (BasePartConnection item in AllConnection)
		//{
		//	item.Bearing.GetComponent<EditingBearing>().SetDisplay(false, false);
  //      }
	}

	protected override void OnSaveingArchive(ArchiveManager.Archive archive)
	{
		archive.partConnectionsArchive = new (AllEditConnection.Count);
		foreach (BasePartConnection itemConnection in AllEditConnection)
		{
			PartConnectionCoreData connData = new PartConnectionCoreData()
			{
				AnchorPosition = itemConnection.AnchorPosition,
				IDsConnectedPart = new string[itemConnection.ConnectedParts.Count],
				IndexConnectedPartCollider = new int[itemConnection.ConnectedParts.Count],
				FlagConneectedPartIsFixed = new bool[itemConnection.ConnectedParts.Count],
			};
			int indexConnectedPart = 0;
			foreach (var itemConnectedPart in itemConnection.ConnectedParts)
			{
				connData.IDsConnectedPart[indexConnectedPart] = GetStrIDByPartCtrl(itemConnectedPart.Key);
				//foreach (int itemConnectColliderIndex in itemConnectedPart.Value)
				//{
				//	connData.connectedPartCollidersIndex[indexConnectedPart] = itemConnectColliderIndex;
				//} ???
				connData.IndexConnectedPartCollider[indexConnectedPart] = itemConnectedPart.Value.ConnectColliderIndex;
				connData.FlagConneectedPartIsFixed[indexConnectedPart] = itemConnectedPart.Value.IsFixed;
				indexConnectedPart++;
			}
			archive.partConnectionsArchive.Add(connData);
		}
	}


	protected override void OnResetData()
	{
		AllEditConnection.Clear();
	}


	/// <summary>
	/// 对两个碰撞器增加铰接组件
	/// </summary>
	/// <param name="colliderA"></param>
	/// <param name="colliderB"></param>
	/// <param name="anchorPos"></param>
	/// <param name="isFixed"></param>
	/// <returns></returns>
	private AnchoredJoint2D ConnectPhysicsCollider(Collider2D colliderA, Collider2D colliderB, Vector2 anchorPos, bool isFixed)
	{
		Rigidbody2D rigidbodyA = colliderA.attachedRigidbody;
		Rigidbody2D rigidbodyB = colliderB.attachedRigidbody;
		AnchoredJoint2D jointCmpnt = (isFixed ? colliderA.gameObject.AddComponent<FixedJoint2D>() : colliderA.gameObject.AddComponent<HingeJoint2D>());
		Vector2 anchorLocal = rigidbodyA.transform.InverseTransformPoint(anchorPos);
		Vector2 anchorTargeLocal = rigidbodyB.transform.InverseTransformPoint(anchorPos);
		jointCmpnt.enabled = false;
		jointCmpnt.connectedBody = rigidbodyB.GetComponent<Rigidbody2D>();
		jointCmpnt.anchor = anchorLocal;
		jointCmpnt.connectedAnchor = anchorTargeLocal;
		jointCmpnt.enableCollision = false;
		jointCmpnt.autoConfigureConnectedAnchor = false;
		jointCmpnt.enabled = true;
		return jointCmpnt;
	}

	/// <summary>
	/// 获取零件的ID
	/// 玩家零件为序号
	/// 场景零件为生成的随机值
	/// </summary>
	/// <param name="partCtrl"></param>
	/// <returns></returns>
	private string GetStrIDByPartCtrl(IConnectableCtrl partCtrl)
	{
		if (partCtrl is PlayerPartCtrl)
		{
			return PlayerPartManager.Instance.AllPlayerPartCtrls.IndexOf(partCtrl as PlayerPartCtrl).ToString();
		}
		else
		{
			return (partCtrl as ScenePartCtrl).GetID.ToString();
		}
	}

	private IConnectableCtrl GetPartCtrlByStrID(string strID)
	{
		if (int.TryParse(strID, System.Globalization.NumberStyles.Integer, null, out int partIndex))
		{
			return PlayerPartManager.Instance.AllPlayerPartCtrls[partIndex];
		}
		else
		{
			return LevelProgressBase.Instance.GetScenepart(Hash128.Parse(strID)).MyCtrl as IConnectableCtrl;
			return ScenePartManager.Instance.AllScenePartCtrls[Hash128.Parse(strID)];
		}
	}

	/// <summary>
	/// 对被连接的零件设置碰撞器忽略
	/// </summary>
	/// <param name="partMain"></param>
	/// <param name="connectedParts"></param>
	/// <param name="bearing"></param>
	private void SetIgnorePart(IConnectableCtrl partMain, Dictionary<IConnectableCtrl, ConnectionDatas> connectedParts, Collider2D bearing = null)
	{
		// 所有连接在这个轴承上的零件都要忽略碰撞
		foreach (var itemConnectedCollider in partMain.GetIgnorableColliders)
		{
			// 忽略和轴承的碰撞
			if (bearing)
			{
				Physics2D.IgnoreCollision(bearing, itemConnectedCollider);
			}
			foreach (var itemConnectedOtherPart in connectedParts)
			{
				if (partMain == itemConnectedOtherPart.Key)
				{
					continue;
				}
				foreach (var itemConectedOtherCollider in itemConnectedOtherPart.Key.GetIgnorableColliders)
				{
					Physics2D.IgnoreCollision(itemConectedOtherCollider, itemConnectedCollider);
				}
			}
		}
	}
	// --------------- //
	// --  类型
	// --------------- //

	// --------------- //
	// --  私有方法
	// --------------- //

	// --------------- //
	// --  类型
	// --------------- //
}